using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFocusController : MonoBehaviour
{
    private List<TMP_InputField> _inputFields = new List<TMP_InputField>();
    public static float KeyboardHeight = Config.height * 0.45f;
    public float AdditionalHeight = 0;
    private static TextFocusController _instance;
    public static TextFocusController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("TextFocusController").AddComponent<TextFocusController>();
            }
            return _instance;
        }
    }

    public void SetFocus(List<TMP_InputField> fields, float initialPosition, Action<float> callback)
    {
        _inputFields.Clear();
        foreach (var field in fields)
        {
            field.onSelect.AddListener(text => {callback.Invoke(KeyboardHeight + AdditionalHeight);});
            field.onDeselect.AddListener(text => {callback.Invoke(initialPosition);});
            _inputFields.Add(field);
        }
    }

    private void OnDestroy() 
    {
        foreach (var field in _inputFields)
        {
            field.onSelect.RemoveAllListeners();
            field.onDeselect.RemoveAllListeners();
        }
    }
}
