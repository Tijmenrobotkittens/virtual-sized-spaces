using System;
using System.Collections.Generic;
using UnityEngine;

public class RKLog : MonoBehaviour
{

    public static string[] tags;
    public static string[] ignoretags;
    public enum types {LOG,WARNING,ERROR};
    public static List<types> AlwaysAllowedTypes = new List<types>();
    public static bool Enabled = true;
    public static string _lastlog = "";
    public static bool IgnoreDoubles = true;
    private static bool _innited = false;


    public static void Init()
    {
        if (!_innited) {
            Application.logMessageReceived += HandleLog;
            _innited = true;
        }

    }

    public static void HandleLog(string logString, string stackTrace, LogType type)
    {
        //output = logString;
        //stack = stackTrace;
        Debug.Log("LOG DONE! "+logString);
    }

    public static void SetTags( string tags )
    {
        RKLog.tags = tags.Split(',');
    }

    public static void SetIgnoreTags(string senttags)
    {
        Debug.Log("tijmen 5 setignoretags "+senttags);
        RKLog.ignoretags = senttags.Split(',');
        Debug.Log("tijmen 6 setignoretags " + senttags);
    }

    public static void AddAllowedType( types type )
    {
        AlwaysAllowedTypes.Add(type);
    }


    public static bool CompareTags( string logtags )
    {
        
        if (tags == null)
        {
            return true;
        }
        if (tags.Length == 0)
        {
            return true;
        }
        string[] logtagsa = logtags.Split(',');
		foreach (string tag in logtagsa)
        {
            if (Array.Exists(RKLog.tags, element => element == tag))
            {
                return true;
            }
        }


        return false;
    }

    public static bool CompareIgnoreTags(string logtags)
    {

        if (ignoretags == null)
        {
            return false;
        }
        if (ignoretags.Length == 0)
        {
            return false;
        }
        string[] logtagsa = logtags.Split(',');
        foreach (string tag in logtagsa)
        {
            if (Array.Exists(RKLog.ignoretags, element => element == tag))
            {
                return true;
            }
        }


        return false;
    }

    public static void Log(string message, string tags = "", string color_string = "#00d632" )
    {
        //Init();
        if (!Enabled)
        {
            return;
        }

        if (RKLog.CompareTags(tags) == true && RKLog.CompareIgnoreTags(tags) == false || RKLog.AlwaysAllowedTypes.Contains(types.LOG))
        {
            if (message != _lastlog || IgnoreDoubles == false)
            {
                Debug.Log(
            $"<color={color_string}>RK ({tags}): {message}</color>");
                if (IgnoreDoubles == true)
                {
                    _lastlog = message;
                }
            }
            
        }
    }

    public static void LogWarning(string message, string tags = "", string color_string = "#d8c600" )
    {
        //Init();
        if (!Enabled)
        {
            return;
        }


        if (RKLog.CompareTags(tags) == true && RKLog.CompareIgnoreTags(tags) == false || RKLog.AlwaysAllowedTypes.Contains(types.WARNING))
        {
            if (message != _lastlog || IgnoreDoubles == false)
            {
                Debug.LogWarning(
            $"<color={color_string}>RK ({tags}): {message}</color>");
                if (IgnoreDoubles == true)
                {
                    _lastlog = message;
                }
            }
        }

        
    }

    public static void LogError(string message, string tags = "", string color_string = "#ea0016" )
    {
        //Init();
        if (!Enabled)
        {
            return;
        }

        if (RKLog.CompareTags(tags) == true && RKLog.CompareIgnoreTags(tags) == false || RKLog.AlwaysAllowedTypes.Contains(types.ERROR))
        {
            if (message != _lastlog || IgnoreDoubles == false)
            {
                Debug.LogError(
            $"<color={color_string}>RK ({tags}): {message}</color>");
                if (IgnoreDoubles == true)
                {
                    _lastlog = message;
                }
            }
        }
    }
}

