using System;
using System.Collections.Generic;

[Serializable]
public class ResponseIntList {
    public string key;
    public int value;
}

[Serializable]
public class ResponseStringList
{
    public string key;
    public string value;
}


[Serializable]
public class SendValueResponse
{
    public bool success;
    public int error;
    public List<ResponseIntList> ints;
    public List<ResponseStringList> strings;
}