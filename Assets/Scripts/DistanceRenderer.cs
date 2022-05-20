using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DistanceRenderer : MonoBehaviour
{
    public bool checkParent = true;
    public GameObject textPrefab;
    public List<GameObject> targets = new List<GameObject>();
    [Header("UI Settings")]
    public Text uiText;
    public bool renderLine = true;
    public bool renderText = false;

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

    public float GetDistanceInMeter(int index)
    {
        if (calculatedDistances.Values.Count > index)
        {
            int counter = 0;
            foreach(float f in calculatedDistances.Values)
            {
                if(counter == index)
                {
                    return f;
                }
                counter++;
            }
        }
        return 0f;
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

        if(renderLine)
        {
            int counter = 1;
            foreach (GameObject gb in targets)
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
    }

    protected void DisplayDistance()
    {
        Debug.Log("Begin");
        foreach (GameObject target in targets)
        {
            Debug.Log("0 " + target);
            if (!targetDisplayDicitionary.ContainsKey(target))
            {
                GameObject newDisplay = Instantiate(textPrefab, transform);
                targetDisplayDicitionary.Add(target, newDisplay);
                calculatedDistances.Add(target, 0f);
            }
            Debug.Log("1 " + target);

            Vector3 dirToTarget = target.transform.position - transform.position;
            Vector3 newPos = transform.position + dirToTarget / 2;
            float dist = Vector3.Distance(transform.position, target.transform.position);
            dist = (dist * 8) / 100;
            calculatedDistances[target] = dist;
            Debug.Log("End");

            if (IsTargetActive(target))
            {
                if(renderText)
                {
                    dist = (float)System.Math.Round(dist * 100f) / 100f;
                    targetDisplayDicitionary[target].SetActive(true);
                    GameObject display = targetDisplayDicitionary[target];
                    display.transform.position = newPos;
                    display.GetComponent<TextMesh>().text = dist.ToString() + " m";
                }
                if (uiText != null) uiText.text = dist.ToString() + " m";
            }
            else
            {
                targetDisplayDicitionary[target].SetActive(false);
                if (uiText != null) uiText.text = "No Tracking";
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
