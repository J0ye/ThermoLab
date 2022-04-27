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
    protected void Start()
    {
        UpdateState();
        Debug.Log("A");
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
                SceneManager.UnloadSceneAsync(loadedScene);
                loadedScene = GetActiveSceneIndex();
            }
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

    protected int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    protected void UpdateState()
    {
        foreach(GameObject obj in itemList)
        {
            obj.SetActive(state);
        }
    }
}
