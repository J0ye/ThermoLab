using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpObject : PopUpManager
{
    public MeasurmentManager manager;
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public void Accept()
    {
    }

    public void Decline()
    {
    }

    protected bool CheckManager()
    {
        if(manager == null)
        {
            return false;
        }
        return true;
    }

}
