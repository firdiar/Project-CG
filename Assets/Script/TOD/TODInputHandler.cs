using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TODInputHandler : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{

    float time = 0;


    bool isDrag = false;
    Vector2 startPos;

    [SerializeField] GameTODManager manager;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!manager.isCanRollAgain) return;

        isDrag = true;
        time = 0;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!manager.isCanRollAgain || !isDrag) return;

        isDrag = false;
        float length = Vector2.Distance(eventData.position, startPos);

        float power = ((length / Screen.height) * 100) / (time < 0 ? 1 : time);

        if (power < 100) {
            Debug.Log("Power Kurang");
            return;
        }

        manager.RollBottle(power);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDrag)
            time += Time.deltaTime;
    }
}
