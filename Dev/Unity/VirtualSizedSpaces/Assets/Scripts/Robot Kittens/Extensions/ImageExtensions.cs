using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;
using RobotKittens;

public enum ScaleMethods {NONE,FILL,FILL_HORIZONTAL,FILL_VERTICAL};


public static class ImageExtensions
{
    public static void loadFromResource(this Image img, string filename, string atlasfilename)
    {
        bool loaded = false;
        Sprite[] _sprites = Resources.LoadAll<Sprite>(atlasfilename);
        foreach (Sprite sprite in _sprites) {
            if (sprite.name == filename) {
                img.sprite = sprite;
                loaded = true;
            }
        }

        if (loaded == false) {
            Debug.LogError("ImageExtensions could not load image "+filename + " from "+atlasfilename);
            if (_sprites == null)
            {
                Debug.LogError("ImageExtensions atlas can not be found" + atlasfilename);
            }
            else
            {
                foreach (Sprite sprite in _sprites)
                {
                    Debug.LogError("ImageExtensions sprite found in atlas " + sprite);
                }
            }
        }


    }


    public static void loadFromResource(this Image img, string filename, ScaleMethods fill = ScaleMethods.NONE) {
        Debug.Log(img);
        Debug.Log(filename);
        Sprite sprite = Resources.Load<Sprite>(filename);

        //Debug.LogError(sprite.texture.width+"x"+sprite.texture.height);

        if (sprite)
        {


            float iwidth = sprite.texture.width;
            float iheight = sprite.texture.height;

			try {
				img.sprite = Resources.Load<Sprite>(filename);
			}
			catch (Exception e)
			{
				Debug.LogError("er ging iets mis met het laden van "+ filename);
			}

            if (fill == ScaleMethods.FILL)
            {
                //Debug.LogError("komt ie");
                //Debug.LogError(sprite);
                //Debug.LogError(filename);

                RectTransform imt = img.GetComponent<RectTransform>();
                Vector2 size = imt.sizeDelta;
                //Debug.Log(size);
                float containerwidth = size[0];
                float containerheight = size[1];
                var factor1 = containerwidth / containerheight;
                var factor2 = iwidth / iheight;

                //Debug.Log(" factor1:"+factor1 + " factor2:"+factor2 );

                if (factor1 > factor2)
                {
                    float newwidth = containerwidth;
                    float newheight = (containerwidth / iwidth) * iheight;
                    //Debug.Log("1: " + newwidth + "x" + newheight);
                    imt.sizeDelta = new Vector2(newwidth, newheight);
                }
                else
                {
                    float newwidth = (containerheight / iheight) * iwidth;
                    float newheight = containerheight;
                    //Debug.Log("2: "+ newwidth + "x"+newheight);
                    imt.sizeDelta = new Vector2(newwidth, newheight);
                }

                Vector2 size2 = imt.sizeDelta;



                float xminus = (size[0] - size2[0]) / 2;
                float yminus = ((size2[1] - size[1]) / 2);
                //Debug.LogError("yminus = " + yminus + " old = " + size[1] + " new: " + size2[1]);
                //Debug.LogError(imt.anchoredPosition);
                imt.anchoredPosition += new Vector2(xminus, yminus);
                //Debug.Log(imt.anchoredPosition);

            }
            else if (fill == ScaleMethods.FILL_VERTICAL)
            {
                //Debug.LogError("komt ie");
                //Debug.LogError(sprite);
                //Debug.LogError(filename);

                RectTransform imt = img.GetComponent<RectTransform>();
                Vector2 size = imt.sizeDelta;
                //Debug.Log(size);
                float containerwidth = size[0];
                float containerheight = size[1];
                var factor1 = containerwidth / containerheight;
                var factor2 = iwidth / iheight;

                float newwidth = (containerheight / iheight) * iwidth;
                float newheight = containerheight;
                imt.sizeDelta = new Vector2(newwidth, newheight);


                Vector2 size2 = imt.sizeDelta;



                float xminus = (size[0] - size2[0]) / 2;
                float yminus = ((size2[1] - size[1]) / 2);
                //Debug.LogError("yminus = " + yminus + " old = " + size[1] + " new: " + size2[1]);
                //Debug.LogError(imt.anchoredPosition);
                //imt.anchoredPosition += new Vector2(xminus, yminus);
                //Debug.Log(imt.anchoredPosition);

            }

        }
        else {
//            Debug.LogError("COULD NOT FIND "+filename);
        }
    }

    public static void alpha(this Image img, float alpha)
    {
        Color c = img.color.alpha(alpha);
        img.color = c;
    }

    public static void alpha(this RawImage img, float alpha)
    {
        Color c = img.color.alpha(alpha);
        img.color = c;
    }

    public static float height(this Image img,float height = -1){
        RectTransform imt = img.GetComponent<RectTransform>();
        if (height != -1)
        {
            imt.sizeDelta = new Vector2(imt.sizeDelta[0], height);
        }
		return img.gameObject.GetComponent<RectTransform>().rect.height * imt.localScale[1];
    }

    public static float width(this Image img, float width = -1)
    {
        RectTransform imt = img.GetComponent<RectTransform>();
        if (width != -1)
        {
            imt.sizeDelta = new Vector2(width, imt.sizeDelta[0]);
        }
		return img.gameObject.GetComponent<RectTransform>().rect.width * imt.localScale[0];
    }

    public static Vector2 size(this Image img, float width = -1, float height = -1)
    {
        RectTransform imt = img.GetComponent<RectTransform>();
        if (width != -1 && height != -1)
        {
            imt.sizeDelta = new Vector2(width, height);
        }
		return imt.sizeDelta * imt.localScale;
    }

    public static Vector2 size(this RawImage img, float width = -1, float height = -1)
    {
        RectTransform imt = img.GetComponent<RectTransform>();
        if (width != -1 && height != -1)
        {
            imt.sizeDelta = new Vector2(width, height);
        }
        return imt.sizeDelta * imt.localScale;
    }

    public static Vector2 size(this GameObject img, float width = -1, float height = -1)
    {
        RectTransform imt = img.GetComponent<RectTransform>();
        if (width != -1 && height != -1)
        {
            imt.sizeDelta = new Vector2(width, height);
        }
        return imt.sizeDelta * imt.localScale;
    }

    public static float x(this Image img, float? x = null)
	{

		if (x != null)
		{
			img.gameObject.x((float)x);
		}
		return img.gameObject.x();
	}

	public static float y(this Image img, float? y = null)
	{

		if (y != null)
		{
			img.gameObject.y((float)y);
		}
		return img.gameObject.y();
	}

}