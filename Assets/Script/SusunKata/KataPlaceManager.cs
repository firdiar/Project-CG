using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataSusunKata {
    [TextArea]
    public string value;
    [TextArea]
    public string answer;
}

public class KataPlaceManager : MonoBehaviour
{
    [SerializeField] List<RectTransform> lines;
    [SerializeField] GameObject kataPrefabs;
    [SerializeField] GameObject linePrefabs;
    [SerializeField] List<DataSusunKata> data;
    List<Transform> answer = new List<Transform>();
    int currentLines = 0;

    int currentIndex = 0;
    int currentSoal = 0;
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        string[] kata = data[0].value.Split('-');
        currentLines = 0;
        currentIndex = 0;
        StartCoroutine(PasangKata(kata));
    }

    bool nextSoal() {
        currentSoal++;

        if (currentSoal > data.Count - 1) {
            //Game Finish
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


        if (string.Join(" ", a).ToLower() == data[0].answer.ToLower()) {
            Debug.Log("Jawaban Benar");

            DeleteAll();
            if (nextSoal()) {
                StartGame();
            }
           

        }
    }


    IEnumerator PasangKata(string[] kata) {

        lines.Add( Instantiate(linePrefabs, this.transform).GetComponent<RectTransform>() );

        foreach (string s in kata) {
            if (lines[currentLines].sizeDelta.x + (s.Length * 20) > GetComponent<RectTransform>().sizeDelta.x)
            {
                currentLines++;
                lines.Add(Instantiate(linePrefabs, this.transform).GetComponent<RectTransform>());
            }
            GameObject c = Instantiate(kataPrefabs, this.transform.position , Quaternion.identity, lines[currentLines]);
            c.transform.GetChild(0).GetComponent<Text>().text = s;
            c.GetComponent<RectTransform>().sizeDelta = new Vector2((s.Length * 20), c.GetComponent<RectTransform>().sizeDelta.y);
            c.GetComponent<Button>().onClick.AddListener(()=> { Tambahkan(c); });
            yield return null;
        }
        yield return null;

    }
}
