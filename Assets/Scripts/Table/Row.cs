using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Row : MonoBehaviour
{
    public List<InputField> lines = new List<InputField>();
    // Start is called before the first frame update
    void Start()
    {
        SetUpList();
    }

    protected void SetUpList()
    {
        lines = new List<InputField>();
        if (transform.childCount > 1)
        {
            foreach (Transform child in transform.GetChild(1))
            {
                InputField temp;
                if (child.TryGetComponent<InputField>(out temp))
                {
                    lines.Add(temp);
                }
            }

        }
    }
}
