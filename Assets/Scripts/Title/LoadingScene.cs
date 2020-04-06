using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    private AsyncOperation async;
    public Canvas LoadingUi;
    public Slider Slider;

    public void LoadRatchAndTimerScene()
    {
        LoadingUi.enabled = true;
        StartCoroutine(LoadScene("RatchAndTimerScene"));
    }
    public void LoadVibeManScene()
    {
        LoadingUi.enabled = true;
        StartCoroutine(LoadScene("VibeManScene"));
    }

    public void LoadTitleScene()
    {
        LoadingUi.enabled = true;
        StartCoroutine(LoadScene("TitleScene"));
    }


    IEnumerator LoadScene(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            Slider.value = async.progress;
            yield return null;
        }
    }

}