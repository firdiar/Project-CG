using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soal", menuName = "SoalTebakGambar/Soal", order = 1)]
public class SoalBaseScriptableObj : ScriptableObject
{
    public SoalTebakGambar[] soal;
}

[System.Serializable]
public class SoalTebakGambar {
    public Texture texture;
    public string answer;
}
