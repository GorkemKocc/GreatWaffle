using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBottomBun : MonoBehaviour
{
    public bool mouseControlled = false;

    public int occupiedSlot = -1;

    void Start()
    {
        occupiedSlot = GamePlay.selectedSlot;
    }

    void Update()
    {
        if (occupiedSlot == GamePlay.selectedSandwhich)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(objPosition.x, objPosition.y - .2f, -1);
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("bottom" + transform.position.z);

    }
}
