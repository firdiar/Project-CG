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

        //SceneManager.LoadScene(sceneName);
        
        LoadingScreen.MAIN.ChangeScene(sceneName);
        //StartCoroutine(ChangeSceneAsync(sceneName));

    }

    public void Keluar()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }


    
}
