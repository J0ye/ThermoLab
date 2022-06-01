using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Table
{
    public string name;
    public string user;
    public Column[] fields;

    public Table(List<Row> rows, string ident, User us)
    {
        name = ident;
        user = us.name;
        fields = new Column[0];
        int rowIndex = 0;
        foreach (Row row in rows)
        {
            int lineIndex = 0;
            foreach (InputField put in row.lines)
            {
                Column[] temp = new Column[fields.Length + 1];
                for (int i = 0; i < fields.Length; i++)
                {
                    temp[i] = fields[i];
                }
                temp[fields.Length] = new Column(rowIndex, lineIndex, put.text);
                fields = temp;
                lineIndex++;
            }
            rowIndex++;
        }
    }

    public string ToJSON()
    {
        string msg = JsonUtility.ToJson(this);
        return msg;
    }
}

[Serializable]
public class Column
{
    public int row;
    public int line;
    public string content;

    public Column(int n_row, int n_line, string text)
    {
        row = n_row;
        line = n_line;
        content = text;
    }
}
