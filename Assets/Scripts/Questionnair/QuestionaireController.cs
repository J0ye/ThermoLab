using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionaireController : MonoBehaviour
{
    public List<QuestionController> questions = new List<QuestionController>();
    public bool closeOnStart = true;

    protected int activeQuestion = 0;
    protected bool state = false;
    // Start is called before the first frame update
    void Start()
    {
        Close();
        if (!closeOnStart) Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDisplayState()
    {
        if(state)
        {
            Close();
        } else
        {
            Open();
        }
    }

    public void Open()
    {
        questions[0].gameObject.SetActive(true);
        questions[0].Open();
        activeQuestion = 0;
        state = true;
    }

    public void Close()
    {
        foreach (QuestionController qc in questions)
        {
            qc.gameObject.SetActive(false);
        }
        state = false;
    }

    public void Next()
    {
        if (activeQuestion < questions.Count)
        {
            questions[activeQuestion].Close();
            activeQuestion++;
            activeQuestion = Mathf.Clamp(activeQuestion, 0, questions.Count - 1);
            questions[activeQuestion].Open();
        }
    }

    public void Previous()
    {
        if(activeQuestion > 0)
        {
            questions[activeQuestion].Close();
            activeQuestion--;
            activeQuestion = Mathf.Clamp(activeQuestion, 0, questions.Count-1);
            questions[activeQuestion].Open();
        }
    }
}

public class Questionaire
{
    public int id;
    public List<Question> questions;

    public Questionaire(int newID, List<QuestionController> controllers)
    {
        id = newID;

        questions = new List<Question>();
        foreach(QuestionController qc in controllers)
        {
            questions.Add(qc.GetQuestion());
        }
    }

    public void SaveToGlobal()
    {
        foreach(Questionaire q in global.Instance().questionaires)
        {
            if(q.id == id)
            {
                q.questions = questions;
                return;
            }
        }
        global.Instance().questionaires.Add(this);
    }
}
