using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DragItem : MonoBehaviour
{
    [Range(1f, 10f)]
    public float length = 3f;
    public float staticDistance = 7f;
    public GameObject spawnParent;
    public UnityEvent OnStop = new UnityEvent();

    protected LineRenderer lr;
    protected GameObject displayObject;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        displayObject = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        SetLine();
        Vector3 in3D = TouchControllScript.instance.touchPointIn3D;
        //transform.position = new Vector3(in3D.x, in3D.y, staticDistance);
        transform.position = in3D;

        if (!TouchControllScript.instance.pressed)
        {
            StopDrag();
        }
    }

    public void StopDrag()
    {
        OnStop.Invoke();
        Destroy(gameObject);
        spawnParent.GetComponent<MeshRenderer>().enabled = true;
    }

    protected void SetLine()
    {
        Vector3 bot = transform.position;
        Vector3 top = new Vector3(transform.position.x, transform.position.y + length, transform.position.z);
        displayObject.transform.position = top;

        Vector3[] fillIn = { top, bot };

        lr.SetPositions(fillIn);
    }
}
