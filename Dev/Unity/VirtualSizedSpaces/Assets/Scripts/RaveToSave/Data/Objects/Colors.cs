    using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ColorsData
{
    public static Dictionary<string, Color> colors = new Dictionary<string, Color>
    {
        //Generic
        { "dark_background", ("#1d1d1d".color()) },
        { "darkgrey_background", ("#303030".color()) },
        { "grey_background", ("#404040".color()) },
        { "light_grey", ("#8C8C8C".color()) },
        { "unselected_icon_grey", ("#787878".color()) },
        { "unselected_grey", ("#272727".color()) },
        { "unselected_lightgrey", ("#4C4C4C".color()) },
        { "example_name", ("#24418f".color()) },
        { "black", ("#000000".color()) },
        { "white", ("#ffffff".color()) },
        { "grey", ("#808080".color()) },
        { "error", ("#cd002c".color()) }

    };

    public static string[] GetStrings() {
        List<string> keys = new List<string>();
        foreach (KeyValuePair<string, Color> entry in colors)
        {
            keys.Add(entry.Key);
        }
        return keys.ToArray();
    }
}
