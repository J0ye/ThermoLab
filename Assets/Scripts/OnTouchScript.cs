using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTouchScript : MonoBehaviour
{
    [Tooltip ("These functions will be called, when the TouchObject is pressed")]
    public UnityEvent onPress;
    [Tooltip("These functions will be called, when the touch on the object is released")]
    public UnityEvent onRelease;
    [Tooltip("These functions will be called, when the touch stays on this object")]
    public UnityEvent onStay;
    [Tooltip("These functions will be called, when the touch leaves the borders of the object while it is being pressed")]
    public UnityEvent onExit;

    // Start is called before the first frame update
    void Start()
    {
        StartEvent(onPress);
        StartEvent(onRelease);
        StartEvent(onStay);
        StartEvent(onExit);
    }

    private void OnTouchDown()
    {
        onPress.Invoke();
    }

    private void OnTouchUp()
    {
        onRelease.Invoke();
    }

    private void OnTouchStay()
    {
        onStay.Invoke(); 
    }

    private void OnTouchExit()
    {
        onExit.Invoke();
    }
    
    private void StartEvent(UnityEvent eve)
    {
        if (eve == null)
        {
            eve = new UnityEvent();
        }
    }
}
