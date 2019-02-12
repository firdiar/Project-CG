using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class GameSusunKataManager : MonoBehaviour
{
    [SerializeField] AudioSource AS;

    [Header("MenuScreen")]
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject btnPrefabs;
    [SerializeField] Transform contentParent;
    [SerializeField] List<ObjectLesson> level;
    [SerializeField] GameObject menuOption;
    //GameObject currentObjSelected;
    int currentObjIndex = -1;
    


    [Header("GameScreen")]
    [SerializeField] GameObject gameScreen;
    [SerializeField] KataPlaceManager gameManager;
    [SerializeField] GameObject gameOption;

    public int currentLevel;
    

    public void changeScreen(int idx) {
        if (idx == 0)
        {
            menuScreen.SetActive(true);
            gameScreen.SetActive(false);
        }
        else {

            menuScreen.SetActive(false);
            gameScreen.SetActive(true);
        }

    }

    public void PlaySound(string name) {
        Debug.Log("playing " + name);
        AS.clip = SoundBase.MAIN.GetSoundClip(name);
        AS.Play();
    }

    private void Start()
    {
        changeScreen(0);
        currentLevel = 0;
        if ("" == PlayerPrefs.GetString("DataLevelSusunKata")){
            PlayerPrefs.SetString("DataLevelSusunKata", "1-0-0");
        }

        string a = Resources.Load<TextAsset>("Data/SusunKata/Data").text;

        level = JsonConvert.DeserializeObject<List<ObjectLesson>>(a);

        showLevel();
        refreshMenuLevel();
    }

    void showLevel() {

        int i = 0;
        foreach (ObjectLesson ol in level) {
            int temp = i;
            GameObject obj = Instantiate(btnPrefabs, contentParent);
            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                if (currentObjIndex != -1)
                {
                    contentParent.GetChild(currentObjIndex).transform.GetChild(0).gameObject.SetActive(false);
                }
                //currentObjSelected = obj;
                PlaySound("Klik");
                currentObjIndex = temp;
                contentParent.GetChild(currentObjIndex).GetChild(0).gameObject.SetActive(true);
                
            });
            
            obj.GetComponent<ObjMenuListView>().valueData = ol.lessonData;
            obj.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = ol.lessonName;
            i++;
        }

        

    }

    public void refreshMenuLevel() {
        string[] dataLevel = PlayerPrefs.GetString("DataLevelSusunKata").Split('-');

        for (int i = 0; i < contentParent.childCount; i++) {
            contentParent.GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = (dataLevel[i] == "1");
        }

    }

    public void Main() {
        if (currentObjIndex == -1|| contentParent.GetChild(currentObjIndex).GetComponent<ObjMenuListView>().valueData == "") {
            return;
        }
        changeScreen(1);
        gameManager.StartGame(contentParent.GetChild(currentObjIndex).GetComponent<ObjMenuListView>().valueData, currentObjIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuScreen.activeInHierarchy)
            {
                menuOption.SetActive(true);
            }
            else {
                gameOption.SetActive(true);
            }
        }
    }

    public void Home() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
