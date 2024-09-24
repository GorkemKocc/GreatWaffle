using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chocolateBinControl : MonoBehaviour
{
    public bool isSelected = false;
    private waffleFeat waffleFeat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
                waffleFeat.selectedObj = "Chocolate";
                waffleFeat.CreateNewLineRenderer("Chocolate", ref waffleFeat.chocolateLineRenderer, new Color(0.65f, 0.16f, 0.16f));
                StartCoroutine(waffleFeat.BlinkBorder());
            }
            else if (waffleFeat.selectedObj == "Chocolate")
            {
                waffleFeat.selectedObj = null;
                waffleFeat.points = new List<Vector3>();
                isSelected = false;
            }
        }
    }
}
