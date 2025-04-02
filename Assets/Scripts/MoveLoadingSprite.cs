using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLoadingSprite : MonoBehaviour
{
    public Slider LoadingBar;
    public RectTransform fillArea;

    private RectTransform spriteTr;
    private float minX, maxX;

    void Start()
    {
        // LoadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
        // fillArea = LoadingBar.GetComponent<RectTransform>();
        spriteTr = GetComponent<RectTransform>();

        minX = fillArea.rect.xMin;
        maxX = fillArea.rect.xMax;
    }

    void Update()
    {
        float newX = Mathf.Lerp(minX, maxX, LoadingBar.value);
        spriteTr.anchoredPosition = new Vector2(newX, spriteTr.anchoredPosition.y);
    }
}
