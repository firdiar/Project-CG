using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class DataSusunKata {
    [TextArea]
    public string value;
    [TextArea]
    public string answer;
}



public class KataPlaceManager : MonoBehaviour
{
    [SerializeField] GameObject panelLayer;
    [SerializeField] List<RectTransform> lines;
    [SerializeField] GameObject kataPrefabs;
    [SerializeField] GameObject linePrefabs;
    [SerializeField] List<string> data;
    
    [SerializeField] AudioSource AS;
    List<Transform> answer = new List<Transform>();
    int currentLines = 0;
    int currentIndex = 0;

    int currentSoal = 0;
    int currentIndexLevel = 0;
    //private void Start()
    //{
        
    //    StartGame();
    //}

    

    public void StartGame(string soalData , int idx) {
        string a = Resources.Load<TextAsset>("Data/SusunKata/"+soalData).text;

        data = JsonConvert.DeserializeObject<List<string>>(a);
        currentSoal = 0;
        currentIndexLevel = idx;
        Debug.Log("Current lv : "+(currentIndexLevel+1));
        StartGame();
    }

    void StartGame()
    {
        panelLayer.transform.SetAsFirstSibling();
        panelLayer.GetComponent<Image>().CrossFadeAlpha(0, 0f, true);
        string[] kata = data[currentSoal].Split(' ');
        currentLines = 0;
        currentIndex = 0;
        Shuffle<string>(kata);
        StartCoroutine(PasangKata(kata));
    }

    void Shuffle<T>(T[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            T tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

    bool nextSoal() {
        currentSoal++;

        if (currentSoal > data.Count - 1) {
            //Game Finish
            AS.clip = SoundBase.MAIN.GetSoundClip("GoodJob");
            AS.Play();
            Debug.Log("GameFinish");
            return false;
        }
        return true;
    }

    public void Refresh() {

        for (int i = 0; i < answer.Count; i++) {
            answer[i].GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }
    }

    

    public void DeleteAll() {
        foreach (RectTransform c in lines) {
            Destroy(c.gameObject);
        }
        lines.Clear();
        answer.Clear();
    }

    public void Tambahkan(GameObject kataObj) {

        //Debug.Log(kataObj.transform.GetChild(0).GetComponent<Text>().text);
        //Debug.Log(answer[currentIndex] == null);
        
        AS.clip = SoundBase.MAIN.GetSoundClip("Klik");
        AS.Play();

        if (answer.Contains(kataObj.transform)) {
            answer.Remove(kataObj.transform);
            kataObj.transform.GetChild(1).gameObject.SetActive(false);
            Refresh();
            return;
        }

        answer.Add(kataObj.transform);

        kataObj.transform.GetChild(1).gameObject.SetActive(true);
        kataObj.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = (answer.Count).ToString();

        currentIndex++;


    }

    public void CekJawaban() {
        string[] a = new string[answer.Count];
        for (int i = 0; i < answer.Count; i++)
        {
            a[i] = answer[i].GetChild(0).GetComponent<Text>().text;
        }

        //panelLayer.transform.SetAsLastSibling();
        //panelLayer.GetComponent<Image>().CrossFadeAlpha(0.5f, 0.5f, true);

        if (string.Join(" ", a).ToLower() == data[currentSoal].ToLower())
        {
            Debug.Log("Jawaban Benar");
            AS.clip = SoundBase.MAIN.GetSoundClip("JawabanBenar");
            AS.Play();

            DeleteAll();
            if (nextSoal())
            {

                StartGame();
            }
            else {
                panelLayer.transform.SetAsLastSibling();
                panelLayer.GetComponent<Image>().CrossFadeAlpha(1, 0.5f, true);
                Save();
            }


        }
        else {
            AS.clip = SoundBase.MAIN.GetSoundClip("JawabanSalah");
            AS.Play();
        }
    }

    void Save() {
        if ("" == PlayerPrefs.GetString("DataLevelSusunKata"))
        {
            PlayerPrefs.SetString("DataLevelSusunKata", "1-0-0");
        }
        string[] lv = PlayerPrefs.GetString("DataLevelSusunKata").Split('-');
        
        if (currentIndexLevel + 1 < lv.Length)
        {
            lv[currentIndexLevel + 1] = "1";
            for (int i = 0; i < lv.Length; i++) {
                Debug.Log("is Locked lv "+ (i+1)+" : "+lv[i]);
            }
            Debug.Log("saved");
        }
        
        PlayerPrefs.SetString("DataLevelSusunKata", string.Join("-",lv));
    }


    IEnumerator PasangKata(string[] kata) {

        lines.Add( Instantiate(linePrefabs, this.transform).GetComponent<RectTransform>() );

        foreach (string s in kata) {
            if (lines[currentLines].sizeDelta.x + (s.Length * 20) > GetComponent<RectTransform>().sizeDelta.x)
            {
                currentLines++;
                lines.Add(Instantiate(linePrefabs, this.transform).GetComponent<RectTransform>());
                
            }
           
            GameObject c = Instantiate(kataPrefabs, this.transform.position, Quaternion.identity, lines[currentLines]);
            c.transform.GetChild(0).GetComponent<Text>().text = s;
            c.GetComponent<RectTransform>().sizeDelta = new Vector2((s.Length * 20), c.GetComponent<RectTransform>().sizeDelta.y);
            c.GetComponent<Button>().onClick.AddListener(() => { Tambahkan(c); });
            
            yield return null;
        }

        GameObject temp = Instantiate(kataPrefabs, this.transform.position, Quaternion.identity, lines[currentLines]);
        Destroy(temp);

        yield return null;

    }
}
