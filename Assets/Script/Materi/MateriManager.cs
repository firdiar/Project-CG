using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class MateriManager : MonoBehaviour
{
    public static MateriManager MAIN;

    [Header("MenuScreen")]
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject MenuOption;

    [Header("LearnScreen")]
    [SerializeField] GameObject LearnScreen;
    [SerializeField] GameObject CardMateriPrefabs;
    [SerializeField] GameObject CardSoalPrefabs;
    [SerializeField] GameObject CardSelesaiPrefabs;
    [SerializeField] RectTransform start;
    [SerializeField] RectTransform current;
    [SerializeField] RectTransform end;
    [SerializeField] GameObject LearnOption;
    int index;

    bool isMoved = false;

    LearnCardBaseScriptableObj _dataLearn;
    public LearnCardBaseScriptableObj dataLearn {
        get { return _dataLearn; }
        set {
            _dataLearn = value;
            initializeCard();
        }
    }

    private void Start()
    {
        MAIN = this;
        ChangeScreen(0);
    }

    public void LoadData(string jsonName) {



       dataLearn = Resources.Load<LearnCardBaseScriptableObj>("Data/Materi/" + jsonName);

        //dataLearn = JsonConvert.DeserializeObject<List<string>>(a);
        //Debug.Log("Success");

        //Debug.Log("Fail");
        //List<string> dataLearn = new List<string>();
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //dataLearn.Add("Lorem ipsum");
        //this.dataLearn = dataLearn;

    }

    public void ChangeScreen(int j) {

        switch (j) {
            case 0:
                menuScreen.SetActive(true);
                LearnScreen.SetActive(false);
                for (int i =0; i<start.childCount; i++) {
                    Destroy(start.GetChild(i).gameObject);
                }
                for (int i = 0; i < current.childCount; i++)
                {
                    Destroy(current.GetChild(i).gameObject);
                }
                for (int i = 0; i < end.childCount; i++)
                {
                    Destroy(end.GetChild(i).gameObject);
                }
                break;
            case 1:
                menuScreen.SetActive(false);
                LearnScreen.SetActive(true);
                index = 0;
                break;
        }

    }

    void initializeCard() {
        
        foreach (LearnCard s in dataLearn.learnCard) {
            

            if (s.type == CardType.Materi)
            {
                GameObject g = Instantiate(CardMateriPrefabs, start.position, Quaternion.identity, start);
                if (s.GambarLandscape != null)
                {
                    g.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>().texture = s.GambarLandscape;
                    g.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = s.isiCard;
                    g.transform.GetChild(2).gameObject.SetActive(false);
                    g.transform.GetChild(3).gameObject.SetActive(false);
                }
                else if (s.GambarKotak != null)
                {
                    g.transform.GetChild(1).gameObject.SetActive(false);
                    g.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = s.isiCard;
                    g.transform.GetChild(2).GetComponent<UnityEngine.UI.RawImage>().texture = s.GambarKotak;
                    g.transform.GetChild(3).gameObject.SetActive(false);
                }
                else
                {
                    g.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = s.isiCard;
                    g.transform.GetChild(1).gameObject.SetActive(false);
                    g.transform.GetChild(0).gameObject.SetActive(false);
                    g.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
            else {
                GameObject g = Instantiate(CardSoalPrefabs,  start);
                LearnCard so = s;
                g.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.soal;

               

                g.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.ifTrueMsg;
                g.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.ifFalseMsg;

                g.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.A.value;

                g.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> {
                    
                    if (so.A.isTrue)
                        g.transform.GetChild(1).gameObject.SetActive(true);
                    else
                        g.transform.GetChild(2).gameObject.SetActive(true);

                    g.transform.GetChild(0).gameObject.SetActive(false);
                } );

                g.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.B.value;

                g.transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                    
                    if (so.B.isTrue)
                        g.transform.GetChild(1).gameObject.SetActive(true);
                    else
                        g.transform.GetChild(2).gameObject.SetActive(true);
                    g.transform.GetChild(0).gameObject.SetActive(false);
                });

                g.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.C.value;

                g.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                    
                    if (so.C.isTrue)
                        g.transform.GetChild(1).gameObject.SetActive(true);
                    else
                        g.transform.GetChild(2).gameObject.SetActive(true);
                    g.transform.GetChild(0).gameObject.SetActive(false);
                });

                g.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = so.D.value;

                g.transform.GetChild(0).GetChild(4).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                    
                    if (so.D.isTrue)
                        g.transform.GetChild(1).gameObject.SetActive(true);
                    else
                        g.transform.GetChild(2).gameObject.SetActive(true);
                    g.transform.GetChild(0).gameObject.SetActive(false);
                });

            }

        }
        GameObject go = Instantiate(CardSelesaiPrefabs,  start);
        go.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { ChangeScreen(0); });
        next();
    }
    public void next() {
        if (start.childCount == 0 || (current.childCount >= 1 && current.GetChild(0).gameObject.name.Contains("Soal") && current.GetChild(0).GetChild(0).gameObject.activeInHierarchy)) {
            return;
        }

        isMoved = true;
        if (current.childCount >= 1)
        {
            Transform rt = current.GetChild(0);
            rt.SetParent(end);
        }
        if (start.childCount >= 1) {
            Transform rt = start.GetChild(0);
            rt.SetParent(current);
        }
        
    }
    public void prev()
    {
        if (end.childCount == 0)
        {
            return;
        }
        isMoved = true;
        if (current.childCount >= 1)
        {
            Transform rt = current.GetChild(0);
            rt.SetParent(start);
            rt.SetAsFirstSibling();
        }
        if (end.childCount >= 1)
        {
            Transform rt = end.GetChild(end.childCount-1);
            rt.SetParent(current);
        }
    }

    private void Update()
    {
        if (isMoved) {
            isMoved = false;
            for (int i = 0; i < current.childCount; i++) {
                if (Vector2.Distance(current.GetChild(i).localPosition, Vector2.zero) > 0.5f) {
                    current.GetChild(0).localPosition = Vector2.Lerp(current.GetChild(0).localPosition , Vector2.zero , Time.deltaTime*5);
                    isMoved = true;
                }
            }
            for (int i = 0; i < start.childCount; i++)
            {
                if (Vector2.Distance(start.GetChild(i).localPosition, Vector2.zero) > 0.5f)
                {
                    start.GetChild(i).localPosition = Vector2.Lerp(start.GetChild(i).localPosition, Vector2.zero, Time.deltaTime * 5);
                    isMoved = true;
                }
            }
            for (int i = 0; i < end.childCount; i++)
            {
                if (Vector2.Distance(end.GetChild(i).localPosition, Vector2.zero) > 0.5f)
                {
                    end.GetChild(i).localPosition = Vector2.Lerp(end.GetChild(i).localPosition, Vector2.zero, Time.deltaTime * 5);
                    isMoved = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuScreen.activeInHierarchy)
            {
                MenuOption.SetActive(true);
            }
            else {
                LearnOption.SetActive(true);
            }
        }
    }

    public void Home() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

}
