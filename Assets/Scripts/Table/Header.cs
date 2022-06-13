using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class Header : MonoBehaviour
{
    public List<Row> rows = new List<Row>();
    // Start is called before the first frame update
    void Start()
    {
        SetUpList();
        SceneManager.activeSceneChanged += ChangedActiveScene;
        LoadInputFromSession();
    }

    public InputField GetFieldAt(int row, int line)
    {
        if(row >= rows.Count)
        {
            Debug.LogError("Trying to get not existing row at " + row);
            return rows[0].lines[0];
        }
        else if(line > rows[row].lines.Count)
        {
            Debug.LogError("Trying to get not existing line at " + row + "|" + line);
            return rows[0].lines[0];
        }

        return rows[row].lines[line];
    }

    public int GetFieldCount()
    {
        return rows.Count * rows[0].lines.Count;
    }

    public void ChangedActiveScene(Scene current, Scene next)
    {
        //SaveInput();
    }
    
    public void LoadInputFromSession()
    {
        if(Session.Instance().user != null)
        {
            Table[] tables = Session.Instance().tables;
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].name == gameObject.name)
                {
                    for (int j = 0; j < tables[i].columns.Length; j++)
                    {
                        Column target = tables[i].columns[j];
                        rows[target.row].lines[target.line].text = target.content;
                    }
                }
            }
        }
    }

    public void SaveInput()
    {
        if(Session.Instance().user != null)
        {
            Session.Instance().AddTable(new Table(rows, gameObject.name, Session.Instance().user));
            Debug.Log(Session.Instance().ToJSON());
        }
    }

    protected void SetUpList()
    {
        rows = new List<Row>();
        if (transform.childCount > 1)
        {
            foreach (Transform child in transform.GetChild(1))
            {
                Row temp;
                if (child.TryGetComponent<Row>(out temp))
                {
                    rows.Add(temp);
                }
            }
        }
    }
}
