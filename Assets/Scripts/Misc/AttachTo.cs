using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if(offset == Vector3.zero)
        {
            offset = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Deactivate();
        }

        transform.position = target.position + offset;
    }

    protected void Deactivate()
    {
        this.enabled = false;
    }

    protected void Activate()
    {
        this.enabled = true;
    }
}
