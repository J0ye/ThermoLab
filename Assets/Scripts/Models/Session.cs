using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Session
{
    public User user;
    public Table table;

    protected static Session instance;

    public static Session Instance()
    {
        if(instance == null)
        {
            instance = new Session();
        }
        return instance;
    }

    public string ToJSON()
    {
        string msg = JsonUtility.ToJson(this);
        return msg;
    }
}
