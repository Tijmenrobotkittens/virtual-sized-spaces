using UnityEngine;
 using System.Linq;
 using System.Collections.Generic;
using System;
using I2.Loc;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static Color color(this string hex) {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public static string UCFirst(this string input)
    {
        return input.First().ToString().ToUpper() + input.Substring(1);
    }

    public static string GetTranslation(this string input, string key)
    {
        string text = "Cannot find translation";
        string trans = LocalizationManager.GetTranslation(key);
        if (trans!=null) text = trans;
        return text.Replace("\r", "\r\n");
    }

    public static T ToEnum<T>(this string enumString)
    {
        return (T)Enum.Parse(typeof(T), enumString);
    }

    public static string ParseLinks(this string input, string color = null) {

        Regex rgx = new Regex("<link=\"(.+?)\">(.+?)</link>", RegexOptions.Multiline);

        if (color != null)
        {
            input = rgx.Replace(input, "<link=\"$1\"><u><color="+ color +">$2</color></u></link>");
        }
        else
        {

            input = rgx.Replace(input, "<link=\"$1\"><u>$2</u></link>");
        }

        return input;
    }



}