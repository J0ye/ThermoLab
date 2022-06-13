using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SaveState { Waiting, Declined, Accepted}

public class MeasurmentManager : MonoBehaviour
{
    public List<Header> heads = new List<Header>();
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        SetUpEditEvents();
    }

    public void ChangedActiveScene(Scene current, Scene next)
    {
        //SaveInput();
    }

    public void SaveInput()
    {
        if (Session.Instance().user != null)
        {
            foreach (Header head in heads)
            {
                Session.Instance().AddTable(new Table(head.rows, head.name, Session.Instance().user));
            }
            Debug.Log(Session.Instance().ToJSON());
        }
    }

    public bool EditTableEntryInSession(string nameOfHeader, Column newValue)
    {
        Debug.Log("Editing: " + newValue.row + "|" + newValue.line);
        for(int i = 0; i < Session.Instance().tables.Length; i++)
        {
            if(Session.Instance().tables[i].name == nameOfHeader)
            {
                Session.Instance().tables[i].EditColumn(newValue);
                return true;
            }
        }
        return false;
    }

    protected void SetUpEditEvents()
    {
        foreach(Header head in heads)
        {
            int rowIndex = 0;
            foreach (Row row in head.rows)
            {
                int lineIndex = 0;
                foreach (InputField field in row.lines)
                {
                    Column newValues = new Column(rowIndex, lineIndex, field.text);
                    field.onEndEdit.AddListener(delegate
                    {
                        bool v = EditTableEntryInSession(head.name, newValues);
                    });
                    lineIndex++;
                }
                rowIndex++;
            }
        }
    }
}
