using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageFitter : MonoBehaviour {
    public enum ScaleMethod { fit, fill };
    public ScaleMethod scaleMethod = ScaleMethod.fill;
    private Image _image;
    private RawImage _rawImage;
    private Rect _parentRect;
    private Sprite lastSprite;
    public bool Fullscreen = false;
    private Canvas _parentCanvas;
    private float _canvasWidth = 0;
    private float _canvasHeight = 0;
    public int ForceWidth = 0;
    private float containerWidth;
    private float containerHeight;
    private Tween _tween1;
    private Tween _tween2;
    private bool _started = false;

    private float _height = 0;
    private float _width;


    public enum Positions {
        TOP_LEFT,
        TOP_CENTER,
        TOP_RIGHT,
        MIDDLE_LEFT,
        MIDDLE_CENTER,
        MIDDLE_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_CENTER,
        BOTTOM_RIGHT,
        NONE,
    }


    public float Height {
        get {
            return _height;
        }
    }

    public float Width
    {
        get
        {
            return _width;
        }
    }

    private void GetObjects() {
		if (_image == null && _rawImage == null) {
			_image = GetComponent<Image>();
            _rawImage = GetComponent<RawImage>();
            if (Fullscreen) {

                if (ForceWidth != 0)
                {
                    _canvasWidth = ForceWidth;
                    _canvasHeight = (_canvasWidth/ Screen.width) *Screen.height;
                }
                else
                {
                    _parentCanvas = GetComponentInParent<Canvas>();
                    _canvasWidth = Screen.width / _parentCanvas.scaleFactor;
                    _canvasHeight = Screen.height / _parentCanvas.scaleFactor;
                }
//                Debug.LogError(_canvasWidth + "x" + _canvasHeight);

            }
        }
	}


	void Start() {
		GetObjects();
        if (_started == false)
        {
            Go();
        }

	}

    private void KillTweens()
    {
        if (_tween1 != null) {
            _tween1.Kill();
            _tween1 = null;
        }

        if (_tween2 != null)
        {
            _tween2.Kill();
            _tween2 = null;
        }

    }

    public void Go(Positions position = Positions.MIDDLE_CENTER, float zoom = 1, float animateSpeed = 0, Ease ease = Ease.Linear)
    {
		GetObjects();
        KillTweens();
        Texture texture = null;
        Sprite sprite = null;
        _started = true;

        if (_image != null)
        {
            sprite = _image.sprite;
        }
        else if (_rawImage != null) {
            texture = _rawImage.texture;
        }

		if (texture || sprite) {
            containerWidth = 0;
            containerHeight = 0;


            if (Fullscreen)
            {
                containerWidth = _canvasWidth;
                containerHeight = _canvasHeight;
            }
            else
            {
                _parentRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect;
                containerWidth = _parentRect.width;
                containerHeight = _parentRect.height;
            }

            float iwidth = 0;
            float iheight = 0;

            if (texture == null)
            {
                iwidth = sprite.rect.width;
                iheight = sprite.rect.height;
            }
            else {
                iwidth = texture.width;
                iheight = texture.height;
            }

            
            RectTransform imt = new RectTransform();

            if (_image != null)
            {
                imt = _image.GetComponent<RectTransform>();
            }
            else if (_rawImage != null) {
                imt = _rawImage.GetComponent<RectTransform>();
            }


            Vector2 s = getSize(iwidth,iheight,containerWidth,containerHeight, this.scaleMethod,zoom);

            _width = s[0];
            _height = s[1];

            if (animateSpeed > 0)
            {
                _tween1 = imt.DOSizeDelta(new Vector2(s[0], s[1]), animateSpeed).SetEase(ease);
            }
            else
            {
                imt.sizeDelta = new Vector2(s[0], s[1]);
            }
                        
            if (animateSpeed > 0)
            {
                _tween2 = imt.DOAnchorPos(GetPosition(position, zoom),animateSpeed).SetEase(ease);
            }
            else {
                imt.anchoredPosition = GetPosition(position, zoom);
            }
        }
    }

    public static Vector2 getSize(float width, float height, float containerWidth, float containerHeight, ScaleMethod method = ScaleMethod.fill, float zoom = 1) {
        Vector2 result = new Vector2();

        float newwidth = 0;
        float newheight = 0;

        if (method == ScaleMethod.fill)
        {
            var factor1 = containerWidth / containerHeight;
            var factor2 = width / height;
            if (factor1 > factor2)
            {
                newwidth = (containerWidth) * zoom;
                newheight = ((containerWidth / width) * height) * zoom;
            }
            else
            {
                newwidth = ((containerHeight / height) * width) * zoom;
                newheight = (containerHeight) * zoom;
            }
            result[0] = newwidth;
            result[1] = newheight;
        }

        return result;
    }

    private float GetRight()
    {
        return 0 - ((Width/2)-(containerWidth/2));

    }

    private float GetLeft()
    {
        return ((Width / 2) - (containerWidth / 2));

    }

    private float GetTop()
    {
        return  0-((Height / 2) - (containerHeight / 2));

    }


    private float GetBottom()
    {
        return ((Height / 2) - (containerHeight / 2));

    }


    public Vector2 GetPosition(Positions position,float zoom = 1)
    {
        Vector2 p = new Vector2(0, 0);

        switch (position)
        {
            case Positions.TOP_LEFT:
                p.x = GetLeft();
                p.y = GetTop();
                break;
            case Positions.TOP_CENTER:
                p.y = GetTop();
                break;
            case Positions.TOP_RIGHT:
                p.x = GetRight();
                p.y = GetTop();
                break;
            case Positions.MIDDLE_LEFT:
                p.x = GetLeft();

                break;
            case Positions.MIDDLE_CENTER:
                break;
            case Positions.MIDDLE_RIGHT:
                p.x = GetRight();
                break;
            case Positions.BOTTOM_LEFT:
                p.x = GetLeft();
                p.y = GetBottom();
                break;
            case Positions.BOTTOM_CENTER:
                p.y = GetBottom();
                break;
            case Positions.BOTTOM_RIGHT:
                p.x = GetRight();
                p.y = GetBottom();
                break;
        }
        return p;
    }

    private void OnDestroy()
    {
        KillTweens();
    }
}