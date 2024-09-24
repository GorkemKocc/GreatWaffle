using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binControl : MonoBehaviour
{
    public Transform bottombunObj;
    public Transform topbunObj;
    public Transform meatObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(gameObject.name == "bun bin")
        {
            int index = Array.FindIndex(GamePlay.cuttingBoardSlots, slot => slot == "empty");

            if (index != -1)
            {
                Instantiate(bottombunObj, new Vector3(-4f + (index * 1.5f), -.5f, -1), bottombunObj.rotation);
                Instantiate(topbunObj, new Vector3(-4 + (index * 1.5f), -.3f, -3), topbunObj.rotation);
                GamePlay.cuttingBoardSlots[index] = "justBun";
                GamePlay.selectedSlot = index;
            }
        }

        if (gameObject.name == "hamburgers")
        {
            int index = Array.FindIndex(GamePlay.grillSlots, slot => slot == "empty");

            if (index != -1)
            {
                Instantiate(meatObj, new Vector3(1.5f + index, -.5f, -2), meatObj.rotation);
                GamePlay.grillSlots[index] = "full";
            }
        }
    }
}
