using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{

    public static string[] cuttingBoardSlots = { "empty", "empty", "empty" };

    public static string[] grillSlots = { "empty", "empty", "empty" };

    public static int selectedSlot = -1;
    public static int selectedSandwhich = -1;

    public static bool isCookerEmpty = true;  
    public static bool waffle = false;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Vector2 findMouseScreenPoint()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return objPosition;
    }
    public static List<GameObject> GetChildrenWithName(GameObject parent, string targetName)
    {
        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (child.name == targetName)
            {
                matchingObjects.Add(child.gameObject);
            }
        }

        return matchingObjects;
    }
}
