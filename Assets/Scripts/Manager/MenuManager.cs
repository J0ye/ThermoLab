using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>();
    public bool state = false;

    protected int loadedScene;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        UpdateState();
        loadedScene = GetActiveSceneIndex();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void AddScene(int index)
    {
        if(loadedScene != index)
        {
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
            loadedScene = index;
        }else
        {
            if (loadedScene != GetActiveSceneIndex())
            {
                try
                {
                    SceneManager.UnloadSceneAsync(loadedScene);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error while unloading scene at index " + index);
                }
            }
            loadedScene = GetActiveSceneIndex();
        }
    }

    public void AddPopUp(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
        loadedScene = index;
    }

    public void SwitchState()
    {
        state = !state;
        UpdateState();
    }

    public void Close()
    {
        Scene popupScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.UnloadSceneAsync(popupScene.name);
    }

    protected int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    protected virtual void UpdateState()
    {
        foreach(GameObject obj in itemList)
        {
            obj.SetActive(state);
        }
    }
}
