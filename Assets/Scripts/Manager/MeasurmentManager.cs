using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeasurmentManager : MonoBehaviour
{
    public List<Header> heads = new List<Header>();
    public Text debugOutput;
    // Start is called before the first frame update
    void Start()
    {
        SetUpEditEvents();
        LoadInputFromSessionData();
        Session.Instance().ClearAllLoginEvents();
        Session.Instance().OnLogin += ReactToLogin;
    }

    public void SaveInput()
    {
        if (!Session.CheckUser())
        {
            return;
        }

        foreach (Header head in heads)
        {
            Session.Instance().AddTable(new Table(head.rows, head.name, Session.Instance().user));
        }
        Debug.Log("Saved session data");
        if (debugOutput != null) debugOutput.text = "Saving table in " + Application.persistentDataPath + Session.GetSessionFolderName() + "\n";
        Session.SaveSessionData();
    }

    public void LoadInputFromSessionData()
    {
        if (!Session.CheckUser())
        {
            if (debugOutput != null) debugOutput.text += "No user. Quiting load" + "\n";
            return;
        }

        if (debugOutput != null) debugOutput.text = "Data found writing into table" + "\n";
        foreach (Header head in heads)
        {
            head.LoadInputFromSession();
        }
    }

    public void ReactToLogin(object sender, EventArgs e)
    {
        debugOutput.text += "Reacting" + "\n";
        LoadInputFromSessionData();
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
                        SaveInput();
                    });
                    lineIndex++;
                }
                rowIndex++;
            }
        }
    }
}
