using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen MAIN;
    [SerializeField] Image progress;
    [SerializeField] RawImage bg;

    private void Start()
    {
        MAIN = this;
        gameObject.SetActive(false);
    }


    public void Done() {

    }


    public void setAlpha(float i) {
        bg.CrossFadeAlpha(0, 0.1f, false);
    }

    public void setProgress(float prog) {
        progress.fillAmount = prog;
    }

}
