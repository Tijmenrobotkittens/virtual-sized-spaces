using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct RangeSlider
{
    public GameObject slider1;
    public GameObject slider2;

    public RangeSlider(GameObject newSlider1, GameObject newSlider2)
    {
        slider1 = newSlider1;
        slider2 = newSlider2;
    }
}

public class SliderKnobBaseClass : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public delegate void OnIndexChangeDelegate(int newIndex);
    public Action<int> OnValueChanged;

    private int _value;

    private Image _image;
    private RectTransform _rectTransform;
    private TextMeshProUGUI _text;
    private List<SegmentObject> _segmentObjects = new List<SegmentObject>();
    private GameObject _otherSliderObject;

    private bool _beginDrag, _leftSlider, _twoSliders;
    private int _index;
    private int _lastIndex;
    private float _minMaxMargin;
    MinMaxSlider _parentSlider;
    RectTransform _parentRectTransform;
    private Color _initialColor, _changedColor;
    private Vector3 _position;

    public virtual void Set(int value, Color color, Color onDragColor, Vector3 scale, bool twoSliders, bool leftSlider, GameObject otherSliderObject = null)
    {
        _minMaxMargin = 2.0f;
        _image = transform.Find("Image").GetComponent<Image>();
        _index = value;
        _image.color = color;
        _initialColor = color;
        _changedColor = onDragColor;
        _rectTransform = GetComponent<RectTransform>();
        transform.localScale = scale;
        _text = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        _twoSliders = twoSliders;
        _leftSlider = leftSlider;
        _otherSliderObject = otherSliderObject;
        _parentSlider = GetComponentInParent<MinMaxSlider>();
        _parentRectTransform = GetComponentInParent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            KeepWithinBounds();
            SetToClosestSegment();
            //SetText(Index.ToString());
            yield return null;
        }
    }

    public virtual void SetText(string sliderValue)
    {
        _text.text = sliderValue;
    }

    public virtual int Index
    {
        get { return _parentSlider.MinValue + _index; }
        set
        {
            if (value == -1)
            {
                value = _index = _parentSlider.MaxValue;
            }

            _index = value - _parentSlider.MinValue;

            if (_index == -1)
            {
                _index = _parentSlider.MaxValue;
            } 
            GetComponent<RectTransform>().anchoredPosition = new Vector3(_parentSlider.segmentPositionsX[_index], 0, 0);
        }
    }

    protected virtual void KeepWithinBounds()
    {
        if (_beginDrag && _twoSliders)
        {
            // Set bounds for left slider.
            if (_leftSlider)
            {
                if (_rectTransform.anchoredPosition.x >= _parentSlider.xMin - _minMaxMargin &&
                    _rectTransform.anchoredPosition.x <= _otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x)
                {
                    _rectTransform.position = new Vector3(_position.x, _parentRectTransform.position.y, 0);
                }

                if (_rectTransform.anchoredPosition.x <= _parentSlider.xMin - _minMaxMargin)
                    _rectTransform.anchoredPosition = new Vector3(_parentSlider.xMin + 0.05f, 0, 0);

                if (_rectTransform.anchoredPosition.x >= _otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x - _minMaxMargin)
                    _rectTransform.anchoredPosition = new Vector3(_otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x - 3.0f, 0, 0);
            }

            // Set bounds for right slider.
            if (!_leftSlider)
            {
                if (_rectTransform.anchoredPosition.x >= _otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x - _minMaxMargin
                    && _rectTransform.anchoredPosition.x < _parentSlider.xMax + _minMaxMargin)
                {
                    _rectTransform.position = new Vector3(_position.x, _parentRectTransform.position.y, 0);
                }

                if (_rectTransform.anchoredPosition.x <= _otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x - _minMaxMargin)
                    _rectTransform.anchoredPosition = new Vector3(_otherSliderObject.GetComponent<RectTransform>().anchoredPosition.x + 0.05f, 0, 0);


                if (_rectTransform.anchoredPosition.x >= _parentSlider.xMax + _minMaxMargin)
                    _rectTransform.anchoredPosition = new Vector3(_parentSlider.xMax - 0.05f, 0, 0);
            }
        }

        // Set bounds for single slider.
        else if (_beginDrag && !_twoSliders)
        {
            if (_rectTransform.anchoredPosition.x > _parentSlider.xMin - _minMaxMargin && _rectTransform.anchoredPosition.x < _parentSlider.xMax + _minMaxMargin)
            {
                _rectTransform.position = new Vector3(_position.x, _parentRectTransform.position.y, 0);
            }

            if (_rectTransform.anchoredPosition.x < _parentSlider.xMin - _minMaxMargin)
                _rectTransform.anchoredPosition = new Vector3(_parentSlider.xMin + 0.05f, 0, 0);

            if (_rectTransform.anchoredPosition.x > _parentSlider.xMax + _minMaxMargin)
                _rectTransform.anchoredPosition = new Vector3(_parentSlider.xMax - 0.05f, 0, 0);
        }
    }

    protected virtual void SetToClosestSegment()
    {
        SegmentObject closestSegment;

        if (_parentSlider != null)
        {
            closestSegment = GetClosestSegment(_parentSlider.segmentPositionsX, _rectTransform.anchoredPosition.x);

            _rectTransform.anchoredPosition = new Vector3(closestSegment.positionX, _parentRectTransform.anchoredPosition.y, 0);
            _index = closestSegment.index;

            if (_lastIndex != Index)
            {
            }

            _lastIndex = Index;
            // Clear list after setting the position!!
            _segmentObjects.Clear();
        }
    }

    private SegmentObject GetClosestSegment(Dictionary<int, float> segments, float currentPositionX)
    {
        // For every segment, look where the _recttransform.position.x is closest to.
        foreach (KeyValuePair<int, float> segment in segments)
        {
            // Segment.key = index. Segment.value = position of x-axis.
            float distanceToSegment = Mathf.Abs(segment.Value - currentPositionX);
            _segmentObjects.Add(new SegmentObject(segment.Key, segment.Value, distanceToSegment));
        }

        // Find closest segment position x according to distance.
        // Then return the closest segment position x.
        return _segmentObjects.OrderBy(s => Mathf.Abs(s.positionX - currentPositionX)).First();
    }

    public void OnIndexChanged(int newIndex)
    {
        //Debug.Log("INDEX HAS CHANGED TO: " + newIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDrag = true;
        _image.color = _changedColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _beginDrag = false;
        _image.color = _initialColor;
        OnValueChanged?.Invoke(Index); // If the index changed invoke onvaluechanged
    }

    public void OnDrag(PointerEventData eventData)
    {
        _position = eventData.position;

        // This makes the dragged slider always the top one.
        gameObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
    }
}

