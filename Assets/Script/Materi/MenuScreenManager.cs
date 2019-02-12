using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreenManager : MonoBehaviour
{
    [SerializeField] CourseListBaseScriptableObj data;
    [SerializeField] GameObject Menu1;
    [SerializeField] GameObject coursePrefabs;
    [SerializeField] Transform courseParent;
    [SerializeField] GameObject Menu2;
    [SerializeField] GameObject lessonPrefabs;
    [SerializeField] Transform lessonParent;
    [SerializeField] SoundBase sound;

    bool inProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        //data = Resources.Load<CourseListBaseScriptableObj>("Data/Materi/Materi");

        initializeObj();
    }

    void initializeObj() {
        ChangeActiveScreen(0);
        foreach (CourseList cl in data.list) {
            GameObject c = Instantiate(coursePrefabs, courseParent);
            c.GetComponent<Button>().onClick.AddListener( ()=> {
                if (inProgress)
                    return;

                GenerateLesson(cl.dataRef);
                sound.PlaySound("Klik");
                HideMenu();

            } );
            c.transform.GetChild(1).GetComponent<Text>().text = cl.courseName;
            c.GetComponent<RawImage>().texture = cl.texture;
        }


    }

    public void ChangeActiveScreen(int idx) {
        if (idx == 0)
        {
            Menu1.SetActive(true);
            Menu2.SetActive(false);
        }
        else {

            Menu2.SetActive(true);
            Menu1.SetActive(false);
        }
    }

    public void HideLesson() {
        if (inProgress)
            return;

        inProgress = true;
        Vector3 target = transform.GetChild(1).localRotation.eulerAngles;
        target.y = 90;
        StartCoroutine(Show(transform.GetChild(1).GetComponent<RectTransform>(), target, () => {
            ShowMenu();
            int count = lessonParent.childCount;
            Debug.Log(count);
            for (int i = 0; i < count; i++)
            {
                Destroy(lessonParent.GetChild(i).gameObject);
            }
        }));
    }
    void ShowLesson() {
        
        Vector3 target = transform.GetChild(1).localRotation.eulerAngles;
        target.y = 90;
        transform.GetChild(1).localRotation = Quaternion.EulerAngles(target);
        ChangeActiveScreen(1);
        target.y = 0;
        StartCoroutine(Show(transform.GetChild(1).GetComponent<RectTransform>(), target, () => { inProgress = false; }));
    }

    public void HideMenu() {
        if (inProgress)
            return;

        inProgress = true;
        Vector3 target = transform.GetChild(0).localRotation.eulerAngles;
        target.y = 90;
        StartCoroutine(Show(transform.GetChild(0).GetComponent<RectTransform>(), target, () => { ShowLesson(); } ));
    }
    void ShowMenu() {
       
        Vector3 target = transform.GetChild(0).localRotation.eulerAngles;
        target.y = 90;
        transform.GetChild(1).localRotation = Quaternion.EulerAngles(target);
        ChangeActiveScreen(0);
        target.y = 0;
        StartCoroutine(Show(transform.GetChild(0).GetComponent<RectTransform>(), target, () => { inProgress = false; }));


    }

    void GenerateLesson(List<ObjectLesson> lessonData) {

        

        foreach (ObjectLesson ol in lessonData) {
            GameObject c = Instantiate(lessonPrefabs, lessonParent);
            c.transform.GetChild(0).GetComponent<Text>().text = ol.lessonName;
            c.GetComponent<Button>().onClick.AddListener(() => {
                sound.PlaySound("Klik");
                MateriManager.MAIN.LoadData(ol.lessonData);

                MateriManager.MAIN.ChangeScreen(1);
            });
        }
    }

    IEnumerator Show(RectTransform rot , Vector3 target , UnityEngine.Events.UnityAction call) {
        
        while (Vector3.Distance(rot.localRotation.eulerAngles , target) > 1) {
            rot.localRotation = Quaternion.Euler(Vector3.Lerp(rot.localRotation.eulerAngles, target, 0.3f));
            yield return new WaitForSeconds(0.02f);
        }
        call();
        
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
