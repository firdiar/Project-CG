using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class Soal
{
    public int id;
    public Vector2 StartPos;
    public string pertanyaan;
    public string jawaban;
    public Arah arah ;
}

[System.Serializable]
public enum Arah {
    Horizontal,
    Vertical
}




public class BoardGeneratorTTS : MonoBehaviour
{
    [Header("MenuScreen")]
    [SerializeField] GameObject MenuScreen;
    [SerializeField] List<ObjectLesson> data;
    [SerializeField] Transform contentViewer;
    [SerializeField] GameObject objContent;
    [SerializeField] GameObject optionMenu;
    GameObject currentObjSelected ;

    [Header("GameScreen")]
    [SerializeField] GameObject GameScreen;
    [SerializeField] Transform boxSelectedParent;
    [SerializeField] Transform boxNotSelectedParent;
    [SerializeField] GameObject boxPrefabs;

    [SerializeField] Transform contentListSoal;
    [SerializeField] GameObject listSoalPrefabs;

    [SerializeField] RectTransform LeftSoal;
    [SerializeField] RectTransform RightSoal;

    [SerializeField] List<Soal> soals;
    [SerializeField] GameObject optionGame;

    public Vector2 width;//x for min and y for max
    public Vector2 height;//x for min and y for max

    Vector3 cameraTarget;
    bool movingCamera = false;

    bool showing = false;
    bool showInProgress = false;

    Arah currentArah;
    BoxTTS currentBtts;

    private void Start()
    {
        changeScreen(1);
        string a = Resources.Load<TextAsset>("Data/TTS/Data").text;

        data = JsonConvert.DeserializeObject<List<ObjectLesson>>(a);

        foreach (ObjectLesson ol in data) {
            GameObject obj = Instantiate(objContent, contentViewer);
            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> {
                if (currentObjSelected != null) {
                    currentObjSelected.transform.GetChild(0).gameObject.SetActive(false);
                }
                currentObjSelected = obj;
                currentObjSelected.transform.GetChild(0).gameObject.SetActive(true);
            });
            obj.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = ol.lessonName;
            obj.GetComponent<ObjMenuListView>().valueData = ol.lessonData;
        }


    }

  

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameScreen.activeInHierarchy)
            {
                optionGame.SetActive(true);
            }
            else {
                optionMenu.SetActive(true);
            }
        }
        if (movingCamera) {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTarget, 4*Time.deltaTime);
            if (Vector3.Distance(Camera.main.transform.position, cameraTarget) < 1) {
                movingCamera = false;
            }
        }
    }

    public void Main() {
        if (currentObjSelected == null)
            return;

        Generate(currentObjSelected.GetComponent<ObjMenuListView>().valueData);
        changeScreen(0);
    }

    public void changeScreen(int index) {
        if (index == 0)
        {
            MenuScreen.SetActive(false);
            GameScreen.SetActive(true);
        }
        else {
            MenuScreen.SetActive(true);
            GameScreen.SetActive(false);
        }
    }



    public void Generate(string data)
    {
        movingCamera = false;
        string a = Resources.Load<TextAsset>("Data/TTS/"+data).text;
        Debug.Log(a);
        soals = JsonConvert.DeserializeObject<List<Soal>>(a);
        currentBtts = null;
        foreach (Soal s in soals)
        {
            CreateTTS(s);
        }


        Transform box = boxNotSelectedParent.GetChild(Random.RandomRange(0, boxNotSelectedParent.childCount));

        
    }

    public void clearGameScreen() {
        int count = boxNotSelectedParent.childCount;
        for (int i = 0; i < count; i++) {
            Destroy(boxNotSelectedParent.GetChild(0).gameObject);
        }
        count = boxSelectedParent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(boxSelectedParent.GetChild(0).gameObject);
        }

        Vector2 target = LeftSoal.localPosition;
        target.x = -Screen.width;
        LeftSoal.localPosition = target;

        target = RightSoal.localPosition;
        target.x = Screen.width;
        RightSoal.localPosition = target;

        Camera.main.transform.position = new Vector3(0, 0, -10);

    }

    public void CekJawaban() {
        if (currentBtts == null) {
            return;
        }

        bool result = currentBtts.CheckAnswerAllBox(currentArah);
        Debug.Log(" The Answer : " + result);
        if (result) {
            RightSoal.GetChild(1).gameObject.SetActive(true);
        }

    }

  
    public void ShowLeft() {
        Vector2 target = LeftSoal.localPosition;
        if (showInProgress || (showing && Mathf.Abs(target.x) > 1))
        {
            Debug.Log("Tidak Dapat Show");
            return;
        }

        
        if (Mathf.Abs(target.x) > 1) { 

            target.x = 0;
            showing = true;
        }
        else {
            showing = false;
            target.x = -Screen.width;
        }

        Debug.Log(target);
        StartCoroutine(Show(LeftSoal, target));
        
    }
    public void ShowRight() {
        Vector2 target = RightSoal.localPosition;
        if (showInProgress || (showing && Mathf.Abs(target.x) > 1) || currentBtts == null)
        {
            Debug.Log("Tidak Dapat Show");
            return;
        }

        if (currentArah == Arah.Vertical)
        {
            RightSoal.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = currentBtts.soalVertical.pertanyaan;
            RightSoal.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = currentBtts.soalVertical.jawaban;
            if (currentBtts.wasAnswer)
            {
               
                RightSoal.GetChild(1).gameObject.SetActive(true);
            }
            else {
                RightSoal.GetChild(1).gameObject.SetActive(false);
            }
        }
        else {
            RightSoal.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = currentBtts.soalHorizontal.pertanyaan;
            RightSoal.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = currentBtts.soalHorizontal.jawaban;
            if (currentBtts.wasAnswer)
            {
                
                RightSoal.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RightSoal.GetChild(1).gameObject.SetActive(false);
            }
        }

        

        
        if (Mathf.Abs(target.x) > 1)
        {
            target.x = 0;
            showing = true;
        }
        else
        {
            showing = false;
            target.x = Screen.width;
        }

        Debug.Log(target);

        StartCoroutine(Show(RightSoal, target));
    }

    IEnumerator Show(RectTransform obj, Vector3 target) {
        showInProgress = true;
        while (Vector3.Distance( obj.localPosition, target) > 0.2f) {
            obj.localPosition = Vector3.Lerp(obj.localPosition, target, 12*0.02f);
            yield return new WaitForSeconds(0.02f);
        }
        showInProgress = false;
        Debug.Log("Showing Finish");
        yield return null;
    }

    public void SetObjSelected(BoxTTS box)
    {

        if ((box.nextHorizontal != null || box.prevHorizontal != null) && (box.nextVertical != null || box.prevVertical != null)) {
            Debug.Log("Persimpangan");
            SetObjSelected(box, Arah.Horizontal);
            return;
        }

        int count = boxSelectedParent.childCount;
        for (int i = 0; i < count; i++)
        {
            boxSelectedParent.GetChild(0).GetComponent<BoxTTS>().setToSelected();
            boxSelectedParent.GetChild(0).GetChild(1).gameObject.SetActive(false);
            boxSelectedParent.GetChild(0).SetParent(boxNotSelectedParent);

        }

        List<GameObject> allObj = new List<GameObject>();

        box.activateSelected(ref allObj, ref currentArah, ref currentBtts);


        if (currentBtts == null)
        {
            Debug.Log("BTTS NULL");
            return;
        }
       

        foreach (GameObject obj in allObj)
        {
            obj.transform.SetParent(boxSelectedParent);
        }
        currentBtts.setToFocus();

        cameraTarget = currentBtts.transform.position;
        cameraTarget.z = -10;
        cameraTarget.y -= 3;
        movingCamera = true;

    }


    public void SetObjSelected(BoxTTS box , Arah arah)
    {

        int count = boxSelectedParent.childCount;
        for (int i = 0; i < count; i++)
        {
            boxSelectedParent.GetChild(0).GetComponent<BoxTTS>().setToSelected();
            boxSelectedParent.GetChild(0).GetChild(1).gameObject.SetActive(false);
            boxSelectedParent.GetChild(0).SetParent(boxNotSelectedParent);

        }

        List<GameObject> allObj = new List<GameObject>();

        box.activateSelected(ref allObj, ref currentArah, ref currentBtts , arah);


        if (currentBtts == null)
        {
            Debug.Log("BTTS NULL");
            return;
        }
        

        foreach (GameObject obj in allObj)
        {
            obj.transform.SetParent(boxSelectedParent);
        }
        currentBtts.setToFocus();

        cameraTarget = currentBtts.transform.position;
        cameraTarget.z = -10;
        cameraTarget.y -= 3;
        movingCamera = true;

    }

    

    public void InputText(UnityEngine.UI.Text text) {
        if (currentBtts == null) {
            return;
        }

        if (text.text == "CLEAR")
        {
            currentBtts = currentBtts.ClearAll(currentArah);
            currentBtts.setToFocus();
        }
        else if (text.text == "DEL")
        {
            if (!currentBtts.wasAnswer)
            {
                currentBtts.currentAnswer = "";
            }
            currentBtts = currentBtts.getPrevBox(currentArah);
            currentBtts.setToFocus();
        }
        else
        {
           
            if (!currentBtts.wasAnswer)
            {
                currentBtts.currentAnswer = text.text;
            }
            currentBtts = currentBtts.getNextBox(currentArah);
            currentBtts.setToFocus();
        }

    }


    void CreateTTS(Soal soal) 
    {
        BoxTTS temp = null;
        if (soal.arah == Arah.Horizontal) {
            if (soal.StartPos.x < width.x) {
                width.x = soal.StartPos.x;
            }
            if ((soal.StartPos.x + (soal.jawaban.Length - 1) > width.y)){
                width.y = (soal.StartPos.x + (soal.jawaban.Length - 1));
            }
        }else if (soal.arah == Arah.Vertical)
        {
            if (soal.StartPos.y > width.y)
            {
                height.y = soal.StartPos.y;
            }
            if ((soal.StartPos.y - (soal.jawaban.Length - 1) < height.x)){
                height.x = (soal.StartPos.y - (soal.jawaban.Length - 1));
            }
        }



        for (int i = 0; i < soal.jawaban.Length; i++)
        {

            Vector2 pos = new Vector2(soal.StartPos.x + (soal.arah == Arah.Horizontal ? i : 0), soal.StartPos.y - (soal.arah == Arah.Vertical ? i : 0));
            RaycastHit2D ray = Physics2D.CircleCast(pos, 0.1f,Vector2.zero , 0 );

            BoxTTS btts = null;
             
            if (ray.transform != null)
            {
                Debug.DrawRay(ray.transform.position, (soal.arah == Arah.Horizontal ? Vector2.right : Vector2.down) * 0.1f, Color.red, 10);
                Debug.Log(ray.transform.localPosition);
                btts = ray.transform.GetComponent<BoxTTS>();
                if (btts.boxAnswer != soal.jawaban[i].ToString()) {
                    Debug.LogError("Error : " + soal.id + " answer: (" + soal.jawaban + ") at Value " + i + " must be " + soal.jawaban[i].ToString());
                }
                if (i == 0)
                {
                    btts.transform.GetChild(2).GetComponent<TextMesh>().text = soal.id.ToString();
                    GameObject listSoal = Instantiate(listSoalPrefabs, contentListSoal);
                    listSoal.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = soal.id + ". " + (soal.arah == Arah.Horizontal ? "MENDATAR\n\n" : "MENURUN\n\n") + soal.pertanyaan;
                    listSoal.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> SetObjSelected(btts , soal.arah));
                }
            }
            else {
                GameObject box = Instantiate(boxPrefabs, pos, Quaternion.identity, boxNotSelectedParent);
                if (i == 0)
                {
                    box.transform.GetChild(2).GetComponent<TextMesh>().text = soal.id.ToString();
                    GameObject listSoal = Instantiate(listSoalPrefabs, contentListSoal);
                    listSoal.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = soal.id + ". " + (soal.arah == Arah.Horizontal ? "MENDATAR\n\n" : "MENURUN\n\n") + soal.pertanyaan;
                    listSoal.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SetObjSelected(btts, soal.arah));
                }
                btts = box.GetComponent<BoxTTS>();
            }

            btts.boxAnswer = soal.jawaban[i].ToString();
            //btts.soal = soal;
           
            if (soal.arah == Arah.Vertical)
            {
                //Debug.Log("Done");
                btts.soalVertical = soal;
                btts.prevVertical = temp;
                if(temp != null)
                    temp.nextVertical = btts;
            }
            else {
                btts.soalHorizontal = soal;
                btts.prevHorizontal = temp;
                if (temp != null)
                    temp.nextHorizontal = btts;
            }
            temp = btts;

        }

    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}