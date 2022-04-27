using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    protected AsyncOperation unload;

    public void ExitApp()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
    }

    public void Unpause()
    {
        unload = SceneManager.UnloadSceneAsync("PauseMenu");
        StartCoroutine(ResumeApp());
    }

    protected IEnumerator ResumeApp()
    {
        yield return new WaitUntil(() => unload.isDone);

        Time.timeScale = 1.0f;
    }
}
