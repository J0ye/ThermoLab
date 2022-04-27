using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFunctionScript : MonoBehaviour
{
    public List<Material> colorTable = new List<Material>();

    private Material standardMaterial;
    protected int colorIndex;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Renderer>() != null)
        {
            standardMaterial = GetComponent<Renderer>().material;
        }
    }

    public void SpawnPrefab(GameObject prefab)
    {
        GameObject gb = Instantiate(prefab, transform);
    }

    public void ChangeColorTo(int index)
    {
        GetComponent<Renderer>().material = colorTable[index];
    }

    public void DoColorRotation()
    {
        if(colorIndex >= colorTable.Count)
        {
            ResetColor();
            colorIndex = 0;
        }
        else
        {
            ChangeColorTo(colorIndex);
            colorIndex++;
        }
    }

    public void ResetColor()
    {
        GetComponent<Renderer>().material = standardMaterial;
    }
}
