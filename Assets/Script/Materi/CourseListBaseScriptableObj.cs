using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materi", menuName = "Materi/Course")]
public class CourseListBaseScriptableObj : ScriptableObject
{
    public List<CourseList> list;
}

[System.Serializable]
public class CourseList {
    public Texture texture;
    public string courseName;
    public List<ObjectLesson> dataRef;
}
