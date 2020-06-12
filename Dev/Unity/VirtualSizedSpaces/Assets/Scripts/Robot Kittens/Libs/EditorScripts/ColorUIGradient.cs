using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[ExecuteInEditMode]
public class ColorUIGradient : MonoBehaviour {
    private Gradient2 _gradient;
	public int modify = 0;
    public float alpha = 1;

    [StringInList(typeof(ColorsData), "GetStrings")] public string color;
	[StringInList(typeof(ColorsData), "GetStrings")] public string color2;
    


	void Awake () {


        _gradient = GetComponent<Gradient2>();
      
        UpdateColor();
	}

    private void UpdateColor(){
        if (_gradient)
        {
            if (DataHolder.colors.ContainsKey(color) && DataHolder.colors.ContainsKey(color2))
            {
                GradientColorKey k1 = new GradientColorKey(DataHolder.colors[color], 0);
                GradientColorKey k2 = new GradientColorKey(DataHolder.colors[color2], 1);
                _gradient.EffectGradient = new UnityEngine.Gradient() { colorKeys = new GradientColorKey[] { k1, k2 } };

            }
        }
    }

    void OnGUI()
   {
   }

    private void OnValidate()
    {

        UpdateColor();
    }

}