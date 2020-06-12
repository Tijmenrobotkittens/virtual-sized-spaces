using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class MultipleBooleanChecker
{
    public UnityEvent AllTrue = new UnityEvent();
    private Dictionary<string, bool> _boolsToCheck = new Dictionary<string, bool>();

    public void Set(string name, bool value = false)
    {
        _boolsToCheck[name] = value;
        CheckBools();
    }

    public bool Get(string name) {
        return _boolsToCheck[name];
    }

    private void CheckBools()
    {
        bool result = true;
        foreach (KeyValuePair<string, bool> res in _boolsToCheck)
        {
            if (res.Value == false)
            {
                result = false;
            }
        }
        if (result == true) {
            AllTrue.Invoke();
        }

    }
}
