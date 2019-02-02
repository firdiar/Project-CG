using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class GameTebakGambarManager : MonoBehaviour
{

    string _currentText = "";
    public string currentText { get { return _currentText; } set { _currentText = value; UpdateAnswer(); } }


    [Header("MenuScreen")]
    [SerializeField] GameObject Menu;
    [SerializeField]Transform contentScrollView;
    [SerializeField]MenuBaseScriptableObj data;
    [SerializeField] GameObject buttonPrefabs;
    [SerializeField] string currentValueBtn ="";
    [SerializeField]GameObject OptionMenu;
    GameObject tempButton = null;


    [Header("GameScreen")]
    [SerializeField] GameObject game;
    [SerializeField] Text AnswerText;

    [SerializeField] RectTransform[] img;
    [SerializeField] GameObject OptionGame;
    int currentImg = 0;
    bool isMove = false;
    Vector2 target = Vector2.zero;

    int currentScreen = 0;

    SoalBaseScriptableObj soal;

    int currentIndexSoal;

    public void ChangeScreen(int idx) {
        currentScreen = idx;
        switch (idx) {
            case 0:
                
                Menu.SetActive(true);
                game.SetActive(false);
             
                break;
            case 1:
                Menu.SetActive(false);
                game.SetActive(true);
               
                break;
        }

    }

    private void Start()
    {
        ChangeScreen(0);

        int i = 0;
        foreach (MenuTebakGambar m in data.menu) {
            GameObject btn = Instantiate(buttonPrefabs, Vector3.zero, Quaternion.identity, contentScrollView);
            btn.transform.GetChild(1).GetComponent<Text>().text = m.caption;
            btn.GetComponent<Button>().onClick.AddListener(()=> {
                if (tempButton != null && tempButton != btn.gameObject)
                {
                    tempButton.transform.GetChild(0).gameObject.SetActive(false);
                }
                else if (tempButton == btn.gameObject) {
                    return;
                }
                currentValueBtn = m.value;
                Debug.Log(m.value);
                btn.transform.GetChild(0).gameObject.SetActive(true);
                tempButton = btn.gameObject;
            } );
        }
    }

    public void Play() {

        Debug.Log("Playing : "+ currentValueBtn);
        StartGame(Resources.Load<SoalBaseScriptableObj>("Data/TebakGambar/"+currentValueBtn));
        ChangeScreen(1);
    }

    // Start is called before the first frame update
    void StartGame(SoalBaseScriptableObj soal )
    {

        this.soal = soal;
        currentIndexSoal = -1;
        ShowNext();
    }

    void ShowNext() {

        if (isMove) return;

        currentIndexSoal = Mathf.Clamp(currentIndexSoal + 1 , 0 , soal.soal.Length-1);

        img[currentImg].SetAsFirstSibling();

        target = new Vector2(0, img[currentImg].localPosition.y);
        isMove = true;
        currentImg++;
        if (currentImg == 2)
        {
            currentImg = 0;
        }
        Debug.Log(currentIndexSoal);
        img[currentImg].GetChild(0).GetComponent<RawImage>().texture = soal.soal[currentIndexSoal].texture;


    }

    private void Update()
    {
        if (isMove) {

            img[currentImg].localPosition = Vector2.MoveTowards(img[currentImg].localPosition, target , Time.deltaTime * Screen.width*4.5f);

            if (Vector2.Distance(img[currentImg].localPosition, target) == 0) {
                isMove = false;
                target = Vector2.zero;
            
                img[(currentImg==0? 1 : 0)].localPosition = new Vector2(Screen.width*2, img[currentImg].localPosition.y);
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (currentScreen) {
                case 0:
                    OptionMenu.SetActive(true);
                    break;
                case 1:
                    OptionGame.SetActive(true);
                    break;
            }
        }
    }

    public void BackToHome() {
        //kembali ke halaman awal
    }


    void UpdateAnswer() {
        AnswerText.text = currentText;
    }

    public void Inputs(Text text)
    {
        if (!text.text.Equals("DEL") && !text.text.Equals("CLEAR"))
        {
            currentText += text.text;
        }
        else if (text.text.Equals("DEL"))
        {
            currentText = currentText.Substring(0, Mathf.Clamp(currentText.Length - 1, 0, currentText.Length));
        }
        else if (text.text.Equals("CLEAR"))
        {
            currentText = "";
        }
    }

    public void CEK() {

        if (currentText.ToUpper().Equals(soal.soal[currentIndexSoal].answer.ToUpper())) // ketika benar
        {
          
            if (currentIndexSoal < soal.soal.Length - 1)
            {
                currentText = "";
                ShowNext();
            }
            else {
                Debug.Log("GameFinish");
                ChangeScreen(0);
            }
           

        }
        else {//ketika salah
           
            Debug.Log("Jawaban Salah");
        }
        
    }



}
