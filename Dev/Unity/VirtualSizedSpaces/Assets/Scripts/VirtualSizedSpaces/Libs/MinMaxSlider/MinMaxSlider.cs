using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct SegmentObject
{
    public int index;
    public float positionX;
    public float distanceFrom;

    public SegmentObject(int newIndex, float newPositionX, float newDistanceFrom)
    {
        index = newIndex;
        positionX = newPositionX;
        distanceFrom = newDistanceFrom;
    }
}

public class MinMaxSlider : MonoBehaviour
{
    public float segmentSpacing;
    public GameObject innerBar;
    public float xMax, xMin;
    public GameObject sliderContainer;
    public GameObject valueContainer;

    // Here the int is the segment and float is the x-position of that segment.
    public Dictionary<int, float> segmentPositionsX = new Dictionary<int, float>();

    public bool twoSliders;
    private SliderKnob _leftSlider, _rightSlider;
    private RangeSlider _rangeSliderObject;
    private int _minValue, _maxValue;
    private string _valueType;
    private bool _useUnlimited;
    private float _valueContainerSpacing;
    private float _valueContainerFontSize;

    /// <param name="hasTwoSliders"> Whether you want two or one slider.</param>
    /// <param name="position"> Position of the slider.</param>
    /// <param name="valueContainerPosition"> Position of the slider value(s).</param>
    /// <param name="sliderWidth"> How wide the slider is.</param>
    /// <param name="sliderHeight"> How tall the slider is.</param>
    /// <param name="minValue"> Lowest value possible in the slider.</param>
    /// <param name="maxValue"> Highest value possible in the slider.</param>
    /// <param name="innerMinValue"> Left slider's starting value.</param>
    /// <param name="innerMaxValue"> Right slider's starting value.</param>
    /// <param name="knobDirectory"> Set different image when the knob directory is given.</param>
    /// <param name="initialHexColor"> Initial colour of the knob.</param>
    /// <param name="changedHexColor"> Colour of the knob when it is dragged.</param>
    /// <param name="innerBarDirectory"> Set different image when the innerBar directory is given.</param>
    /// <param name="minMaxValueType"> What do the values stand for? Is it for AGE? Is it for KM?.</param>
    public void Set(bool hasTwoSliders, int sliderWidth, int sliderHeight, int minValue, int maxValue, int innerMinValue, int innerMaxValue, string knobDirectory, string initialHexColor, string changedHexColor, string innerBarDirectory = "", string minMaxValueType = "Pepernoten", Action<int> onMinValueChanged = null, Action<int> onMaxValueChanged = null, bool useUnlimited = false)
    {
        twoSliders = hasTwoSliders;
        _minValue = minValue;
        _valueType = minMaxValueType;
        _useUnlimited = useUnlimited;

        if (_useUnlimited)
        {
            _maxValue = maxValue + 1;
        } else
        {
            _maxValue = maxValue;
        }

        SetSize(sliderWidth, sliderHeight);
        GetMinMax();
        DefineSegments(MinValue, MaxValue);

        sliderContainer = HelperFunctions.GetPrefab2d("Prefabs/Others/SliderContainer", gameObject);
        valueContainer = HelperFunctions.GetPrefab2d("Prefabs/Others/ValueContainer", gameObject);

        SetAnchoredPosition(sliderContainer, Vector3.zero);

        if (!string.IsNullOrEmpty(innerBarDirectory))
        {
            // Add bar for the min and max sliders.
            // Height should be the same as the height in SetSize!
            innerBar = CreateInnerBar(width: 0, height: 20, innerBarDirectory: innerBarDirectory);
        }

        Vector3 sliderKnobScale = new Vector3(0.5f, 0.5f, 0.5f);

        _rangeSliderObject = SpawnSliderObjects(innerMinValue, innerMaxValue, knobDirectory, initialHexColor, changedHexColor, sliderKnobScale);

        _leftSlider = _rangeSliderObject.slider1.GetComponent<SliderKnob>();
        _leftSlider.OnValueChanged += onMinValueChanged;
        if (hasTwoSliders)
        {
            _rightSlider = _rangeSliderObject.slider2.GetComponent<SliderKnob>();
            _rightSlider.OnValueChanged += onMaxValueChanged;
        }

        _valueContainerSpacing = 165;
        _valueContainerFontSize = 45;
    }

    public void SetValueContainerText(float fontSize, float ySpacing)
    {
        _valueContainerSpacing = ySpacing;
        _valueContainerFontSize = fontSize;
    }

    private void LateUpdate()
    {
        if (twoSliders && innerBar != null)
            ConnectToSliders(_rangeSliderObject);
        else if (!twoSliders && innerBar != null)
            ConnectToSlider(_rangeSliderObject.slider1);

        SetValueContainer();
    }

    private void SetValueContainer() // TODO heavy om dit elke frame aan te roepen
    {
        ValueContainer valueContainerScript;

        if (valueContainer != null)
        {
            valueContainerScript = valueContainer.GetComponent<ValueContainer>();
            valueContainerScript.SetTextValues(_valueContainerFontSize, _valueContainerSpacing);

            if (!twoSliders)
            {
                if (_rangeSliderObject.slider1.GetComponent<SliderKnob>().Index >= _maxValue && _useUnlimited)
                {
                    valueContainerScript.value.text = $"<b>{_maxValue - 1}+ {_valueType}</b>";
                }
                else
                {
                    valueContainerScript.value.text = "<b>" +  _rangeSliderObject.slider1.GetComponent<SliderKnob>().Index + $" {_valueType}</b>";
                }
            }
            else
            {

                if (_rangeSliderObject.slider2.GetComponent<SliderKnob>().Index >= _maxValue && _useUnlimited)
                {
                    valueContainerScript.value.text = "<b>" +  _rangeSliderObject.slider1.GetComponent<SliderKnob>().Index + "</b>" + " - " + "<b>" +  (_maxValue - 1).ToString() + "</b>" + $"+ {_valueType}";
                }
                else
                {
                    valueContainerScript.value.text = "<b>" +  _rangeSliderObject.slider1.GetComponent<SliderKnob>().Index + "</b>" + " - " + "<b>" +  _rangeSliderObject.slider2.GetComponent<SliderKnob>().Index.ToString() + "</b>" + $" {_valueType}";
                }
            }
        }
    }

    private void SetAnchoredPosition(GameObject obj, Vector3 newPosition)
    {
        obj.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }

    private GameObject CreateInnerBar(float width, float height, string innerBarDirectory)
    {
        if (!string.IsNullOrEmpty(innerBarDirectory))
        {
            innerBar = HelperFunctions.GetPrefab2d($"{innerBarDirectory}", sliderContainer);

        }
        else
        {
            innerBar = HelperFunctions.GetPrefab2d("Prefabs/Others/Innerbar", sliderContainer);
        }
        innerBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        return innerBar;
    }

    private void SetSize(float width, float height)
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void GetMinMax()
    {
        xMin = transform.GetComponent<RectTransform>().rect.xMin;
        xMax = transform.GetComponent<RectTransform>().rect.xMax;
    }

    public int From
    {
        get { return _leftSlider.Index; }
        set { _leftSlider.Index = value; }
    }

    public int To
    {
        get { return _rightSlider.Index; }
        set { _rightSlider.Index = value; }
    }

    public int MinValue
    {
        get { return _minValue; }
    }

    public int MaxValue
    {
        get { return _maxValue;}
    }

    // Function to define segments within the bar.
    private void DefineSegments(int minValue, int maxValue)
    {
        float length = Mathf.Abs(xMax - xMin);

        int _segments = Mathf.Abs(minValue - maxValue);

        // Get a general spacing, so it is equal for each segment.
        segmentSpacing = length / _segments;

        for (int i = 0; i < _segments + 1; i++)
        {
            if (i == 0)
                segmentPositionsX.Add(i, xMin);
            else
                segmentPositionsX.Add(i, xMin + i * segmentSpacing);
        }
    }

    // Innerbar is connected from slider a to slider b.
    private void ConnectToSliders(RangeSlider rangeSlider)
    {
        RectTransform rectTransform = innerBar.GetComponent<RectTransform>();

        float length = Mathf.Abs(rangeSlider.slider2.GetComponent<RectTransform>().anchoredPosition.x - rangeSlider.slider1.GetComponent<RectTransform>().anchoredPosition.x);
        float middlePositionX = (rangeSlider.slider2.GetComponent<RectTransform>().anchoredPosition.x + rangeSlider.slider1.GetComponent<RectTransform>().anchoredPosition.x) / 2;

        // Keep height, but change the width according to the length between the two sliders.
        rectTransform.sizeDelta = new Vector2(length, rectTransform.rect.height);

        // Center the position between the two sliders.
        rectTransform.anchoredPosition = new Vector3(middlePositionX, rangeSlider.slider1.GetComponent<RectTransform>().anchoredPosition.y, 0);
    }

    // Innerbar is connected from minX to slider.
    private void ConnectToSlider(GameObject slider)
    {
        RectTransform rectTransform = innerBar.GetComponent<RectTransform>();

        // Length is minimum of x to slider position.
        float length = Mathf.Abs(xMin - slider.GetComponent<RectTransform>().anchoredPosition.x);

        float middlePositionX = (xMin + slider.GetComponent<RectTransform>().anchoredPosition.x) / 2;

        // Keep height, but change the width according to the length between the xMin and slider's current position.
        rectTransform.sizeDelta = new Vector2(length, rectTransform.rect.height);

        // Center the position between the points.
        rectTransform.anchoredPosition = new Vector3(middlePositionX, slider.GetComponent<RectTransform>().anchoredPosition.y, 0);
    }

    private RangeSlider SpawnSliderObjects(int startMinValue, int startMaxValue, string sliderKnobDirectory, string initialColor, string onDragColor, Vector3 scale)
    {
        GameObject slideObject1 = HelperFunctions.GetPrefab2d($"{sliderKnobDirectory}", sliderContainer);
        slideObject1.name = "Slider1";
        GameObject slideObject2 = null;

        if (twoSliders)
        {
            slideObject2 = HelperFunctions.GetPrefab2d($"{sliderKnobDirectory}", sliderContainer);
            slideObject2.name = "Slider2";
            slideObject2.GetComponent<RectTransform>().anchoredPosition = new Vector3(segmentPositionsX[startMaxValue - _minValue], 0, 0);

            slideObject2.GetComponent<SliderKnob>().Set(startMaxValue, initialColor.color(), onDragColor.color(), scale, twoSliders, false, slideObject1);

            slideObject1.GetComponent<RectTransform>().anchoredPosition = new Vector3(segmentPositionsX[startMinValue - _minValue], 0, 0);
        }
        else
        {
            slideObject1.GetComponent<RectTransform>().anchoredPosition = new Vector3(xMin, 0, 0);
        }

        slideObject1.GetComponent<SliderKnob>().Set(startMinValue, initialColor.color(), onDragColor.color(), scale, twoSliders, true, slideObject2);

        return new RangeSlider(slideObject1, slideObject2);
    }

}
