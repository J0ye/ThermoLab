using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    public int targetSceneIndex = 0;

    public void OpenTargetScene()
    {
        if(targetSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
    }

    public void SetTarget(int target)
    {
        targetSceneIndex = target;
    }

    public void SetTarget(string target)
    {
        try
        {
            int numVal = Int32.Parse(target);
            targetSceneIndex = numVal;
        }
        catch(Exception e)
        {
            Debug.LogWarning("Input could not be parsed to int");
        }
    }
}
