using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cookFood : MonoBehaviour
{
    public float cookingTime = 0;
    bool isCooked = false;
    public int occupiedSlot = -2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isCooked)
            cookingTime += Time.deltaTime;

        if ( cookingTime > 3 && cookingTime < 5)
        {
            GetComponent<SpriteRenderer> ().color = new Color(1,1,0);
        }
        if (cookingTime > 5)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }

        if (occupiedSlot == GamePlay.selectedSandwhich)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(objPosition.x, objPosition.y - .2f, -2);
        }

    }
    private void OnMouseDown()
    {
        Debug.Log("meat" + transform.position.z);

        int bunIndex = Array.FindIndex(GamePlay.cuttingBoardSlots, slot => slot == "justBun");

        if (bunIndex != -1)
        {
            int meatIndex = Mathf.FloorToInt(GetComponent<Transform>().position.x - 1.5f);

            GetComponent<Transform>().position = new Vector3(-4f + (bunIndex * 1.5f), -.5f, -2);
            isCooked = true;

            GamePlay.cuttingBoardSlots[bunIndex] = "fullBun";
            GamePlay.grillSlots[meatIndex] = "empty";
            occupiedSlot = bunIndex;
        }
    }
}
