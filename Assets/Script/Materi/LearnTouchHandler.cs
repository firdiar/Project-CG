using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LearnTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    Vector2 startPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        startPos = eventData.position - startPos;

        if (startPos.x > 0)
        {
            MateriManager.MAIN.prev();
        }
        else if (startPos.x < 0) {

            MateriManager.MAIN.next();
        }
    }
}
