using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerController : MonoBehaviour
{
    public static bool Debugging = true;


    public static Dictionary<string, bool> DebugTypes = new Dictionary<string, bool>
    {
        {"test", true},
       
    };

    public static bool Testing(string key)
    {
        if (DebuggerController.Debugging == false)
        {
            return false;
        }
        if ((Debug.isDebugBuild == false && Application.isEditor == false))
        {
            return false;
        }
        else
        {
            if (DebuggerController.DebugTypes.ContainsKey(key))
            {
                return DebuggerController.DebugTypes[key];
            }
            else
            {
                return false;
            }
        }
    }
}
