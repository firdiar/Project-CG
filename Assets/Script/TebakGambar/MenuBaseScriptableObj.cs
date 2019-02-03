using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SoalTebakGambar/Menu", order = 1)]
public class MenuBaseScriptableObj : ScriptableObject
{
    public MenuTebakGambar[] menu;
}

[System.Serializable]
public class MenuTebakGambar
{
    public string caption;
    public string value;
}
