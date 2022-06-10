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

    public void Clear()
    {
        user = null;
        tables = null;
    }

    public void AddTable(Table newTable)
    {
        int indexer;

        // CheckForTable returns the index of a fitting table in the tables array in the out parameter indexer.
        if(!CheckForTable(newTable, out indexer))
        {
            Table[] temp = new Table[tables.Length + 1];
            for(int i = 0; i < tables.Length; i++)
            {
                temp[i] = tables[i];
            }
            Debug.Log(temp.Length + " | " + tables.Length);
            temp[tables.Length] = newTable;
            tables = temp;
        }
        else
        {
            tables[indexer] = newTable;
        }
    }

    public string ToJSON()
    {
        string msg = JsonUtility.ToJson(this);
        return msg;
    }

    public static Session FromJson(string txt)
    {
        instance = JsonUtility.FromJson<Session>(txt);
        return instance;
    }

    protected bool CheckForTable(Table target, out int indexer)
    {
        if(tables == null)
        {
            tables = new Table[0];
        }
        for(int i = 0; i < tables.Length; i++)
        {
            if(tables[i].name == target.name 
                && tables[i].user == target.user)
            {
                indexer = i;
                return true;
            }
        }
        indexer = 0;
        return false;
    }
}
