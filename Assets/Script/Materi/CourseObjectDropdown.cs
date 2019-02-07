using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectCourse {
    public string courseName;

    public List<ObjectLesson> lessons;
    
}
[System.Serializable]
public class ObjectLesson {
    public ObjectLesson(string lessonName , string lessonData) {
        this.lessonName = lessonName;
        this.lessonData = lessonData;
    }
    public string lessonName;
    public string lessonData;
}

public class CourseObjectDropdown : MonoBehaviour
{
    [SerializeField] GameObject objPrefabs;
    [SerializeField] RectTransform objPlace;
    bool isChangeSize = false;
    bool isOut = false;
    bool isOnProcess = false;
    [SerializeField] RectTransform rotator;
    [SerializeField] RectTransform objSizer;
    [SerializeField] RectTransform place;
    //[SerializeField] float objHeight;


    [SerializeField] Text courseText;

    ObjectCourse _data;
    public ObjectCourse data {
        get { return _data; }
        set {
            _data = value;
            initializeObj();
        }
    }



    void initializeObj() {
        for (int i = 0; i < _data.lessons.Count; i++) {
            GameObject tex = Instantiate(objPrefabs, objPlace.position, Quaternion.identity, objPlace);
            string temp = data.lessons[i].lessonData;
            tex.transform.GetChild(0).GetComponent<Text>().text = data.lessons[i].lessonName;
            tex.GetComponent<Button>().onClick.AddListener(()=> {
                Debug.Log(temp);
                MateriManager.MAIN.LoadData(temp);
                MateriManager.MAIN.ChangeScreen(1);
            });
        }
        courseText.text = data.courseName;

        
    }


    // Start is called before the first frame update
    public void Show()
    {
        if (isOnProcess) return;
        isChangeSize = true;

        Vector2 targetSize;
        List<Vector2> tempTarget = new List<Vector2>();
        Vector3 tempEuler;
        if (!isOut)
        {
            targetSize = new Vector2(objSizer.sizeDelta.x, objPlace.sizeDelta.y + objPlace.childCount * objPlace.sizeDelta.y);


            for (int i = 0; i < objPlace.childCount; i++)
            {
                tempTarget.Add(new Vector2(objPlace.GetChild(i).localPosition.x, objPlace.GetChild(i).localPosition.y - objPlace.sizeDelta.y * (i + 1)));
            }

            tempEuler = new Vector3(0, 0, 180);
            
        }
        else {
            targetSize = new Vector2(objSizer.sizeDelta.x, objPlace.sizeDelta.y);


            for (int i = 0; i < objPlace.childCount; i++)
            {
                tempTarget.Add(Vector2.zero);
            }
            tempEuler = Vector3.zero;

        }
        
        isOut = !isOut;

       
        StartCoroutine(Showing( targetSize,tempTarget , tempEuler ));
    }

    IEnumerator Showing(Vector2 targetSize , List<Vector2> tempTarget , Vector3 tempEuler) {
        isOnProcess = true;

        while (isChangeSize)
        {
            
            rotator.eulerAngles = Vector3.Lerp(rotator.eulerAngles , tempEuler , 5*Time.deltaTime);

            objSizer.sizeDelta = Vector2.Lerp(objSizer.sizeDelta, targetSize, 12*Time.deltaTime);

            

            for (int i = 0; i < objPlace.childCount; i++)
            {
                objPlace.GetChild(i).localPosition = Vector2.Lerp(objPlace.GetChild(i).localPosition, tempTarget[i], 5*Time.deltaTime);
            }

            if (Vector2.Distance(objPlace.GetChild(objPlace.childCount-1).localPosition, tempTarget[objPlace.childCount-1]) <= 0.5f)
            {
                isChangeSize = false;
            }

            yield return null;
        }

        Debug.Log("Done");
        isOnProcess = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
