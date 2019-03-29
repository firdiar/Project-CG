using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class MateriManager : MonoBehaviour
{
    public static MateriManager MAIN;
    
    [Header("MenuScreen")]
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject menuOption;
    [SerializeField] GameObject layer;
    [SerializeField] Transform menuLain;


    [Header("LearnScreen")]
    [SerializeField] GameObject LearnScreen;
    [SerializeField] GameObject CardMateriPrefabs;
    [SerializeField] GameObject CardSoalPrefabs;
    [SerializeField] GameObject CardSelesaiPrefabs;
    [SerializeField] RectTransform start;
    [SerializeField] RectTransform current;
    [SerializeField] RectTransform end;
    [SerializeField] GameObject LearnOption;
    Transform cardOne = null;
    Transform cardTwo = null;
    //int index;

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

    public void ShowMenuLain() {
        layer.SetActive(true);
        StartCoroutine(MoveMenuLain(new Vector3(0 , menuLain.localPosition.y , menuLain.localPosition.z), menuLain));
    }
    public void HideMenuLain()
    {
        layer.SetActive(false);
        StartCoroutine(MoveMenuLain(new Vector3(Screen.width , menuLain.localPosition.y, menuLain.localPosition.z), menuLain));
    }

    IEnumerator MoveMenuLain(Vector3 target, Transform obj) {
        Debug.Log("Moving");
        while (Vector3.Distance(obj.localPosition, target) > 1f && ( target.x==0 ? layer.activeInHierarchy:!layer.activeInHierarchy)) {
            Debug.Log("Moving");
            obj.localPosition = Vector3.Lerp(obj.localPosition, target, 0.2f);
            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }


    public void StartGame(string sceneName) {
        LoadingScreen.MAIN.ChangeScene(sceneName);
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
                //index = 0;
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
            cardOne = current.GetChild(0);
            cardOne.SetParent(end);
        }
        if (start.childCount >= 1) {
            cardTwo = start.GetChild(0);
            cardTwo.SetParent(current);
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
            cardOne = current.GetChild(0);
            cardOne.SetParent(start);
            cardOne.SetAsFirstSibling();
        }
        if (end.childCount >= 1)
        {
            cardTwo = end.GetChild(end.childCount-1);
            cardTwo.SetParent(current);
        }
    }

    private void Update()
    {
        if (isMoved) {
  
            if (cardOne != null && Vector2.Distance(cardOne.localPosition, Vector2.zero) > 0.5f)
            {
                cardOne.localPosition = Vector2.Lerp(cardOne.localPosition, Vector2.zero, Time.deltaTime * 5);

            }
            if (cardTwo != null && Vector2.Distance(cardTwo.localPosition, Vector2.zero) > 0.5f)
            {
                cardTwo.localPosition = Vector2.Lerp(cardTwo.localPosition, Vector2.zero, Time.deltaTime * 5);
            }
            isMoved = !((cardOne == null || Vector2.Distance(cardOne.localPosition, Vector2.zero) < 0.5f) && (cardTwo == null || Vector2.Distance(cardTwo.localPosition, Vector2.zero) < 0.5f));
            //Debug.Log("is Move : "+isMoved);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuScreen.activeInHierarchy)
            {
                menuOption.SetActive(true);
            }
            else {
                LearnOption.SetActive(true);
            }
        }
    }

    public void Home() {
        Debug.Log("Game Quit");
        Application.Quit();
    }

}
