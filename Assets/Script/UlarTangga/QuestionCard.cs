using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class QuestionCard : MonoBehaviour
{

    [SerializeField]Text questionText = null;
    [SerializeField]RectTransform buttonPlace = null;
    [SerializeField]GameObject answerButtonPrefabs = null;
    public delegate void ResultCard(bool res);
    event ResultCard resultCard;

    Question _question;
    public Question question {
        set {

            

            _question = value;
            questionText.text = _question.question;

            StartCoroutine(initButton());

        }
    }

    IEnumerator initButton() {
        int count = buttonPlace.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(buttonPlace.GetChild(i).gameObject);
        }
        count = _question.answer.Length;
        for (int i = 0; i < count; i++)
        {
            Instantiate(answerButtonPrefabs, Vector2.zero, Quaternion.identity, buttonPlace);
            yield return new WaitForSeconds(0.1f);
        }
    }

    

    // Start is called before the first frame update
    public void Show(ResultCard events)
    {
        resultCard += events;

    }

    // Update is called once per frame
    public void Hide()
    {
        
    }

    public void Answer(Text text) {

        resultCard(_question.answer[_question.TrueAnswer].Equals(text.text));

        Hide();

        resultCard = null;
    }

}
