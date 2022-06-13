using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Events;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public InputField nameInput;
    public InputField passwordInput;

    [Header("Register")]
    public InputField newNameInput;
    public InputField newPasswordInput;
    public InputField repeatPassword;

    public UnityEvent OnLoginSuccsessfull = new UnityEvent();
    public UnityEvent OnLoginUnsuccsessfull = new UnityEvent();
    public UnityEvent OnRegister = new UnityEvent();
    public UnityEvent OnLogout = new UnityEvent();

    protected List<User> userData;
    // Start is called before the first frame update
    void Start()
    {
        LoadUserData();
    }

    public void CreateNewUser()
    {
        if(CheckInputField(newNameInput, newPasswordInput) ||
            newPasswordInput.text != repeatPassword.text)
        {
            Debug.Log("name: " + newNameInput.text + " , passwort: " + newPasswordInput.text + " | " + repeatPassword.text);
            Debug.Log("New user credentials are not correct.");
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        string filename = newNameInput.text + "LoginData";
        filename = MakeValidFileName(filename);
        filename = "/" + filename + ".dat";
        FileStream file = File.Create(Application.persistentDataPath + filename);
        User newUser = new User(newNameInput.text, newPasswordInput.text);
        bf.Serialize(file, newUser);
        file.Close();
        Debug.Log("File " + filename + "saved to " + Application.persistentDataPath);
        OnRegister.Invoke();
        LoadUserData();
    }

    public void LoadUserData()
    {
        userData = new List<User>();
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
        BinaryFormatter bf = new BinaryFormatter();
        foreach (var file in directoryInfo.GetFiles("*.dat"))
        {
            FileStream openFile = File.Open(file.FullName, FileMode.Open);
            User newUserData = (User)bf.Deserialize(openFile);
            openFile.Close();
            userData.Add(newUserData);
        }
    }

    public void Login()
    {
        if(CheckInputField(nameInput, passwordInput))
        {
            Debug.Log("login input has wrong characters");
            return;
        }

        // Temporary user build from input
        User temp = new User(nameInput.text, passwordInput.text);

        foreach(User u in userData)
        {
            if(u.Compare(temp))
            {
                // There exists a user for this name and password. U represents the user data coresponding to user input
                OnLoginSuccsessfull.Invoke();
                // Set the session data manualy to the logged in user, if no session data can be loaded
                Session.Instance().user = u;
                LoadSessionData();
                Session.Instance().user = u;
                Debug.Log("Login to user: " + Session.Instance().user.id);
                return;
            }
        }

        Debug.Log("No login");
        OnLoginUnsuccsessfull.Invoke();
    }

    public void Logout()
    {
        SaveSessionData();
        if(Session.Instance().user == null)
        {
            Debug.LogWarning("No user is logged in. Wont run logout.");
            return;
        }

        Session.Instance().Clear();
    }

    public void SaveSessionData()
    {
        string id = GetUserIDAsValidFileName();
        DirectoryInfo directory = Directory.CreateDirectory(Application.persistentDataPath + @"\Sessions");
        if (Session.Instance().user != null)
        {
            string filename = directory.FullName + @"\" + id + ".json";
            Debug.Log("Saving session data to: " + filename);
            File.WriteAllText(filename, Session.Instance().ToJSON());
        }
    }

    public bool LoadSessionData()
    {
        if (Session.Instance().user == null)
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
                Debug.Log("File content");
                string txt = File.ReadAllText(file.FullName);
                Debug.Log(txt);
                Session.FromJson(txt);
                return true;
            }
        }

        return false;
    }

    private bool CheckInputField(InputField target)
    {
        if(String.IsNullOrWhiteSpace(target.text))
        {
            return true;
        }

        return false;
    }

    private bool CheckInputField(InputField target1, InputField target2)
    {
        if (String.IsNullOrWhiteSpace(target1.text) ||
            String.IsNullOrWhiteSpace(target2.text))
        {
            return true;
        }

        return false;
    }

    private string GetIDAsValidFileName(string id)
    {
        return MakeValidFileName(id);
    }

    private string GetUserIDAsValidFileName()
    {
        return MakeValidFileName(Session.Instance().user.id.ToString());
    }

    private static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }
}
