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

    protected List<User> userData;
    // Start is called before the first frame update
    void Start()
    {
        LoadUserData();
    }

    // Update is called once per frame
    void Update()
    {
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

        User temp = new User(nameInput.text, passwordInput.text);

        foreach(User u in userData)
        {
            if(u.Compare(temp))
            {
                OnLoginSuccsessfull.Invoke();
                return;
            }
        }

        Debug.Log("No login");
        OnLoginUnsuccsessfull.Invoke();
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

    private static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }
}

[Serializable]
public class User
{
    public Guid id;
    public string name;
    public string password;

    public User()
    {
        id = Guid.NewGuid();
        name = "empty";
        password = "empty";
    }

    public User(string newName, string newPassword)
    {
        id = Guid.NewGuid();
        name = newName;
        password = newPassword;
    }

    public bool Compare(User other)
    {
        if(name == other.name && password == other.password)
        {
            return true;
        }

        return false;
    }
}
