
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


namespace RektTransform
{
    public static class Anchors
    {
     
        public static readonly MinMax TopLeft = new MinMax(0, 1, 0, 1);
        public static readonly MinMax TopCenter = new MinMax(0.5f, 1, 0.5f, 1);
        public static readonly MinMax TopRight = new MinMax(1, 1, 1, 1);
        public static readonly MinMax TopStretch = new MinMax(0, 1, 1, 1);

        public static readonly MinMax MiddleLeft = new MinMax(0, 0.5f, 0, 0.5f);
        public static readonly MinMax TrueCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);
        public static readonly MinMax MiddleCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);
        public static readonly MinMax MiddleRight = new MinMax(1, 0.5f, 1, 0.5f);
        public static readonly MinMax MiddleStretch = new MinMax(0, 0.5f, 1, 0.5f);

        public static readonly MinMax BottomLeft = new MinMax(0, 0, 0, 0);
        public static readonly MinMax BottomCenter = new MinMax(0.5f, 0, 0.5f, 0);
        public static readonly MinMax BottomRight = new MinMax(1, 0, 1, 0);
        public static readonly MinMax BottomStretch = new MinMax(0, 0, 1, 0);

        public static readonly MinMax StretchLeft = new MinMax(0, 0, 0, 1);
        public static readonly MinMax StretchCenter = new MinMax(0.5f, 0, 0.5f, 1);
        public static readonly MinMax StretchRight = new MinMax(1, 0, 1, 1);
        public static readonly MinMax TrueStretch = new MinMax(0, 0, 1, 1);
        public static readonly MinMax StretchStretch = new MinMax(0, 0, 1, 1);
    }

    public struct MinMax
    {
        public Vector2 min;
        public Vector2 max;

        public MinMax(Vector2 min, Vector2 max)
        {
            this.min = new Vector2(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y));
            this.max = new Vector2(Mathf.Clamp01(max.x), Mathf.Clamp01(max.y));
        }

        public MinMax(float minx, float miny, float maxx, float maxy)
        {
            this.min = new Vector2(Mathf.Clamp01(minx), Mathf.Clamp01(miny));
            this.max = new Vector2(Mathf.Clamp01(maxx), Mathf.Clamp01(maxy));
        }
    }


    public static class Cast
    {
        public static RectTransform RT(this GameObject go)
        {
            if (go == null || go.transform == null)
                return null;

            return go.GetComponent<RectTransform>();
        }

        public static RectTransform RT(this Transform t)
        {
            if (t is RectTransform == false)
                return null;

            return t as RectTransform;
        }

        public static RectTransform RT(this Component c)
        {
            return RT(c.transform);
        }

        public static RectTransform RT(this UIBehaviour ui)
        {
            if (ui == null)
                return null;

            return ui.transform as RectTransform;
        }
    }

    public static class RectTransformExtension
    {
        public static Rect GetWorldRect(this RectTransform RT)
        {
         
            Vector3[] corners = new Vector3[4];
            RT.GetWorldCorners(corners);
            Vector2 Size = new Vector2(corners[2].x - corners[1].x, corners[1].y - corners[0].y);
            return new Rect(new Vector2(corners[1].x, -corners[1].y), Size);
        }

        public static MinMax GetAnchors(this RectTransform RT)
        {
       
            return new MinMax(RT.anchorMin, RT.anchorMax);
        }

      
        public static void SetAnchors(this RectTransform RT, MinMax anchors)
        {
        
            RT.anchorMin = anchors.min;
            RT.anchorMax = anchors.max;
        }

     
        public static RectTransform GetParent(this RectTransform RT)
        {
         
            return RT.parent as RectTransform;
        }

    
        public static float GetWidth(this RectTransform RT)
        {
        
            return RT.rect.width;
        }
        public static float GetHeight(this RectTransform RT)
        {
         
            return RT.rect.height;
        }

        public static Vector2 GetSize(this RectTransform RT)
        {
         
            return new Vector2(RT.GetWidth(), RT.GetHeight());
        }

        public static void SetWidth(this RectTransform RT, float width)
        {
         

            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public static void SetHeight(this RectTransform RT, float height)
        {
          

            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static void SetSize(this RectTransform RT, float width, float height)
        {


            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static void SetSize(this RectTransform RT, Vector2 size)
        {

            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }

    
        public static Vector2 GetLeft(this RectTransform RT)
        {

            return new Vector2(RT.offsetMin.x, RT.anchoredPosition.y);
        }

        public static Vector2 GetRight(this RectTransform RT)
        {
      

            return new Vector2(RT.offsetMax.x, RT.anchoredPosition.y);
        }

        public static Vector2 GetTop(this RectTransform RT)
        {

            return new Vector2(RT.anchoredPosition.x, RT.offsetMax.y);
        }

        public static Vector2 GetBottom(this RectTransform RT)
        {

            return new Vector2(RT.anchoredPosition.x, RT.offsetMin.y);
        }

       

        public static void SetLeft(this RectTransform RT, float left)
        {

            float xmin = RT.GetParent().rect.xMin;
            float anchorFactor = RT.anchorMin.x * 2 - 1;

            RT.offsetMin = new Vector2(xmin + (xmin * anchorFactor) + left, RT.offsetMin.y);
        }

        public static void SetRight(this RectTransform RT, float right)
        {

            float xmax = RT.GetParent().rect.xMax;
            float anchorFactor = RT.anchorMax.x * 2 - 1;

            RT.offsetMax = new Vector2(xmax - (xmax * anchorFactor) + right, RT.offsetMax.y);
        }

        public static void SetTop(this RectTransform RT, float top)
        {

            float ymax = RT.GetParent().rect.yMax;
            float anchorFactor = RT.anchorMax.y * 2 - 1;

            RT.offsetMax = new Vector2(RT.offsetMax.x, ymax - (ymax * anchorFactor) + top);
        }

        public static void SetBottom(this RectTransform RT, float bottom)
        {

            float ymin = RT.GetParent().rect.yMin;
            float anchorFactor = RT.anchorMin.y * 2 - 1;

            RT.offsetMin = new Vector2(RT.offsetMin.x, ymin + (ymin * anchorFactor) + bottom);
        }

        public static void Left(this RectTransform RT, float left)
        {
            RT.SetLeft(left);
        }

        public static void Right(this RectTransform RT, float right)
        {
            RT.SetRight(-right);
        }

        public static void Top(this RectTransform RT, float top)
        {
            RT.SetTop(-top);
        }

        public static void Bottom(this RectTransform RT, float bottom)
        {
            RT.SetRight(bottom);
        }

        public static void SetLeftFrom(this RectTransform RT, MinMax anchor, float left)
        {
           

            Vector2 origin = RT.AnchorToParentSpace(anchor.min - RT.anchorMin);

            RT.offsetMin = new Vector2(origin.x + left, RT.offsetMin.y);
        }

        public static void SetRightFrom(this RectTransform RT, MinMax anchor, float right)
        {
         

            Vector2 origin = RT.AnchorToParentSpace(anchor.max - RT.anchorMax);

            RT.offsetMax = new Vector2(origin.x + right, RT.offsetMax.y);
        }

        public static void SetTopFrom(this RectTransform RT, MinMax anchor, float top)
        {
            

            Vector2 origin = RT.AnchorToParentSpace(anchor.max - RT.anchorMax);

            RT.offsetMax = new Vector2(RT.offsetMax.x, origin.y + top);
        }

        public static void SetBottomFrom(this RectTransform RT, MinMax anchor, float bottom)
        {
     

            Vector2 origin = RT.AnchorToParentSpace(anchor.min - RT.anchorMin);

            RT.offsetMin = new Vector2(RT.offsetMin.x, origin.y + bottom);
        }

      

        public static void SetRelativeLeft(this RectTransform RT, float left)
        {
           

            RT.offsetMin = new Vector2(RT.anchoredPosition.x + left, RT.offsetMin.y);
        }

        public static void SetRelativeRight(this RectTransform RT, float right)
        {

            RT.offsetMax = new Vector2(RT.anchoredPosition.x + right, RT.offsetMax.y);
        }

        public static void SetRelativeTop(this RectTransform RT, float top)
        {

            RT.offsetMax = new Vector2(RT.offsetMax.x, RT.anchoredPosition.y + top);
        }

        public static void SetRelativeBottom(this RectTransform RT, float bottom)
        {

            RT.offsetMin = new Vector2(RT.offsetMin.x, RT.anchoredPosition.y + bottom);
        }

       

        public static void MoveLeft(this RectTransform RT, float left = 0)
        {

            float xmin = RT.GetParent().rect.xMin;
            float center = RT.anchorMax.x - RT.anchorMin.x;
            float anchorFactor = RT.anchorMax.x * 2 - 1;

            RT.anchoredPosition = new Vector2(xmin + (xmin * anchorFactor) + left - (center * xmin), RT.anchoredPosition.y);
        }

        public static void MoveRight(this RectTransform RT, float right = 0)
        {

            float xmax = RT.GetParent().rect.xMax;
            float center = RT.anchorMax.x - RT.anchorMin.x;
            float anchorFactor = RT.anchorMax.x * 2 - 1;

            RT.anchoredPosition = new Vector2(xmax - (xmax * anchorFactor) - right + (center * xmax), RT.anchoredPosition.y);
        }

        public static void MoveTop(this RectTransform RT, float top = 0)
        {
            Log("MoveTop called on " + RT + ".");

            float ymax = RT.GetParent().rect.yMax;
            float center = RT.anchorMax.y - RT.anchorMin.y;
            float anchorFactor = RT.anchorMax.y * 2 - 1;

            RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, ymax - (ymax * anchorFactor) - top + (center * ymax));
        }

        public static void MoveBottom(this RectTransform RT, float bottom = 0)
        {


            float ymin = RT.GetParent().rect.yMin;
            float center = RT.anchorMax.y - RT.anchorMin.y;
            float anchorFactor = RT.anchorMax.y * 2 - 1;

            RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, ymin + (ymin * anchorFactor) + bottom - (center * ymin));
        }


        public static void MoveLeftInside(this RectTransform RT, float left = 0)
        {
 
            RT.MoveLeft(left + RT.GetWidth() / 2);
        }

        public static void MoveRightInside(this RectTransform RT, float right = 0)
        {
 
            RT.MoveRight(right + RT.GetWidth() / 2);
        }

        public static void MoveTopInside(this RectTransform RT, float top = 0)
        {
 
            RT.MoveTop(top + RT.GetHeight() / 2);
        }

        public static void MoveBottomInside(this RectTransform RT, float bottom = 0)
        {
  
            RT.MoveBottom(bottom + RT.GetHeight() / 2);
        }

        public static void MoveLeftOutside(this RectTransform RT, float left = 0)
        {
  
            RT.MoveLeft(left - RT.GetWidth() / 2);
        }

        public static void MoveRightOutside(this RectTransform RT, float right = 0)
        {
    
            RT.MoveRight(right - RT.GetWidth() / 2);
        }

        public static void MoveTopOutside(this RectTransform RT, float top = 0)
        {
     
            RT.MoveTop(top - RT.GetHeight() / 2);
        }

        public static void MoveBottomOutside(this RectTransform RT, float bottom = 0)
        {
      
            RT.MoveBottom(bottom - RT.GetHeight() / 2);
        }



        public static void Move(this RectTransform RT, float x, float y)
        {
            RT.MoveLeft(x);
            RT.MoveBottom(y);
        }

        public static void Move(this RectTransform RT, Vector2 point)
        {
            RT.MoveLeft(point.x);
            RT.MoveBottom(point.y);
        }

        public static void MoveInside(this RectTransform RT, float x, float y)
        {
            RT.MoveLeftInside(x);
            RT.MoveBottomInside(y);
        }

        public static void MoveInside(this RectTransform RT, Vector2 point)
        {
            RT.MoveLeftInside(point.x);
            RT.MoveBottomInside(point.y);
        }

   
        public static void MoveOutside(this RectTransform RT, float x, float y)
        {
            RT.MoveLeftOutside(x);
            RT.MoveBottomOutside(y);
        }

        public static void MoveOutside(this RectTransform RT, Vector2 point)
        {
            RT.MoveLeftOutside(point.x);
            RT.MoveBottomOutside(point.y);
        }

     
        public static void MoveFrom(this RectTransform RT, MinMax anchor, Vector2 point)
        {
            RT.MoveFrom(anchor, point.x, point.y);
        }

        public static void MoveFrom(this RectTransform RT, MinMax anchor, float x, float y)
        {
           Vector2 origin = RT.AnchorToParentSpace(AnchorOrigin(anchor) - RT.AnchorOrigin());
            RT.anchoredPosition = new Vector2(origin.x + x, origin.y + y);
        }

       

        public static Vector2 ParentToChildSpace(this RectTransform RT, Vector2 point)
        {
            return RT.ParentToChildSpace(point.x, point.y);
        }

        public static Vector2 ParentToChildSpace(this RectTransform RT, float x, float y)
        {
            float xmin = RT.GetParent().rect.xMin;
            float ymin = RT.GetParent().rect.yMin;
            float anchorFactorX = RT.anchorMin.x * 2 - 1;
            float anchorFactorY = RT.anchorMin.y * 2 - 1;

            return new Vector2(xmin + (xmin * anchorFactorX) + x, ymin + (ymin * anchorFactorY) + y);
        }



        public static Vector2 ChildToParentSpace(this RectTransform RT, float x, float y)
        {
            return RT.AnchorOriginParent() + new Vector2(x, y);
        }

        public static Vector2 ChildToParentSpace(this RectTransform RT, Vector2 point)
        {
            return RT.AnchorOriginParent() + point;
        }



        public static Vector2 ParentToAnchorSpace(this RectTransform RT, Vector2 point)
        {
            return RT.ParentToAnchorSpace(point.x, point.y);
        }

        public static Vector2 ParentToAnchorSpace(this RectTransform RT, float x, float y)
        {
            Rect parent = RT.GetParent().rect;
            if (parent.width != 0)
                x /= parent.width;
            else
                x = 0;

            if (parent.height != 0)
                y /= parent.height;
            else
                y = 0;

            return new Vector2(x, y);
        }

     

        public static Vector2 AnchorToParentSpace(this RectTransform RT, float x, float y)
        {
            return new Vector2(x * RT.GetParent().rect.width, y * RT.GetParent().rect.height);
        }

        public static Vector2 AnchorToParentSpace(this RectTransform RT, Vector2 point)
        {
            return new Vector2(point.x * RT.GetParent().rect.width, point.y * RT.GetParent().rect.height);
        }


        public static Vector2 AnchorOrigin(this RectTransform RT)
        {
            return AnchorOrigin(RT.GetAnchors());
        }

        public static Vector2 AnchorOrigin(MinMax anchor)
        {
            float x = anchor.min.x + (anchor.max.x - anchor.min.x) / 2;
            float y = anchor.min.y + (anchor.max.y - anchor.min.y) / 2;

            return new Vector2(x, y);
        }

 
        public static Vector2 AnchorOriginParent(this RectTransform RT)
        {
            return Vector2.Scale(RT.AnchorOrigin(), new Vector2(RT.GetParent().rect.width, RT.GetParent().rect.height));
        }

        public static Canvas GetRootCanvas(this RectTransform RT)
        {
            Canvas rootCanvas = RT.GetComponentInParent<Canvas>();

            while (!rootCanvas.isRootCanvas)
                rootCanvas = rootCanvas.transform.parent.GetComponentInParent<Canvas>();

            return rootCanvas;
        }
    }
}

