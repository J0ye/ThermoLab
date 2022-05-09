using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistanceRenderer : MonoBehaviour
{
    public bool checkParent = true;
    public GameObject textPrefab;
    public List<GameObject> targets = new List<GameObject>();

    protected LineRenderer lr;
    protected Dictionary<GameObject, GameObject> targetDisplayDicitionary = new Dictionary<GameObject, GameObject>();
    protected Dictionary<GameObject, float> calculatedDistances = new Dictionary<GameObject, float>();

    // Update is called once per frame
    void LateUpdate()
    {
        if (lr == null) SetLineRenderer();
        if(targets.Count > 0)
        {
            PaintLine();
            DisplayDistance();
        }
    }

    public float GetDistanceInMeter(GameObject target)
    {
        if(calculatedDistances.ContainsKey(target))
        {
            return calculatedDistances[target];
        }
        return 0f;
    }

    protected void PaintLine()
    {
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);

        int counter = 1;
        foreach(GameObject gb in targets)
        {
            if (IsTargetActive(gb))
            {
                lr.positionCount = lr.positionCount + 2;
                lr.SetPosition(counter, gb.transform.position);
                lr.SetPosition(counter + 1, transform.position);
                counter += 2;
            }
        }
    }

    protected void DisplayDistance()
    {
        foreach(GameObject target in targets)
        {
            if(!targetDisplayDicitionary.ContainsKey(target))
            {
                GameObject newDisplay = Instantiate(textPrefab, transform);
                targetDisplayDicitionary.Add(target, newDisplay);
                calculatedDistances.Add(target, 0f);
            }

            if(IsTargetActive(target))
            {
                targetDisplayDicitionary[target].SetActive(true);
                GameObject display = targetDisplayDicitionary[target];
                Vector3 dirToTarget = target.transform.position - transform.position;
                Vector3 newPos = transform.position + dirToTarget / 2;
                float dist = Vector3.Distance(transform.position, target.transform.position);
                dist = (dist * 8) / 100;
                calculatedDistances[target] = dist;
                display.transform.position = newPos;
                dist = (float)System.Math.Round(dist * 100f) / 100f;
                display.GetComponent<TextMesh>().text = dist.ToString() + " m";
            }
            else
            {
                targetDisplayDicitionary[target].SetActive(false);
            }
        }
    }

    protected bool IsTargetActive(GameObject target)
    {
        if(checkParent)
        {
            if(target.activeSelf && target.transform.parent.gameObject.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return target.activeSelf;
    }

    protected void SetLineRenderer()
    {
        lr = GetComponent<LineRenderer>();
    }
}
