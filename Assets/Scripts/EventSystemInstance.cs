using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemInstance : MonoBehaviour
{
    public static EventSystemInstance instance;
    // Start is called before the first frame update
    void Start()
    {
        
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }
}
