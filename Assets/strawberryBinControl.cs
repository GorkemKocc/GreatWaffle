using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class strawberryBinControl : MonoBehaviour
{
    public bool isSelected = false;
    private waffleFeat waffleFeat;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void setWaffleFeat(GameObject waffleObject)
    {
        this.waffleFeat = waffleObject.GetComponent<waffleFeat>();
        waffleFeat.selectedObj = null;
    }

    private void OnMouseDown()
    {
        if (waffleFeat != null)
        {
            if (!isSelected && waffleFeat.selectedObj == null)
            {
                isSelected = true;
                waffleFeat.selectedObj = "Strawberry";
                StartCoroutine(waffleFeat.BlinkBorder());
            }
            else if (waffleFeat.selectedObj == "Strawberry")
            {
                waffleFeat.selectedObj = null;
                isSelected = false;
            }
        }
    }
}
