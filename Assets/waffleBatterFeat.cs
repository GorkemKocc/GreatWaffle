using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waffleBatterFeat : MonoBehaviour
{
    public bool mouseControlled = false;
    public GameObject waffleMachinePrefab;
    public cookerFeat cooker;
    void Start()
    {
        cooker = waffleMachinePrefab.GetComponent<cookerFeat>();

        transform.position = new Vector3(transform.position.x, transform.position.y, -4);
    }

    void Update()
    {
        if (mouseControlled)
        {
            Vector2 objPosition = GamePlay.findMouseScreenPoint();
            transform.position = new Vector3(objPosition.x, objPosition.y, -4);
        }
    }
    private void OnMouseDown()
    {
        mouseControlled = true;
    }

    private bool IsOverlappingWithWaffleMachine()
    {
        Collider2D waffleMachineCollider = waffleMachinePrefab.GetComponent<Collider2D>();
        Collider2D waffleBatterCollider = GetComponent<Collider2D>();

        float distance = Vector3.Distance(waffleBatterCollider.bounds.center, waffleMachineCollider.bounds.center);

        float collisionTolerance = 2.0f;
        bool isOverlapping = distance < collisionTolerance;
        //Debug.Log(isOverlapping);
        return isOverlapping;
    }

    private void OnMouseUp()
    {
        if (IsOverlappingWithWaffleMachine())
        {
            //Debug.Log("Waffle hamuru waffle machine'e dokundu!");
            cooker.cooking(); 
        }
        if (transform.position != new Vector3(-6, -1, -4))
        {
            transform.position = new Vector3(-6, -1, -4);
            mouseControlled = false;
        }
    }

}
