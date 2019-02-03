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

    Vector2 targetPos = Vector2.zero;

    Question _question;
    public Question question {
        set {
            _question = value;
            questionText.text = _question.question;

            StartCoroutine(initButton());
        }
    }

    void Start() {
        targetPos = this.GetComponent<RectTransform>().localPosition;
    }

    IEnumerator initButton() {
        int count = buttonPlace.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(buttonPlace.GetChild(i).gameObject);
        }
        count = _question.answer.Count;
        for (int i = 0; i < count; i++)
        {
            Button b = Instantiate(answerButtonPrefabs, Vector2.zero, Quaternion.identity, buttonPlace).GetComponent<Button>();
            b.transform.GetChild(0).GetComponent<Text>().text = _question.answer[i];
            b.onClick.AddListener(() => Answer(b.transform.GetChild(0).GetComponent<Text>()));
           
            yield return new WaitForSeconds(0.01f);
        }
    }

    

    // Start is called before the first frame update
    public void Show(ResultCard events)
    {
        resultCard += events;
        targetPos = new Vector2(0, 0);
        
    }

    // Update is called once per frame
    public void Hide()
    {
        targetPos = new Vector2(Screen.width*1.5f, 0);
    }

    public void Answer(Text text) {

        resultCard(_question.answer[_question.trueAnswer].Equals(text.text));

        Hide();

        resultCard = null;
    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(this.GetComponent<RectTransform>().localPosition, targetPos));
        if (Vector2.Distance(this.GetComponent<RectTransform>().localPosition, targetPos) > 0.2f)
        {
            this.GetComponent<RectTransform>().localPosition = Vector2.Lerp( this.GetComponent<RectTransform>().localPosition, targetPos, Time.deltaTime * 5);
        }
        

    }

}
