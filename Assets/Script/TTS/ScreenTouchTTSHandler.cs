using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTouchTTSHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]BoardGeneratorTTS BGT;
    bool isClick = false;
    Vector2 pos;
    Vector3 delta;

    public void OnDrag(PointerEventData eventData)
    {
        isClick = false;
        delta = eventData.position - pos;

        Vector3 position = Camera.main.transform.position;

        position.x = Mathf.Clamp(Camera.main.transform.position.x - delta.x/(Screen.width/8) , BGT.width.x , BGT.width.y);
        position.y = Mathf.Clamp(Camera.main.transform.position.y - delta.y / (Screen.width / 8), BGT.height.x, BGT.height.y);

        Camera.main.transform.position = position;

        pos = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClick = true;
        pos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isClick) {

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Debug.DrawRay(ray.origin, ray.direction * Camera.main.farClipPlane, Color.red, 5);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Camera.main.farClipPlane);
            if (hit.transform != null)
            {
                BoxTTS btts = hit.transform.GetComponent<BoxTTS>();
                if (btts != null) {
                    BGT.SetObjSelected(btts);
                }
                Debug.Log(hit.transform.name);

            }


            isClick = false;
        }
        
    }
}
