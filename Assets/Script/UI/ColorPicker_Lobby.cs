using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker_Lobby : MonoBehaviour
{
    public static ColorPicker_Lobby instance;

    public Color color;
    RectTransform Rect;
    Texture2D ColorTexture;
    public GameObject ColorOfTop;
    public GameObject ColorOfPants;
    public GameObject ColorOfShoes;

    Renderer objectThatChange;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        objectThatChange = ColorOfTop.GetComponent<Renderer>();
        Rect = GetComponent<RectTransform>();

        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    void Update()
    {
        if(RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);
            float width = Rect.rect.width;
            float height = Rect.rect.height;
            delta += new Vector2(width *.5f, height *.5f);
            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);
            int texX = Mathf.RoundToInt(x * ColorTexture.width);
            int texY = Mathf.RoundToInt(y * ColorTexture.height);
            if (Input.GetMouseButtonDown(0))
            {
                color = ColorTexture.GetPixel(texX, texY);
                objectThatChange.material.color = color;
            }
        }
    }

    public void SelectTop() { objectThatChange = ColorOfTop.GetComponent<Renderer>(); }
    public void SelectPants() { objectThatChange = ColorOfPants.GetComponent<Renderer>(); }
    public void SelectShoes() { objectThatChange = ColorOfShoes.GetComponent<Renderer>(); }
}
