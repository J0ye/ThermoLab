using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class DrawerManager : MonoBehaviour
{
    public Vector2 openPosition = Vector2.zero;
    [Range(0f, 5f)]
    public float animationDuration = 1f;
    public bool state = false;

    protected Tween lastTween;
    protected RectTransform rt;
    protected Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
        openPosition = new Vector2(startPos.x + rt.rect.width/2, openPosition.y);
    }

    public void SwitchState()
    {
        state = !state;
        UpdateState();
    }

    protected void UpdateState()
    {
        if(state)
        {
            CompleteLastTween();
            lastTween = rt.DOAnchorPos(openPosition, animationDuration);
        }
        else
        {
            CompleteLastTween();
            lastTween = rt.DOAnchorPos(startPos, animationDuration);
        }
    }

    protected void CompleteLastTween()
    {
        if(lastTween != null)
        {
            if(lastTween.active)
            {
                lastTween.Complete();
            }
        }
    }
}
