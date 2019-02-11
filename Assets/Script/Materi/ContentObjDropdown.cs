using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ContentObjDropdown : MonoBehaviour
{

    List<ObjectCourse> data = new List<ObjectCourse>();
    [SerializeField] GameObject coursePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    ObjectCourse oc = new ObjectCourse();
        //    oc.courseName = "test";
        //    oc.lessons = new List<ObjectLesson>();
        //    oc.lessons.Add(new ObjectLesson("Lesson 1", "data1"));
        //    oc.lessons.Add(new ObjectLesson("Lesson 2", "data2"));
        //    oc.lessons.Add(new ObjectLesson("Lesson 2", "data2"));
        //    oc.lessons.Add(new ObjectLesson("Lesson 2", "data2"));
        //    data.Add(oc);
        //}

        string a = Resources.Load<TextAsset>("Data/Materi/Course").text;

        data = JsonConvert.DeserializeObject<List<ObjectCourse>>(a);

        initializeObj();
    }

    void initializeObj()
    {
        //this.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().spacing = 50 * (Screen.height / 1280f);
        foreach (ObjectCourse oc in data) {
            CourseObjectDropdown COD = Instantiate(coursePrefabs, Vector3.zero, Quaternion.identity, this.transform).GetComponent<CourseObjectDropdown>();
            COD.data = oc;
            //COD.transform.localScale *= Screen.width / 720f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
