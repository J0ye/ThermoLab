using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A table is an abstract and serilzable form of the headers, that build tables used to store measurments.
/// Every table has an array of column and each entry represents one input field of the measurment table.
/// </summary>
[Serializable]
public class Table
{
    public string name;
    public string user;
    public Column[] columns;
    public Table(List<Row> rows, string ident, User us)
    {
        name = ident;
        user = us.name;
        columns = new Column[0];
        int rowIndex = 0;
        foreach (Row row in rows)
        {
            int lineIndex = 0;
            foreach (InputField put in row.lines)
            {
                Column[] temp = new Column[columns.Length + 1];
                for (int i = 0; i < columns.Length; i++)
                {
                    temp[i] = columns[i];
                }
                temp[columns.Length] = new Column(rowIndex, lineIndex, put.text);
                columns = temp;
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
