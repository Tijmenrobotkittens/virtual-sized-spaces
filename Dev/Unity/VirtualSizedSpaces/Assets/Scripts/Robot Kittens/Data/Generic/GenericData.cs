using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericData
{
    private Dictionary<string, int> _ints = new Dictionary<string, int>();
    private Dictionary<string, string> _strings = new Dictionary<string, string>();
    private Dictionary<string, float> _floats = new Dictionary<string, float>();
    private Dictionary<string, bool> _bools = new Dictionary<string, bool>();

    public void Set(string key, int value) {
        _ints[key] = value;
    }

    public void Set(string key, string value)
    {
        _strings[key] = value;
    }

    public void Set(string key, float value)
    {
        _floats[key] = value;
    }

    public void Set(string key, bool value)
    {
        _bools[key] = value;
    }

    public int GetInt(string key) {
        int ret = -1;
        if (_ints.ContainsKey(key)) {
            ret = _ints[key];
        }
        return ret;
    }

    public string GetString(string key)
    {
        string ret = "";
        if (_strings.ContainsKey(key))
        {
            ret = _strings[key];
        }
        return ret;
    }

    public float GetFloat(string key)
    {
        float ret = -1;
        if (_floats.ContainsKey(key))
        {
            ret = _floats[key];
        }
        return ret;
    }

    public bool GetBool(string key)
    {
        bool ret = false;
        if (_bools.ContainsKey(key))
        {
            ret = _bools[key];
        }
        return ret;
    }


    public Dictionary<string, int> Ints {
        get {
            return _ints;
        }
    }

    public Dictionary<string, string> Strings
    {
        get
        {
            return _strings;
        }
    }

    public Dictionary<string, float> Floats
    {
        get
        {
            return _floats;
        }
    }

    public Dictionary<string, bool> Bools
    {
        get
        {
            return _bools;
        }
    }

    public Dictionary<string, string> GetValues() {
        Dictionary<string, string> vals = new Dictionary<string, string>();
        foreach (KeyValuePair<string, int> keyval in Ints)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, string> keyval in Strings)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, float> keyval in Floats)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, bool> keyval in Bools)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }
        return vals;
    }

    public override string ToString()
    {
        string ret = "";
        foreach (KeyValuePair<string, int> keyval in Ints)
        {
            ret += "Ints\n-----------\n";
            ret += keyval.Key + " : " + keyval.Value + "\n";
        }

        foreach (KeyValuePair<string, string> keyval in Strings)
        {
            ret += "Strings\n-----------\n";
            ret += keyval.Key + " : " + keyval.Value + "\n";
        }

        foreach (KeyValuePair<string, float> keyval in Floats)
        {
            ret += "Floats\n-----------\n";
            ret += keyval.Key + " : " + keyval.Value + "\n";
        }

        foreach (KeyValuePair<string, bool> keyval in Bools)
        {
            ret += "Bools\n-----------\n";
            ret += keyval.Key + " : " + keyval.Value + "\n";
        }
        return ret;
    }



}
