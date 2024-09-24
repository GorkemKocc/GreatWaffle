using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class waffleFeat : MonoBehaviour
{
    public GameObject strawberryPrefab;
    public GameObject customerPrefab;
    public customerFeat customerFeatScript;
    public GameObject moneyTextPrefab;
    public Transform moneyTextTarget;
    public LineRenderer chocolateLineRenderer;

    public List<Vector3> points = new List<Vector3>();

    public bool mouseControlled = false;
    public string selectedObj;
    public float blinkDuration = 0.5f;
    public int blinkCount = 30;
    private Color originalColor;
    public Dictionary<string, float> ingredients = new Dictionary<string, float>();
    private int waffleArea = 0;

    void Start()
    {
        originalColor = GetComponent<SpriteRenderer>().color;

        if (customerFeatScript == null)
        {
            customerFeatScript = FindObjectOfType<customerFeat>();
        }
        if(waffleArea == 0)
        {
            waffleArea = CalculateWaffleArea();
            Debug.Log("Toplam nokta sayýsý: " + waffleArea);
        }
        if (moneyTextTarget == null)
        {
            GameObject moneyTextObject = GameObject.Find("CurrentMoneyText");
            if (moneyTextObject != null)
            {
                moneyTextTarget = moneyTextObject.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("CurrentMoneyText GameObject not found in the scene.");
            }
        }


    }

    public void CreateNewLineRenderer(string objectName, ref LineRenderer lineRenderer, Color color)
    {
        GameObject LineRendererObject = new GameObject(objectName + "LineRendererObject");
        lineRenderer = LineRendererObject.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;

        SpriteRenderer waffleRenderer = GetComponent<SpriteRenderer>();

        lineRenderer.sortingLayerID = waffleRenderer.sortingLayerID;
        lineRenderer.sortingOrder = waffleRenderer.sortingOrder + 1;

        //int maxSortingOrder = FindObjectsOfType<Renderer>().Max(r => r.sortingOrder);
        //lineRenderer.sortingOrder = maxSortingOrder + 1;

        GameObject waffleObject = GameObject.Find("waffle(Clone)");
        if (waffleObject != null)
        {
            LineRendererObject.transform.SetParent(waffleObject.transform);
            //Debug.Log("LineRenderer parent olarak waffle ayarlandý.");
        }

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        Material lineMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/" + objectName + "LineMaterial.mat", typeof(Material));
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
        else
        {
            Debug.LogError("ChocolateLineMaterial yüklenemedi. Materyalin isminin ve dosya yolunun doðru olduðundan emin olun.");
        }
    }
    void Update()
    {
        if (mouseControlled)
        {
            Vector2 objPosition = GamePlay.findMouseScreenPoint();
            transform.position = new Vector3(objPosition.x, objPosition.y, -3);
        }
    }

    public IEnumerator BlinkBorder()
    {
        while (selectedObj != null)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(blinkDuration);
            GetComponent<SpriteRenderer>().color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
        GetComponent<SpriteRenderer>().color = originalColor;
    }

    private bool IsOverlappingWithCustomer()
    {
        Collider2D customer = customerPrefab.GetComponent<Collider2D>();
        Collider2D waffleCollider = GetComponent<Collider2D>();

        float distance = Vector3.Distance(waffleCollider.bounds.center, customer.bounds.center);

        float collisionTolerance = 2.0f;
        bool isOverlapping = distance < collisionTolerance;
        //Debug.Log(isOverlapping);
        return isOverlapping;
    }

    private void OnMouseDrag()
    {
        if (selectedObj == "Chocolate")
        {
            DrawChocolateLine();
        }
    }

    private void OnMouseDown()
    {
        if (selectedObj == "Strawberry")
        {
            Vector2 objPosition = GamePlay.findMouseScreenPoint();
            GameObject newStrawberry = Instantiate(strawberryPrefab, new Vector3(objPosition.x, objPosition.y, -4), Quaternion.identity);

            newStrawberry.transform.SetParent(this.transform);

            SpriteRenderer strawberryRenderer = newStrawberry.GetComponent<SpriteRenderer>();

            SpriteRenderer waffleRenderer = GetComponent<SpriteRenderer>();

            strawberryRenderer.sortingLayerID = waffleRenderer.sortingLayerID;
            strawberryRenderer.sortingOrder = waffleRenderer.sortingOrder + 2;

            //int maxSortingOrder = FindObjectsOfType<Renderer>().Max(r => r.sortingOrder);
            //strawberryRenderer.sortingOrder = maxSortingOrder + 1;

            DecreaseMoney(1);
        }
        else if (selectedObj == null)
        {
            mouseControlled = true;
        }
    }

    private void OnMouseUp()
    {
        if (IsOverlappingWithCustomer())
        {
            FindIngredients();
            customerFeat.CalculateCustomerSatisfaction(ingredients, customerFeatScript.order);

            Destroy(gameObject);
            customerFeatScript.DeliverProduct();
            GamePlay.waffle = false;
            mouseControlled = false;
        }
        else if (transform.position != new Vector3(-0.33f, -1.05f, -3))
        {
            transform.position = new Vector3(-0.33f, -1.05f, -3);
        }
        else if (selectedObj == "Chocolate")
        {
            CreateNewLineRenderer("Chocolate", ref chocolateLineRenderer, new Color(0.65f, 0.16f, 0.16f));
            points = new List<Vector3>();
        }
        mouseControlled = false;
    }

    private void DecreaseMoney(int amount)
    {
        GameObject moneyText = Instantiate(moneyTextPrefab, moneyTextTarget.position, Quaternion.identity, moneyTextTarget.parent);
        RectTransform moneyTextRect = moneyText.GetComponent<RectTransform>();

        moneyTextRect.anchoredPosition = new Vector2(moneyTextRect.anchoredPosition.x, moneyTextRect.anchoredPosition.y - 20);

        Text moneyTextComponent = moneyText.GetComponent<Text>();
        moneyTextComponent.text = "-" + amount.ToString();

        StartCoroutine(MoveMoneyText(moneyText, amount));
    }

    private IEnumerator MoveMoneyText(GameObject moneyText, int amount)
    {
        Vector3 startPosition = moneyText.transform.position;
        Vector3 endPosition = moneyTextTarget.position - new Vector3(0, 0.3f, 0);

        float elapsedTime = 0;
        float moneyAnimationDuration = .7f;

        int currentMoney = int.Parse(moneyTextTarget.GetComponent<Text>().text);
        moneyTextTarget.GetComponent<Text>().text = (currentMoney - amount).ToString();

        while (elapsedTime < moneyAnimationDuration)
        {
            moneyText.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moneyAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(moneyText);
    }

    private void DrawChocolateLine()
    {
        Vector2 mousePosition = GamePlay.findMouseScreenPoint();
        Vector3 localPosition = new Vector3(mousePosition.x, mousePosition.y, -4);
        
        Collider2D waffleCollider = GetComponent<Collider2D>();
        if (waffleCollider.OverlapPoint(mousePosition))
        {
            points.Add(localPosition);

            chocolateLineRenderer.positionCount = points.Count;
            chocolateLineRenderer.SetPositions(points.ToArray());

            DecreaseMoney(1);
        }
    }

    public int CalculateWaffleArea()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider == null)
        {
            Debug.LogError("circleCollider bulunamadý.");
            return 0;
        }

        Vector2 center = circleCollider.transform.position + (Vector3)circleCollider.offset;
        float radius = circleCollider.radius * circleCollider.transform.localScale.x;

        int pointCount = 0;

        for (float x = center.x - radius; x <= center.x + radius; x += 0.1f)
        {
            for (float y = center.y - radius; y <= center.y + radius; y += 0.1f)
            {
                Vector2 point = new Vector2(x, y);
                if (Vector2.Distance(point, center) <= radius)
                {
                    pointCount++;
                }
            }
        }
        return pointCount;
    }

    public float CalculateTotalPaintedPercentage(string lineRendererObjectName)
    {
        SpriteRenderer waffleRenderer = GetComponent<SpriteRenderer>();

        if (waffleRenderer == null)
        {
            Debug.LogError("waffleRenderer bulunamadý.");
            return 0f;
        }

        List<GameObject> lineRendererObject = GamePlay.GetChildrenWithName(this.gameObject, lineRendererObjectName);
        List<LineRenderer> lineRenderers = lineRendererObject
            .Select(obj => obj.GetComponent<LineRenderer>())
            .Where(lr => lr != null)
            .ToList();

        HashSet<Vector2> uniquePoints = new HashSet<Vector2>();

        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 localPoint = lineRenderer.GetPosition(i);

                Vector3 worldPoint = lineRenderer.transform.TransformPoint(localPoint);
                Vector3 pointToCheck = new Vector3(worldPoint.x, worldPoint.y, waffleRenderer.transform.position.z);

                if (waffleRenderer.bounds.Contains(pointToCheck))
                {
                    Vector2 roundedPoint = new Vector2(Mathf.Round(worldPoint.x * 10f) / 10f,
                                                       Mathf.Round(worldPoint.y * 10f) / 10f);
                    uniquePoints.Add(roundedPoint);
                }
                else
                {
                    Debug.Log("Point is outside bounds." + lineRenderer.positionCount);
                }
            }
        }

        float paintedArea = uniquePoints.Count / (float)waffleArea;

        //Debug.Log("Unique points count: " + uniquePoints.Count);
        //Debug.Log("Yüzde: " + paintedArea * 100f);

        return paintedArea * 100f;
    }
    void FindIngredients()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child == transform) continue;

            string ingredientName = child.name;

            if (ingredientName.Contains("ChocolateLineRendererObject"))
            {
                if (ingredients.ContainsKey(ingredientName)) continue;

                ingredients[ingredientName] = CalculateTotalPaintedPercentage(ingredientName);
            }
            else 
            {
                ingredients[ingredientName] = ingredients.ContainsKey(ingredientName) ? ingredients[ingredientName] + 1 : 1;
            }
        }

        foreach (var ingredient in ingredients)
        {
            Debug.Log("Ingredient: " + ingredient.Key + ", Amount: " + ingredient.Value);
        }
    }

}
