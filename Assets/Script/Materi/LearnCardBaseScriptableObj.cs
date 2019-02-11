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

    public CardType type = CardType.Materi;

    [Header("Materi")]
    public Texture GambarLandscape;
    public Texture GambarKotak;
    [TextArea]
    public string isiCard;

    [Header("Soal")]
    [TextArea]
    public string soal;

    public Answer A;
    public Answer B;
    public Answer C;
    public Answer D;
    [TextArea]
    public string ifTrueMsg;
    [TextArea]
    public string ifFalseMsg;

}

public enum CardType {
    Materi,
    Soal
}

[System.Serializable]
public class Answer {
    public string value;
    public bool isTrue;
}
