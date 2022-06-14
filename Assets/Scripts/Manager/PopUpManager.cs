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
        Scene popupScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); 
        SceneManager.UnloadSceneAsync(popupScene.name);
    }
}
