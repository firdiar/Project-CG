using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen MAIN;
    [SerializeField] Image progress;
    [SerializeField] RawImage bg;
    [SerializeField] Image circle;
    [SerializeField] Text textLoad;
    bool inProgress = false;
    

    private void Start()
    {
        if (MAIN != null) {
            Destroy(gameObject);
            return;
        }

        MAIN = this;
        //StartCoroutine(FadeTo(0, 0, bg));
        setAlpha(0,0, progress);
        setAlpha(0, 0, bg);
        setAlpha(0, 0, textLoad);
        setAlpha(0, 0, circle);

        DontDestroyOnLoad(this.gameObject);
        inProgress = false;
        //gameObject.SetActive(false);
        
    }

    public void ChangeScene(string sceneName) {
        inProgress = true;
        StartCoroutine(ChangeSceneAsync(sceneName));

    }
    IEnumerator ChangeSceneAsync(string sceneName)
    {
        Debug.Log("Load "+sceneName);


        //this.gameObject.SetActive(true);
        //setAlpha( , progress);
        setAlpha(1, 0, progress);
        setAlpha(1, 0, bg);
        setAlpha(1, 0f, textLoad);
        setAlpha(1, 0f, circle);
        //StartCoroutine(FadeTo(1, 0.5f, bg));
        setProgress(0);

        AsyncOperation AO = SceneManager.LoadSceneAsync(sceneName);
        while (!AO.isDone)
        {
            //Debug.Log(AO.progress);
            setProgress(AO.progress);
            yield return null;
        }
        //StartCoroutine(FadeTo(0, 1, bg));
        setAlpha(0, 0.8f, progress);
        setAlpha(0, 0.8f, bg);
        setAlpha(0, 0.8f, textLoad);
        setAlpha(0, 0.8f, circle);
        setProgress(1);
        inProgress = false;
    }

    //IEnumerator FadeTo(float aValue, float aTime , RawImage obj)
    //{
    //    if (isFading) {
    //        Debug.Log("fading");
    //        while (isFading)
    //        {
    //            yield return new WaitForSeconds(0.02f);
    //        }
    //    }
        

    //    isFading = true;
    //    float alpha = bg.color.a;
    //    float t = 0.0f;
    //    while ( t < 1.0f && isFading )
    //    {
    //        t = Mathf.Clamp(t +  (0.02f/ (aTime<0.02f?0.02f:aTime)) , 0 , 1);
    //        Color newColor = new Color(obj.color.r, obj.color.g , obj.color.b , Mathf.Lerp(alpha, aValue, t));
    //        obj.color = newColor;

    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    isFading = false;
    //}


    public void setAlpha(float i , float time , Image img) {
        img.CrossFadeAlpha(i, time, false);
    }
    public void setAlpha(float i, float time, RawImage img)
    {
        img.CrossFadeAlpha(i, time, false);
    }
    public void setAlpha(float i, float time, Text img)
    {
        img.CrossFadeAlpha(i, time, false);
    }


    public void setProgress(float prog) {
        progress.fillAmount = prog;
    }

}
