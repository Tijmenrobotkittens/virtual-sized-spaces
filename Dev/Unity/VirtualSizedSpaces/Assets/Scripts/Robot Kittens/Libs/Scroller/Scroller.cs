using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class SnapEvent : UnityEvent<int> { }
public class SnapPercent : UnityEvent<List<float>> { }
public class MoveEvent : UnityEvent<float, float> { }
public class PercentEvent : UnityEvent<float, float> { }
public class ScrollEvent : UnityEvent<string, string> { }


namespace RobotKittens
{

    public class XYID
    {

        public float x;
        public float y;
        public int id;

        public XYID(float _x, float _y, int _id)
        {
            x = _x;
            y = _y;
            id = _id;
        }
    }


    public class Scroller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject scrollTarget;
        private bool _dragging = false;
        public bool horizontal = true;
        public bool vertical = true;
        public bool smoothDividing = true;
        public float divideBy = 1.1f;
        public float stretchDistance = 100;
        public float snapSpeed = 0.15f;
        private float lastX = 0;
        private float lastY = 0;
        private float Xspeed = 0;
        private float Yspeed = 0;
        private bool _animating = false;
        private int _overstretchX = 0;
        private int _overstretchY = 0;
        private bool _finishdragging = false;
        private List<XYID> snapPoints = new List<XYID>();
        public float x = 0;
        public float y = 0;
        public string scrollerName = "";

        public SnapPercent snapPercentEvent = new SnapPercent();
        public SnapEvent snapped = new SnapEvent();
        public UnityEvent scrolled;
        public MoveEvent moved = new MoveEvent();
        public PercentEvent percentChanged = new PercentEvent();
        public ScrollEvent scrollEvent = new ScrollEvent();
        public int currentSnap = 0;
        private bool enabled = true;
        private Tween tween1;
        private Tween tween2;
        public bool livesnap = false;
        public bool livesnapinvert = false;
        private RectTransform _scrollTargetRect;
        private float _canvasWidth = 0;
        private float _canvasHeight = 0;
        public Scroller parentScroller;
        private Canvas _canvas;
        private float _scale = 1;
        private RectTransform _rect;
        private string lockedaxis = "";
        private float scrollDiffBeforeLock = 10;
        private float hdistance = 0;
        private float vdistance = 0;
        public bool LockAxisAfterMove = false;
        public bool MoveEventEnabled = false;
        public bool PercentEventEnabled = false;
        public bool UpdateSizeBasedOnChildRect = false;

        public float XVelocity = 0;
        public float YVelocity = 0;

        public float minX = 0;
        public float maxX = 0;

        public float minY = 0;
        public float maxY = 0;
        private float permanentScrollUntilLimit = 0;

        private void killTweens()
        {
            if (tween1 != null)
            {
                tween1.Kill();
            }

            if (tween2 != null)
            {
                tween2.Kill();
            }
        }

        void GetCanvas(GameObject g)
        {
            if (g.GetComponent<Canvas>() != null)
            {
                _canvas = g.GetComponent<Canvas>();
                return;
            }
            else
            {
                if (g.transform.parent != null)
                {
                    GetCanvas(g.transform.parent.gameObject);
                }
            }
        }



        private void GetObjects()
        {
            if (!_scrollTargetRect)
            {
                GetCanvas(scrollTarget);
                _canvasWidth = _canvas.GetComponent<CanvasScaler>().referenceResolution.x;
                _scrollTargetRect = scrollTarget.GetComponent<RectTransform>();
                _canvasHeight = HelperFunctions.GetHeight(_canvasWidth);
                _canvas = GetComponentInParent<Canvas>();
                _rect = scrollTarget.GetComponent<RectTransform>();
                _scale = _canvas.scaleFactor;
                //            Debug.LogError("SCALE = "+_scale);
            }
        }

        void Start()
        {
            GetObjects();
            UpdateMove();
        }

        public void SetAutoHeight()
        {
            SetHeight(scrollTarget.gameObject.height());
        }

        public void SetHeight(float height)
        {
            GetObjects();
            float cy = height - _canvasHeight;
            if (cy < 0)
            {
                cy = 0;
            }
            maxY = cy;
        }

        public void ScrollDownUntilMax(float pixels = 1)
        {
            permanentScrollUntilLimit = pixels;
        }

        public void Reset()
        {
            GetObjects();
            killTweens();
            _animating = false;

            _rect.anchoredPosition = new Vector2(0, 0);
        }

        public void ScrollTo(Vector2 pos, float speed = 1)
        {
            killTweens();
            _animating = false;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            //            Debug.LogError(pos);

            tween1 = _scrollTargetRect.DOAnchorPos(pos, speed).SetEase(Ease.OutExpo).OnUpdate(UpdateMove);
            tween1.onComplete += () => scrolled.Invoke();
        }

        public void UpdateMove()
        {
            x = _scrollTargetRect.anchoredPosition.x;
            y = _scrollTargetRect.anchoredPosition.y;
            if (MoveEventEnabled)
            {
                moved.Invoke(_scrollTargetRect.anchoredPosition.x, _scrollTargetRect.anchoredPosition.y);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!enabled) return;
            
            if (_canvas.scaleFactor != _scale)
            {
                _scale = _canvas.scaleFactor;
            }
            lockedaxis = "";
            vdistance = 0;
            hdistance = 0;

            permanentScrollUntilLimit = 0;
            GetObjects();
            _dragging = true;
            killTweens();
            lastX = eventData.position.x;
            lastY = eventData.position.y;
            scrollEvent.Invoke(scrollerName, "begin");
            if (parentScroller)
            {
                parentScroller.OnBeginDrag(eventData);
            }
        }

        public void calcSnapPercent()
        {
            List<float> percents = new List<float>();
            if (livesnap)
            {
                if (_scrollTargetRect == null)
                {
                    return;
                }
                Vector2 currentPos = _scrollTargetRect.anchoredPosition;

                foreach (XYID xYID in snapPoints)
                {
                    float dis = 0;
                    if (horizontal)
                    {
                        dis = xYID.x - (0 - currentPos[0]);

                    }
                    else
                    {
                        dis = xYID.y - currentPos[1];
                    }

                    float distancepercent = 0;

                    if (livesnapinvert)
                    {
                        distancepercent = Mathf.Clamp(((dis / _canvasWidth) * 100), -100, 100);
                    }
                    else
                    {
                        distancepercent = Mathf.Clamp(100 - ((dis / _canvasWidth) * 100), -100, 100);
                    }

                    percents.Add(distancepercent);
                }
                snapPercentEvent.Invoke(percents);
            }

            UpdateMove();

        }

        private void OnDestroy()
        {
            killTweens();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!enabled) return;

            OnDrag2(eventData.position.x, eventData.position.y);
        }

        public void OnDrag2(float x, float y)
        {


            if (lockedaxis != "")
            {
                if (lockedaxis == "h")
                {
                    y = lastY;
                }
                else if (lockedaxis == "v")
                {
                    x = lastX;
                }
            }



            if (parentScroller)
            {
                parentScroller.OnDrag2(x, y);
            }

            if (!enabled)
            {
                return;
            }
            permanentScrollUntilLimit = 0;
            _dragging = true;
            _finishdragging = false;

            float newX = x;
            float newY = y;

            float diffX = (newX - lastX) / _scale;
            float diffY = (newY - lastY) / _scale;
            vdistance += Math.Abs(diffY);
            hdistance += Math.Abs(diffX);


            if (parentScroller != null && LockAxisAfterMove == true && (vdistance > scrollDiffBeforeLock || hdistance > scrollDiffBeforeLock))
            {
                if (vdistance > (scrollDiffBeforeLock / _scale))
                {
                    lockedaxis = "v";
                }
                else if (hdistance > (scrollDiffBeforeLock / _scale))
                {
                    lockedaxis = "h";
                }

            }


            float changeX = 0;
            float changeY = 0;
            if (horizontal)
            {
                changeX = diffX;
                Xspeed = diffX;
            }

            if (vertical)
            {
                Yspeed = diffY;
                changeY = diffY;
            }

            Vector2 currentPos = _rect.anchoredPosition;

            Vector2 result = calcMovement(currentPos, changeX, changeY);

            SetPosition(result[0], result[1]);

            lastX = x;
            lastY = y;
            calcSnapPercent();


        }

        public void SetPercent(float xpercent, float ypercent, bool triggerEvent = true)
        {
            float xpos = 0;
            float ypos = 0;

            if (horizontal)
            {
                xpos = (PercentToRange(maxX, minX, xpercent));
            }

            if (vertical)
            {
                ypos = (PercentToRange(minY, maxY, ypercent));
            }

            SetPosition(xpos, ypos, triggerEvent);
        }

        public void disable()
        {
            enabled = false;
        }

        public void enable()
        {
            enabled = true;
        }

        public int getCurrentSnap()
        {
            return currentSnap;
        }

        public void ClearSnapPoints()
        {
            snapPoints.Clear();
        }

        public void addSnapPoint(float x, float y, int id)
        {
            XYID xYID = new XYID(x, y, id);
            snapPoints.Add(xYID);
        }

        private Vector2 calcMovement(Vector2 currentPos, float changeX, float changeY)
        {
            _overstretchX = 0;
            _overstretchY = 0;

            float finalX = currentPos[0] + changeX;
            float finalY = currentPos[1] + changeY;


            finalX = Mathf.Clamp(finalX, minX - stretchDistance, maxX + stretchDistance);
            finalY = Mathf.Clamp(finalY, minY - stretchDistance, maxY + stretchDistance);

            if (finalX < minX)
            {
                _overstretchX = 1;
                float over = 0 - (finalX - minX);
                float percent = 100 - ((100 / stretchDistance) * over);
                finalX = currentPos[0] + ((changeX / 100) * percent);
            }

            if (finalX > maxX)
            {
                _overstretchX = 2;
                float over = stretchDistance - (finalX - maxX);
                float percent = (100 / stretchDistance) * over;
                finalX = currentPos[0] + ((changeX / 100) * percent);
            }

            if (finalY < minY)
            {
                _overstretchY = 1;
                float over = 0 - (finalY - minY);
                float percent = 100 - ((100 / stretchDistance) * over);
                finalY = currentPos[1] + ((changeY / 100) * percent);
            }

            if (finalY > maxY)
            {
                _overstretchY = 2;
                float over = (stretchDistance - (finalY - maxY));
                float percent = (100 / stretchDistance) * over;
                finalY = currentPos[1] + ((changeY / 100) * percent);
            }

            XVelocity = changeX;
            YVelocity = changeY;
            //Debug.LogError(changeX + ":"+ changeY);

            return new Vector2(finalX, finalY);

        }





        public void OnEndDrag(PointerEventData eventData)
        {
            if (!enabled) return;
            permanentScrollUntilLimit = 0;
            if (smoothDividing)
            {
                _animating = true;
            }
            _finishdragging = true;
            _dragging = false;
            scrollEvent.Invoke(scrollerName, "end");
            if (parentScroller)
            {
                parentScroller.OnEndDrag(eventData);
            }
        }

        private void SetPosition(float x = 0, float y = 0, bool triggerEvent = true)
        {
            this.x = x;
            this.y = y;
            _rect.anchoredPosition = new Vector2(x, y);
            if (MoveEventEnabled && triggerEvent)
            {
                moved.Invoke(x, y);
            }

            if (PercentEventEnabled && triggerEvent)
            {
                float percentx = RangeToPercent(maxX, minX, x);
                float percenty = RangeToPercent(minY, maxY, y);
                percentChanged.Invoke(percentx * 100, percenty * 100);
            }
        }

        private static float RangeToPercent(float from, float to, float value)
        {
            float range = to - from;
            float percent = (value - from) / range;
            return percent;
        }

        private static float PercentToRange(float from, float to, float percent)
        {
            float range = to - from;
            float value = from + ((range / 100) * percent);
            return value;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public void snapNext()
        {
            if (snapPoints.Count - 1 > currentSnap)
            {
                int newsnap = currentSnap + 1;
                snap(newsnap);
                currentSnap = newsnap;
            }

        }

        public void snapPrevious()
        {
            if (currentSnap - 1 >= 0)
            {
                int newsnap = currentSnap - 1;
                snap(newsnap);
                currentSnap = newsnap;
            }
        }

        public Vector2 GetPosition()
        {
            return _rect.anchoredPosition;
        }

        public void snap(int id, bool triggerEvent = true)
        {
            XYID finalXYID = snapPoints[id];
            currentSnap = id;
            tween1 = _scrollTargetRect.DOLocalMove(new Vector2(0 - finalXYID.x, finalXYID.y), snapSpeed).SetEase(Ease.OutExpo).OnUpdate(calcSnapPercent);
            if (triggerEvent)
            {
                tween1.onComplete += () => { snapped.Invoke(id); };
            }
        }

        void Update()
        {
            Vector2 currentPos = _scrollTargetRect.anchoredPosition;


            if (_animating == true && snapPoints.Count > 0)
            {
                int counter = 0;
                double closest = 0;
                int closestKey = 0;
                bool closestSet = false;
                XYID finalXYID = new XYID(0, 0, 0);
                foreach (XYID xYID in snapPoints)
                {
                    var dis = GetDistance(0 - currentPos[0], currentPos[1], xYID.x, xYID.y);
                    if (counter == currentSnap)
                    {
                        dis = dis * 4;
                    }
                    if (dis < closest || closestSet == false)
                    {
                        closestKey = counter;
                        closest = dis;
                        closestSet = true;
                        finalXYID = xYID;
                    }
                    counter++;
                }

                snap(closestKey);

                _animating = false;
            }
            if (_overstretchX != 0 && _finishdragging == true)
            {
                _finishdragging = false;
                float target = minX;
                if (_overstretchX == 2)
                {
                    target = maxX;
                }

                tween1 = _rect.DOLocalMoveX(target, snapSpeed).SetEase(Ease.OutExpo).OnUpdate(calcSnapPercent);

                _animating = false;
            }
            else if (_overstretchY != 0 && _finishdragging == true)
            {
                _finishdragging = false;
                float target = minY;
                if (_overstretchY == 2)
                {
                    target = maxY;
                }

                tween1 = _rect.DOLocalMoveY(target, snapSpeed).SetEase(Ease.OutExpo).OnUpdate(calcSnapPercent);

                _animating = false;
            }
            if (_animating)
            {


                float newX = currentPos[0] + Xspeed;
                float newY = currentPos[1] + Yspeed;

                newX = Mathf.Clamp(newX, minX, maxX);
                newY = Mathf.Clamp(newY, minY, maxY);


                SetPosition(newX, newY);
                Xspeed = Xspeed / divideBy;
                Yspeed = Yspeed / divideBy;
                if (Mathf.Abs(Yspeed) < 0.01 && Mathf.Abs(Xspeed) < 0.01)
                {
                    _animating = false;
                }

            }
            else if (permanentScrollUntilLimit != 0)
            {

                float newY = currentPos[1] + permanentScrollUntilLimit;

                newY = Mathf.Clamp(newY, minY, maxY);
                if (newY == maxY)
                {
                    permanentScrollUntilLimit = 0;
                }

                SetPosition(scrollTarget.GetComponent<RectTransform>().anchoredPosition[0], newY);
                Yspeed = Yspeed / divideBy;

            }
        }
    }
}