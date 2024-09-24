using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBun : MonoBehaviour
{
    public bool mouseControlled = false;

    public int occupiedSlot = -1;

    void Start()
    {
        occupiedSlot = GamePlay.selectedSlot;
    }

    void Update()
    {
        if (mouseControlled)
        {
            GamePlay.selectedSandwhich = occupiedSlot;
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(objPosition.x, objPosition.y, -3);
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("top" + transform.position.z);
        mouseControlled = true;
        GamePlay.selectedSandwhich = occupiedSlot;
    }

    private void OnMouseUp()
    {
        mouseControlled = false;
        GamePlay.selectedSandwhich = -1;
    }
}
