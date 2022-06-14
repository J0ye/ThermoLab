using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Session
{
    public User user;
    public Table[] tables;

    protected static Session instance;
    #region Static Methods
    public static Session Instance()
    {
        if(instance == null)
        {
            instance = new Session();
            instance.user = new User();
            instance.tables = new Table[0];
        }
        return instance;
    }

    public static void SetUser(User newUser)
    {
        if (newUser.Compare(Instance().user))
        {
            return;
        }
        Instance().user = newUser;
        Instance().user.id = newUser.id;
        LoadSessionData();
        Debug.Log("Succesfull login to user: " + newUser.name + " | " + newUser.id);
        Instance().OnLogin?.Invoke(Instance(), EventArgs.Empty);
    }

    /// <summary>
    /// Saves the data of the session into a file
    /// </summary>
    public static void SaveSessionData()
    {
        if(Instance().user == null)
        {
            Debug.Log("Did not save data. No user data");
            return;
        }
        string id = GetUserIDAsValidFileName();
        DirectoryInfo directory = Directory.CreateDirectory(Application.persistentDataPath + @"\Sessions");
        if (Instance().user != null)
        {
            string filename = directory.FullName + @"\" + id + ".json";
            Debug.Log("Saving session data to: " + filename);
            File.WriteAllText(filename, Instance().ToJSON());
        }
    }

    /// <summary>
    /// Will seatch the persitent data path for a file corresponding to the user ID and load session data as JSON.
    /// </summary>
    /// <returns>True if session data was succsefull extracted</returns>
    public static bool LoadSessionData()
    {
        if (Instance().user == null)
        {
            Debug.LogWarning("Tried to load session data without a user.");
            return false;
        }
        string id = GetUserIDAsValidFileName() + ".json";

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + @"\Sessions");
        if (directoryInfo.Exists)
        {
            foreach (var file in directoryInfo.GetFiles(id))
            {
                Guid temp = Instance().user.id; // The id gets overwritten by the json import 
                EventHandler temp2 = Instance().OnLogin;
                Debug.Log("File content");
                string txt = File.ReadAllText(file.FullName);
                Debug.Log(txt);
                FromJson(txt);
                Instance().user.id = temp; // Reset ID to temp from before json import
                Instance().OnLogin = temp2;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Creates a string of the users ID that can be used as a file name
    /// </summary>
    /// <returns>A sanatized string of the user ID</returns>
    public static string GetUserIDAsValidFileName()
    {
        return MakeValidFileName(Instance().user.id.ToString());
    }

    /// <summary>
    /// Creates a string that can be used as a file name
    /// </summary>
    /// <param name="name">The target string</param>
    /// <returns>A sanatized version of the input string</returns>
    public static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }
    #endregion

    public void Clear()
    {
        user = null;
        tables = null;
    }

    public event EventHandler OnLogin;

    #region JSON Methods
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
    #endregion

    public void AddTable(Table newTable)
    {
        int indexer;

        // CheckForTable returns the index of a fitting table in the tables array in the out parameter indexer.
        if (!CheckForTable(newTable, out indexer))
        {
            Debug.Log("Saving table " + newTable.name + " as new.");
            Table[] temp = new Table[tables.Length + 1];
            for (int i = 0; i < tables.Length; i++)
            {
                temp[i] = tables[i];
            }
            temp[tables.Length] = newTable;
            tables = temp;
        }
        else
        {
            Debug.Log("Overriding table " + newTable.name);
            tables[indexer] = newTable;
        }
    }

    protected bool CheckForTable(Table target, out int indexer)
    {
        if(tables == null)
        {
            tables = new Table[0];
            Debug.Log("Created new array for tables in session.");
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
