using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] GameObject option;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            option.SetActive(true);
        }
    }

    public void ChangeScene(string sceneName) {

        SceneManager.LoadScene(sceneName);
        //StartCoroutine(ChangeSceneAsync(sceneName));

    }

    public void Keluar()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }


    //IEnumerator ChangeSceneAsync(string sceneName) {
    //    Debug.Log("Masuk");
    //    LoadingScreen loadingBar = LoadingScreen.MAIN;
    //    loadingBar.gameObject.SetActive(true);
    //    DontDestroyOnLoad(loadingBar.gameObject);
    //    loadingBar.setAlpha(1);
    //    loadingBar.setProgress(0);

    //    AsyncOperation AO = SceneManager.LoadSceneAsync(sceneName);
    //    while (!AO.isDone)
    //    {
    //        Debug.Log(AO.progress);
    //        yield return null;
    //    }
    //}
}
