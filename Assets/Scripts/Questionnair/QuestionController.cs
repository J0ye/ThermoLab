using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestionController : MonoBehaviour
{
    public GameObject optionPrefab;
    public ScriptableQuestion content;
    [Range(0f, 100f)]
    public float margin = 25f;
    [Range(0f, 5f)]
    public float animationDuration = 0.5f;

    protected List<SelectionOption> spawnedOptions = new List<SelectionOption>();
    protected RectTransform rt;
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
        rt.DOMoveX(0 + Screen.width/2, animationDuration);
    }

    public void Close()
    {
        StartCoroutine(AsynClose());
    }

    public void CheckSelection()
    {
        Question check = new Question(titel.text, spawnedOptions);
        Debug.Log("Json: " + check.JSON());
    }

    public Question GetQuestion()
    {
        return new Question(titel.text, spawnedOptions);
    }

    protected IEnumerator AsynClose()
    {
        Vector3 startpos = rt.position;
        Vector3 offsetPos = new Vector3(-Screen.width, startpos.y, startpos.z);
        Tween temp = rt.DOMoveX(offsetPos.x, animationDuration);
        yield return temp.WaitForCompletion();
        //gameObject.SetActive(false);
    }

    protected void SpawnOptions()
    {
        int counter = 0;
        foreach(string option in content.options)
        {
            GameObject opt = Instantiate(optionPrefab, transform);
            Vector3 p = opt.GetComponent<RectTransform>().position;
            opt.GetComponent<RectTransform>().position = new Vector3(p.x, p.y - (margin * spawnedOptions.Count), p.z);
            SelectionOption detail = opt.GetComponent<SelectionOption>();
            spawnedOptions.Add(detail);
            detail.SetText(option);
            counter++;
        }
    }
}

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

    public string JSON()
    {
        return JsonUtility.ToJson(this);
    }
}
