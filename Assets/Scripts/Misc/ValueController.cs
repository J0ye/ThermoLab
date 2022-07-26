using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ValueController : MonoBehaviour
{
    public Vector2 forwardBounds = Vector2.zero;
    public Vector2 sidewayBounds = Vector2.zero;
    public Vector2 upwardBounds = Vector2.zero;

    protected Vector3 startpos;
    protected Vector3 startrot;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        startrot = transform.rotation.eulerAngles;
    }

    public void MoveForward(float value)
    {
        Vector3 calc = transform.forward * value;
        transform.position = startpos + calc;

        ClampPosition();
    }
    public void MoveSideway(float value)
    {
        Vector3 calc = new Vector3(startrot.x, startrot.y + (value*360), startrot.z);
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(calc));

        ClampPosition();
    }

    public void MoveUpward(float value)
    {
        Vector3 calc = transform.up * value;
        transform.position = startpos + calc;

        ClampPosition();
    }

    private void ClampPosition()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        x = Mathf.Clamp(x, sidewayBounds.x, sidewayBounds.y);
        y = Mathf.Clamp(y, upwardBounds.x, upwardBounds.y);
        z = Mathf.Clamp(z, forwardBounds.x, forwardBounds.y);
        Vector3 clampedPosition = new Vector3(x, y, z);
        transform.position = clampedPosition;
    }
}
