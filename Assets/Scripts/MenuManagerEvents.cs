using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManagerEvents : MenuManager
{
    public UnityEvent OnStart = new UnityEvent();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        OnStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
