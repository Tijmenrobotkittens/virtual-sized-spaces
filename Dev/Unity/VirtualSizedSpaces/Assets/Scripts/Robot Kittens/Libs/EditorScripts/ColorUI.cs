using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorUI : MonoBehaviour {
    private TextMeshProUGUI textmesh;
    private Image image;
    private RawImage rawimage;
    public int modify = 0;
    public float alpha = 1;
    [StringInList(typeof(ColorsData), "GetStrings")] public string color;

	void Awake () {

        textmesh = gameObject.GetComponent<TextMeshProUGUI>();
        image = gameObject.GetComponent<Image>();
        rawimage = gameObject.GetComponent<RawImage>();
        UpdateColor();
	}

    private void UpdateColor(){
        if (image)
        {
            if (DataHolder.colors.ContainsKey(color))
            {
				image.color = DataHolder.colors[color].modify(modify).alpha(alpha);
            }

        }

        if (rawimage)
        {
            if (DataHolder.colors.ContainsKey(color))
            {
                rawimage.color = DataHolder.colors[color].modify(modify).alpha(alpha);
            }

        }

        if (textmesh)
		{
			textmesh.color = DataHolder.colors[color].modify(modify).alpha(alpha);
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