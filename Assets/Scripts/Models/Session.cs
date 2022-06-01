using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Session
{
    public User user;
    public Table[] tables;

    protected static Session instance;

    public static Session Instance()
    {
        if(instance == null)
        {
            instance = new Session();
        }
        return instance;
    }

    public void AddTable(Table newTable)
    {
        if(!CheckForTable(newTable))
        {
            Table[] temp = new Table[tables.Length + 1];
            temp = tables;
            temp[tables.Length + 1] = newTable;
            tables = temp;
        }
    }

    public string ToJSON()
    {
        string msg = JsonUtility.ToJson(this);
        return msg;
    }

    protected bool CheckForTable(Table target)
    {
        for(int i = 0; i < tables.Length; i++)
        {
            if(tables[i].name == target.name 
                && tables[i].user == target.user)
            {
                return true;
            }
        }
        return false;
    }
}
