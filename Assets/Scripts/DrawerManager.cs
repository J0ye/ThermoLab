using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class DrawerManager : MenuManager
{
    public Vector2 openPosition = Vector2.zero;
    [Range(0f, 5f)]
    public float animationDuration = 1f;

    [Header("Menu Options")]
    public GameObject exitButton;

    protected Tween lastTween;
    protected RectTransform rt;
    protected Vector2 startPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
        openPosition = new Vector2(startPos.x + rt.rect.width/2, openPosition.y);

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            exitButton.SetActive(false);
        }
    }

    protected override void UpdateState()
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
