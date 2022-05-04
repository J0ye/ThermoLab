using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    public GameObject prefab;

    protected MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void Spawn()
    {
        mr.enabled = false;

        GameObject temp = Instantiate(prefab, transform.position, prefab.transform.rotation);
        temp.GetComponent<DragItem>().spawnParent = gameObject;
    }

    public void ReadySpawner()
    {
        mr.enabled = true;
    }
}
