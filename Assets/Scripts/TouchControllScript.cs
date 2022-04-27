using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllScript : MonoBehaviour {

    public static TouchControllScript instance;

    public LayerMask touchInputMask;
    public float range = 3f;
    [HideInInspector]
    public Vector3 touchPointIn3D = Vector3.zero;
    public bool pressed = false;

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit rayHit;

    public void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update () {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            pressed = true;
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();
            
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            if (Physics.Raycast(ray, out rayHit, touchInputMask))
            {
                GameObject recipient = rayHit.transform.gameObject;
                Debug.Log("Debug Hit: " + rayHit.transform.gameObject.name);
                touchList.Add(recipient);

                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                SetTouchPointIn3D(rayHit);
            }
            else
            {
                SetTouchPointIn3D(ray);
            }

            foreach (GameObject obj in touchesOld)
            {
                if (!touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else
        {
            Debug.Log("reset");
            pressed = false;
        }
#endif

        if (Input.touchCount > 0)
        {
            pressed = true;
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();


            foreach (Touch touch in Input.touches)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out rayHit, touchInputMask))
                {
                    GameObject recipient = rayHit.transform.gameObject;

                    touchList.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    SetTouchPointIn3D(rayHit);
                }
                else
                {
                    SetTouchPointIn3D(ray);
                }
            }

            foreach (GameObject obj in touchesOld)
            {
                if(!touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else if(!Input.GetKey(KeyCode.Mouse0))
        {
            pressed = false;
        }
    }

    private void SetTouchPointIn3D(RaycastHit hit)
    {
        touchPointIn3D = hit.point;
    }

    private void SetTouchPointIn3D(Ray ray)
    {
        touchPointIn3D = ray.origin + ray.direction * range;
    }
}
