using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpManager : MenuManager
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).name);
    }
}
