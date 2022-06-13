using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtPlayer : MonoBehaviour
{
    public string lookFor = "Player";
    public float rotationOffset = 0f;

    protected GameObject player;

    // Start is called before the first frame update
    protected void Awake()
    {
        SetPlayer();
    }

    // Update is called once per frame
    protected void LateUpdate()
    {
        if (player == null)
        {
            SetPlayer();
        }
        else
        {
            transform.LookAt(player.transform.position);
            //transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationOffset);
        }
    }

    protected void SetPlayer()
    {
        if (!string.IsNullOrWhiteSpace(lookFor))
        {
            player = GameObject.FindGameObjectWithTag(lookFor);
        }
        else
        {
            Debug.LogError("There is no " + lookFor + " object in the scene. Help me here: " + gameObject.name);
        }
    }
}
