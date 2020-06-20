using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ValueContainer : MonoBehaviour
{
    public TextMeshProUGUI value;
    private GameObject _minMaxSlider;
    private RectTransform valueRectTransform;
    private void Awake()
    {
        _minMaxSlider = transform.parent.gameObject;

        value = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        value.ForceMeshUpdate();
        Vector2 valueSize = value.GetPreferredValues();
        valueRectTransform = value.GetComponent<RectTransform>();
        valueRectTransform.sizeDelta = new Vector2(valueSize.x, 50);
        SetAnchoredPosition(value.gameObject, new Vector2(0, 165));
    }

    public void SetTextValues(float fontSize, float ySpacing)
    {
        value.fontSize = fontSize;
        Vector2 valueSize = value.GetPreferredValues();
        valueRectTransform = value.GetComponent<RectTransform>();
        SetAnchoredPosition(value.gameObject, new Vector2(0, ySpacing));
    }

    public void SetValuePosition(Vector2 newPosition)
    {
        SetAnchoredPosition(value.gameObject, newPosition);
    }

    private void SetAnchoredPosition(GameObject obj, Vector2 position)
    {
        obj.GetComponent<RectTransform>().anchoredPosition = position;
    }
}
