using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SWCItem : MonoBehaviour
{
    public string itemTag;

    float startPosx;
    float startPosY;
    bool isBeingHeld = false;

    SWCMission swc;

    public void Init(SWCMission _base)
    {
        swc = _base;
    }

    private void Update()
    {
        if (isBeingHeld)
        {
            Vector2 mousePos;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            this.gameObject.transform.position = new Vector2(mousePos.x - startPosx, mousePos.y - startPosY);
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Debug.Log("클릭");

        Vector3 mousePos;
        mousePos = GameManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);

        startPosx = mousePos.x - this.transform.position.x;
        startPosY = mousePos.y - this.transform.position.y;

        isBeingHeld = true;
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(itemTag))
        {
            Debug.Log("정확");
            swc.Success();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(itemTag))
        {
            swc.Minus();
        }
    }
}