using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class QuestionController : MonoBehaviour
{
    public GameObject optionPrefab;
    public ScriptableQuestion content;
    [Range(0f, 100f)]
    public float margin = 25f;
    [Range(0f, 5f)]
    public float animationDuration = 0.5f;

    [HideInInspector]
    public List<SelectionOption> spawnedOptions = new List<SelectionOption>();
    protected RectTransform rt;
    protected Tween activeTween;
    protected Text titel;
    protected Text description;
    protected Image image;
    
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        titel = transform.GetChild(0).GetComponent<Text>();
        description = transform.GetChild(1).GetComponent<Text>();
        image = transform.GetChild(2).GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Close();
        }
    }

    void FixedUpdate()
    {
        if(content != null)
        {
            titel.text = content.titel;
            description.text = content.description;
            if (content.image != null && image.gameObject.activeSelf == true)
            {
                image.sprite = content.image;
            }else
            {
                image.gameObject.SetActive(false);
            }
            if(spawnedOptions.Count == 0) SpawnOptions();
        }
    }

    public void Open()
    {
        Vector3 startpos = rt.position;
        Vector3 offsetPos = new Vector3(Screen.width*2, startpos.y, startpos.z);
        rt.position = offsetPos;
        gameObject.SetActive(true);
        CancelActiveTween();
        activeTween = rt.DOMoveX(0 + Screen.width/2, animationDuration);
    }

    public void Close()
    {
        CancelActiveTween();
        StartCoroutine(AsynClose());
    }

    public void LoadInputFromSession(Question question)
    {
        Debug.Log("Loading input for: " + titel.text);
        for(int i = 0; i < question.selections.Length; i++)
        {
            if(i < spawnedOptions.Count)
            {
                if(question.selections[i])
                {
                    spawnedOptions[i].ExecuteOnSelection();
                    Debug.Log(titel.text + " gets a selected option");
                }
            }
        }
    }

    public void CheckSelection()
    {
        Question check = new Question(titel.text, spawnedOptions);
        Debug.Log("Json: " + check.ToJSON());
    }

    public Question GetQuestion()
    {
        return new Question(titel.text, spawnedOptions);
    }

    protected IEnumerator AsynClose()
    {
        Vector3 startpos = rt.position;
        Vector3 offsetPos = new Vector3(-Screen.width, startpos.y, startpos.z);
        activeTween = rt.DOMoveX(offsetPos.x, animationDuration);
        yield return activeTween.WaitForCompletion();
        gameObject.SetActive(false);
    }

    protected void CancelActiveTween()
    {
        if(activeTween != null)
        {
            if(activeTween.IsPlaying())
            {
                activeTween.Complete();
            }
            activeTween = null;
        }
    }

    protected void SpawnOptions()
    {
        int counter = 0;
        foreach(string option in content.options)
        {
            GameObject opt = Instantiate(optionPrefab, transform);
            Vector3 p = opt.GetComponent<RectTransform>().position;
            var padding = opt.GetComponent<RectTransform>().sizeDelta.y * margin;
            opt.GetComponent<RectTransform>().position = new Vector3(p.x, p.y - (padding * counter), p.z);
            SelectionOption detail = opt.GetComponent<SelectionOption>();
            spawnedOptions.Add(detail);
            detail.SetText(option);
            counter++;
            //margin * spawnedOptions.Count
        }
    }
}


[Serializable]
public class Question
{
    public string titel = "0";
    public bool[] selections;

    public Question(string newTitle, bool[] newSelections)
    {
        titel = newTitle;
        selections = newSelections;
    }

    public Question(string newTitle, List<SelectionOption> optionList)
    {
        titel = newTitle;

        bool[] temp = new bool[optionList.Count];

        int i = 0;
        foreach(SelectionOption so in optionList)
        {
            temp[i] = so.selected;
            i++;
        }
        selections = temp;
    }

    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }
}
