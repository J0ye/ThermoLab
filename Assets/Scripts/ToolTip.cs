using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ToolTip : MonoBehaviour
{
    public Transform anker;

    protected LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        if(anker == null)
        {
            if(transform.childCount > 0)
            {
                anker = transform.GetChild(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetLine();
    }

    protected void SetLine()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        if(anker == null)
        {
            lr.positionCount = 1;
        }
        else
        {
            lr.SetPosition(1, anker.position);
        }
    }
}
