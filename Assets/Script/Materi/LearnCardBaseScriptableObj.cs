using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materi", menuName = "Materi/Isi")]
public class LearnCardBaseScriptableObj : ScriptableObject
{
    public List<LearnCard> learnCard;

}

[System.Serializable]
public class LearnCard {
    public Texture GambarLandscape;
    public Texture GambarKotak;
    [TextArea]
    public string isiCard;
}
