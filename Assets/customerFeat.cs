using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class customerFeat : MonoBehaviour
{
    public GameObject customerPrefab;
    private GameObject currentCustomer;
    private GameObject moneyTextPrefab;
    private Transform moneyTextTarget;
    private static waffleFeat waffleFeat;
    private Text customerSatisfactionText;

    private GameObject customerOrderTextPrefab;
    private GameObject customerOrderText;

    public Canvas canvas;

    public List<string> order = new List<string>();
    public Dictionary<int, List<string>> orderList = new Dictionary<int, List<string>>();


    private float overallSatisfaction = 100f;
    public float moneyAnimationDuration = 1.0f;

    void Start()
    {
        customerOrderTextPrefab = Resources.Load<GameObject>("CustomerSatisfactionText");
        canvas = FindObjectOfType<Canvas>();

        customerSatisfactionText = GameObject.Find("CustomerSatisfactionText").GetComponent<Text>();

        moneyTextPrefab = GameObject.Find("CurrentMoneyText");
        moneyTextTarget = moneyTextPrefab.transform;

        orderList.Add(1,new List<string>
        {
            "ChocolateLineRendererObject",
            "Strawberry(Clone)",
        });
        orderList.Add(2, new List<string>
        {
            "Strawberry(Clone)",
        });
        orderList.Add(3, new List<string>
        {
            "ChocolateLineRendererObject",
        });
        StartCoroutine(SpawnNewCustomer());

    }

    void setOrder(int key)
    {
        order = orderList[key];
    }

    void Update()
    {

    }

    public static void setWaffleFeat(GameObject waffleObject)
    {
        waffleFeat = waffleObject.GetComponent<waffleFeat>();
        waffleFeat.selectedObj = null;
    }

    public void CustomerGivesMoney(int amount)
    {
        GameObject moneyText = Instantiate(moneyTextPrefab, moneyTextTarget.position, Quaternion.identity, moneyTextTarget.parent);
        RectTransform moneyTextRect = moneyText.GetComponent<RectTransform>();
        moneyTextRect.anchoredPosition = new Vector2(moneyTextRect.anchoredPosition.x, moneyTextRect.anchoredPosition.y - 70); // Canvas üzerindeki pozisyonu ayarla

        Text moneyTextComponent = moneyText.GetComponent<Text>();
        moneyTextComponent.text = "+" + amount.ToString();

        StartCoroutine(MoveMoneyText(moneyText, amount));
    }

    private IEnumerator MoveMoneyText(GameObject moneyText, int amount)
    {
        Vector3 startPosition = moneyText.transform.position;
        Vector3 endPosition = moneyTextTarget.position;

        float elapsedTime = 0;

        while (elapsedTime < moneyAnimationDuration)
        {
            moneyText.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moneyAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moneyTextTarget.GetComponent<Text>().text = (int.Parse(moneyTextTarget.GetComponent<Text>().text) + amount).ToString();

        Destroy(moneyText);
    }

    IEnumerator SpawnNewCustomer()
    {
        setOrder(Random.Range(1, 4));

        yield return new WaitForSeconds(2f);
        Vector3 spawnPosition = new Vector3(-5, 2.9f, -2);
        currentCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
        CreateOrderText(this.order);
        // Debug.Log("Yeni müþteri oluþturuldu: " + currentCustomer);
    }

    public void DeliverProduct()
    {
        if (currentCustomer != null)
        {
            CustomerGivesMoney(50);
            Destroy(currentCustomer);
            Destroy(customerOrderText);

            //Debug.Log("Müþteri ürünü aldý ve ayrýldý.");
            UpdateCustomerSatisfaction();

            StartCoroutine(SpawnNewCustomer());
        }
    }

    public static float CalculateCustomerSatisfaction(Dictionary<string, float> ingredients, List<string> order)
    {
        float totalSatisfaction = 0f;
        float maxSatisfaction = order.Count;

        foreach (var itemName in order)
        {
            if (ingredients.ContainsKey(itemName))
            {
                float availableAmount = ingredients[itemName];
                Debug.Log(itemName);
                Debug.Log(itemName.Contains("LineRenderer"));
                if (itemName.Contains("LineRenderer"))
                {
                    totalSatisfaction += Mathf.Min(availableAmount / 100f, 1f);
                    Debug.Log($"yüzde: {Mathf.Min(availableAmount / 100f, 1f)}");
                }
                else
                {
                    totalSatisfaction += availableAmount >= 10f ? 1f : availableAmount / 10f;
                    Debug.Log($"sayý: {(availableAmount >= 10f ? 1f : availableAmount / 10f)}");
                }
            }
            else
            {
                totalSatisfaction -= 1f;
            }
        }
        foreach (var ingredient in ingredients.Keys)
        {
            if (!order.Contains(ingredient))
            {
                totalSatisfaction -= 0.5f;
            }

            //totalSatisfaction -= ingredients.Keys.Count(ingredient => !order.Contains(ingredient)) * 0.5f;
        }

        float satisfactionPercentage = (totalSatisfaction / maxSatisfaction) * 100f;
        satisfactionPercentage = Mathf.Clamp(satisfactionPercentage, 0f, 100f);
        //Debug.Log("Customer Satisfaction: " + satisfactionPercentage + "%");

        return satisfactionPercentage;
    }

    public void UpdateCustomerSatisfaction()
    {
        float customerSatisfaction = CalculateCustomerSatisfaction(waffleFeat.ingredients, this.order);
        Debug.Log(customerSatisfaction);

        overallSatisfaction = (overallSatisfaction + customerSatisfaction) / 2f;
        Debug.Log(overallSatisfaction);

        if (overallSatisfaction > 80f)
        {
            customerSatisfactionText.text = $":) {overallSatisfaction:F2}";
        }
        else if (overallSatisfaction >= 50f && overallSatisfaction <= 80f)
        {
            customerSatisfactionText.text = $":| {overallSatisfaction:F2}";
        }
        else
        {
            customerSatisfactionText.text = $":( {overallSatisfaction:F2}";
        }
    }

    private void CreateOrderText(List<string> order)
    {
        customerOrderText = Instantiate(customerOrderTextPrefab, canvas.transform);

        Text orderTextComponent = customerOrderText.GetComponent<Text>();

        orderTextComponent.fontSize = 40;

        string orderText = string.Join(" & ", order);
        orderTextComponent.text = orderText;

        Vector3 dinerCopPosition = currentCustomer.transform.position;
        Vector3 orderTextPosition = new Vector3(dinerCopPosition.x + 3.2f, dinerCopPosition.y, dinerCopPosition.z);

        RectTransform rectTransform = customerOrderText.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(716.5f, 145.25f);
        rectTransform.position = orderTextPosition;

    }
}
