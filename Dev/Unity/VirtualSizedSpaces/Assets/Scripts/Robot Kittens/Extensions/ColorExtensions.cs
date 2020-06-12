using UnityEngine;
 using System.Linq;
 using System.Collections.Generic;

public static class ColorExtensions
{

    public static Color modify(this Color color, float modifyvalue)
    {
        color.r = color.r + (modifyvalue / 255);
        color.g = color.g + (modifyvalue / 255);
        color.b = color.b + (modifyvalue / 255);
        return color;
    }

    public static Color invert(this Color color)
    {
        color.r = 1-color.r;
        color.g = 1-color.g;
        color.b = 1-color.b;
        return color;
    }

    public static Color alpha(this Color color, float alpha)
    {
        return new Color(color.r,color.g,color.b,alpha);
    }
}