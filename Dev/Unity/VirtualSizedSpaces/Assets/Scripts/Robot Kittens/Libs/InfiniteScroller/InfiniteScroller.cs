using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RobotKittens;
public class InfiniteScrollerEvent : UnityEvent<int> { }
public class InfiniteScroller : MonoBehaviour
{
    private bool _innited = false;
    private string _prefab;
    private float _spacing;
	public enum directions {Horizontal,Vertical}
	public enum builddirections { Forwards,  Backwards }
    private ObjectPool _objectPool = new ObjectPool();
    private int _cachenumber;
	private int _minkey = 0;
    private int _maxkey = 0;
	private float _startpos = 0;
    private float _lastMin = -1;
    private float _lastMax = -1;
    private Action<int, GameObject> _callback;

    private RectTransform _rectTransform;
    private float _height;
	private float _width;
    public Scroller _scroller;
	public directions direction = directions.Vertical;
	public builddirections buildDirection = builddirections.Forwards;
    public float padding = 0;
    public float hiddenPadding = 0;
    private float? _lastval = null;
	private bool started = false;
	private bool breaknext = false;
    private bool refreshOnNextTick = false;
    public UnityEvent reorder = new UnityEvent();
    private float? forceCurrentMin = 0;
    private int lastCount = -1;
    private bool _reuseItems = false;
    public InfiniteScrollerEvent clicked = new InfiniteScrollerEvent();
    private int _nrOfItems = 0;
    private bool _destroyed = false;

    private void Start() {
		_width = _scroller.GetComponent<RectTransform>().rect.width + padding;
		_height = _scroller.GetComponent<RectTransform>().rect.height + padding;
	}

    public void UpdateSize() {
        _width = _scroller.GetComponent<RectTransform>().rect.width + padding;
        _height = _scroller.GetComponent<RectTransform>().rect.height + padding;
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateSize();
    }

	public void setStartPos(float startpos) {
        _startpos = startpos;
    }

    public void SetNrOfItems(int nr)
    {
        UpdateSize();
        _nrOfItems = nr;
    }

    public GameObject GetSelected(Vector2 selectedPosition)
    {
	    List<GameObject> objs = GetPoolObjects();
	    int closestIndex = 0;
	    float closest = float.PositiveInfinity;
	    float distance;
	    for (int i = 0; i < objs.Count; i++)
	    {
		    if (objs[i].activeSelf)
		    {
			    RectTransform objRect = objs[i].GetComponent<RectTransform>();
			    distance = Vector2.Distance(selectedPosition, objRect.anchoredPosition + _scroller.GetPosition());
			    if (distance < closest)
			    {
				    closest = distance;
				    closestIndex = i;
			    }
		    }
	    }
	    return objs[closestIndex];
    }

    public void SetKey(int key)
    {
        RKLog.LogError("Set Key " + key, "infinitescroller");
        Rebuild(key);

        
    }

	public void init(string prefab, float spacing, int nrOfItems, Action<int,GameObject> callback, float startpos = 0, int cachenumber = 20, bool reuseitems = false)
    {
        //Debug.Log("init infinitescroller");
        _reuseItems = reuseitems;
		_startpos = startpos;
        _prefab = prefab;
        _spacing = spacing;
        _nrOfItems = nrOfItems;
        _cachenumber = cachenumber;
        _callback = callback;

        if (_reuseItems)
        {
            _objectPool.reuseID = true;
        }

        createCache();
		_rectTransform = gameObject.GetComponent<RectTransform>();
        _innited = true;

        if (nrOfItems > 0 || nrOfItems == -1)
        {
            Rebuild();
        }
    }

	public ObjectPool objectPool(){
		return _objectPool;
	}

    public void Rebuild(int? key = null){
        if (_nrOfItems == 0) {
            return;
        }
		_scroller.Reset();
        _objectPool.deactivateAll();
        _scroller.minY = 0 - float.MaxValue;
        _scroller.maxY = float.MaxValue;
        _scroller.minX = 0 - float.MaxValue;
        _scroller.maxX = float.MaxValue;
        //_objectPool.deactivateAll();

        if (key != null) {
            _minkey = (int)key;
            _maxkey = (int)key-1;
        }


		else if (buildDirection == builddirections.Backwards)
		{
			_minkey = _nrOfItems;
			_maxkey = _nrOfItems;
		}
		else
		{
			_minkey = 0;
			_maxkey = -1;
		}

        if (_nrOfItems == 0) {
            _scroller.minY = 0;
            _scroller.maxY = 0;
            _scroller.minX = 0;
            _scroller.maxX = 0;
        }

       // Debug.LogError("rebuild "+_startpos);
	}

    public void Refresh()
    {
        UpdateSize();
        refreshOnNextTick = true;
    }

    private InfiniteScrollerCurrentStateData RefreshBeforeBuild(InfiniteScrollerCurrentStateData cd){
//        Debug.LogError("refreshbeforebuild");
//        Debug.LogError(cd.debug());
        if (_innited)
        {
            _objectPool.deactivateAll();
            cd.maxkey = cd.minkey -1;

            float plus = _spacing;

            //if (cd.minkey == 0 && direction == directions.Vertical) {
            //    plus =  0-plus;
            //}


            cd.currentmax = cd.currentmin + swapvar(plus);


        }
        UpdateSize();
        //breaknext = true;
        return cd;
    }

	public void Go(){
        UpdateSize();
		started = true;
	}

	private float swapvar(float val) {
		if (direction == directions.Vertical) {
			return 0 - val;
		}
		else {
			return val;
		}
	}

    private float GetPadding() {
        if (hiddenPadding != 0) {
            return hiddenPadding;
        }
        if (padding != 0) {
            return padding;
        }
        return 0;

    }

    private InfiniteScrollerCurrentStateData clearInvisibleAndGetData(float offset, float totalvalue){
       
        List<GameObject> removes = new List<GameObject>();
        float currentmin = 0;
        float currentmax = 0;
        bool _minset = false;
        bool _maxset = false;
        foreach (GameObject go in _objectPool.getActive())
        {
            RectTransform r = go.GetComponent<RectTransform>();
            Vector2 p = r.anchoredPosition;
            IInfiniteScrollPrefab prefabInterface = go.GetInterface<IInfiniteScrollPrefab>();


            float ppos = p.y;

            float sizedeltapos = r.sizeDelta.y;
            float anchor = r.anchoredPosition.y;

            if (direction == directions.Horizontal)
            {
                ppos = p.x;
                sizedeltapos = r.sizeDelta.x;
            }

            float abspos = ppos + offset;
           
            if (swapvar(abspos) > totalvalue)
            {

                removes.Add(go);
            }
            else if (swapvar(abspos + swapvar(sizedeltapos)) < 0- GetPadding())
            {
               removes.Add(go);

            }
            else
            {
                if (swapvar(ppos) < swapvar(currentmin) || _minset == false)
                {
                    currentmin = ppos + swapvar(0 - _spacing);
                    _minset = true;
                    _minkey = Int32.Parse(go.name);

                }


                if (swapvar(ppos + swapvar(sizedeltapos)) > swapvar(currentmax) || _maxset == false)
                {
                    currentmax = (ppos + swapvar(sizedeltapos)) + swapvar(_spacing);
                    _maxkey = Int32.Parse(go.name);
                    _maxset = true;
                }


            }

        }

        InfiniteScrollerCurrentStateData ret = new InfiniteScrollerCurrentStateData();

        foreach (GameObject go in removes)
        {
            _objectPool.deactivate(go);
        }

        ret.maxkey = _maxkey;
        ret.minkey = _minkey;
        ret.currentmax = currentmax;
        ret.currentmin = currentmin;
        ret.minset = _minset;
        ret.maxset = _maxset;
        return ret;

    }

    private void fill()
    {
        if (!_rectTransform) {
            return;
        }

       // Debug.LogError(_data.length());

		//Debug.LogError("----------------------------------------------------------------------");
		float xoffset = _rectTransform.anchoredPosition.x;
        float yoffset = _rectTransform.anchoredPosition.y;
       // Debug.LogError("yoffset"+yoffset);
             
        		
		float offset = yoffset;
		float totalvalue = _height +hiddenPadding;
		if (direction == directions.Horizontal) {
			offset = xoffset;
			totalvalue = _width;
		}



        InfiniteScrollerCurrentStateData ret = clearInvisibleAndGetData(offset, totalvalue);



        if (refreshOnNextTick)
        {
            ret = RefreshBeforeBuild(ret);
            refreshOnNextTick = false;
        }

        float currentmin = ret.currentmin;
        float currentmax = ret.currentmax;
        _minkey = ret.minkey;
        _maxkey = ret.maxkey;
        bool _minset = ret.minset;
        bool _maxset = ret.maxset;


        //testBreak("currentmin = " + currentmin + ", " + _minkey + " currentmax = " + currentmax + ", " + _maxkey);
        if (_lastMin != currentmin || _lastMax != currentmax)
        {
//            Debug.LogError("currentmin = " + currentmin + ", " + _minkey + " currentmax = " + currentmax + ", " + _maxkey);
           // testBreak("dus");
        }
        _lastMin = currentmin;
        _lastMax = currentmax;
	


		// Debug.Log("------------------------------------------------------");





        if (!_minset){

			if (buildDirection == builddirections.Forwards)
			{
				currentmin = _startpos;
			}
			else
			{
				currentmin = 0 - totalvalue;
//                Debug.LogError("currentmin = "+currentmin + " startpos = "+_startpos);
			}



			_minset = true;
        }

        if (!_maxset) {

			if (buildDirection == builddirections.Forwards) {
				currentmax = _startpos;
			}
			else {
				currentmax = _startpos - totalvalue;
			}
			_maxset = true;
        }




		//Debug.LogError("minkey1 = " + currentmax + "maxkey = " + currentmin);
        //Debug.LogError("currentmin = " + currentmax + "currentmax = " + currentmin);
		build("down",currentmax, offset, totalvalue);
		//Debug.LogError("minkey2 = " + currentmax + "maxkey = " + currentmin);
		build("up", currentmin, offset, totalvalue);

	}

    public void ScheduleRebuild(){
        refreshOnNextTick = true;

    }

    public List<GameObject> GetPoolObjects(){
        return _objectPool.GetObjects();
    }





	private void build(string dir, float current, float offset, float totalvalue){
        //Debug.LogError("hallo! "+ _width + "x"+_height);

        //RKLog.LogError("build" + dir, " current " + current);

        bool broken = false;

		int nextkey = this._maxkey + 1;

		if (dir == "up")
		{
			nextkey = this._minkey - 1;
		}
       
        if (nextkey < 0 && _nrOfItems != -1 || ((nextkey > ( _nrOfItems - 1 )) && _nrOfItems != -1) )
        {
           // RKLog.LogError("INFINITESCROLLER BREAK0 " + this.gameObject.name, "infinitescroller");
            return;
        }

        //Debug.LogError("build "+dir);

        int counter = 0;
		while (CheckCanRenderNext(dir,current, offset, totalvalue, broken)) {


            if ((nextkey < 0 && _nrOfItems != -1) || ((nextkey > ( _nrOfItems - 1 )) && _nrOfItems != -1 ))
            {

                RKLog.LogError("INFINITESCROLLER BREAK1 " + this.gameObject.name, "infinitescroller");
                break;
            }




            //       RKLog.LogError("ja, render next " + current + "dir: " + dir + " nrofitems: "+ this._nrOfItems+ " : "+ this.gameObject.name + " nextkey: "+ nextkey + " >= "+ ( _nrOfItems - 1 ), "infinitescroller");


            if (_nrOfItems == 0) {
                RKLog.LogError("INFINITESCROLLER2 HEEFT GEEN DATA "+this.gameObject.name,"infinitescroller");
				break;
			}


           
            GameObject ob = _objectPool.getObject();
			if (!ob) {
                RKLog.LogError("INFINITESCROLLER BREAK2 " + this.gameObject.name, "infinitescroller");
                broken = true;
                break;
			}

            if (dir == "up")
            {
                //Debug.LogError("got ob ");
            }


            //IInfiniteScrollPrefab obinterface = ob.GetInterface<IInfiniteScrollPrefab>();

            //obinterface.setData<IInfiniteScrollPrefab>(_data);
            //obinterface.setKey(nextkey);
            // obinterface.SetKey(nextkey);
            ob.name = nextkey.ToString();
            //RKLog.LogError("Get nextkey " + nextkey + " length "+_nrOfItems, "infinitescroller");
            if (nextkey <= _nrOfItems || _nrOfItems == -1)
            {
                _callback(nextkey, ob);
            }

            float size = ob.GetComponent<RectTransform>().sizeDelta.y;

			if (direction == directions.Horizontal) {
				size = ob.GetComponent<RectTransform>().sizeDelta.x;
				float add = 0;
				if (dir == "up") {
					add = size;
				}
				ob.absolutePosition(new Vector2(current-add, 0));

			}
			else {
                
				float add = 0;
				if (dir == "up")
				{
					add = size;
				}
//                Debug.LogError("add pixels " + size + " dir = "+dir+ " nextkey = " + nextkey + "current:  " + current +" add: "+ add + " height: "+_height);


				ob.absolutePosition(new Vector2(0, current+add));

                //Debug.LogError("place on " + ob.x() + " - " + ob.y());
				//Debug.LogError("place on "+(current + add));
			}

            //Debug.Log("check outeritem "+nextkey + " = "+ outerItem(nextkey));

			if (outerItem(nextkey))
			{
				if (nextkey == 0)
				{
					if (direction == directions.Horizontal)
					{
						_scroller.maxX = 0;
                        if (_nrOfItems == 1) {
                            _scroller.minX = 0;
                        }
					}
					else {

						_scroller.minY = 0 - ob.y()+_startpos;
                        if (_nrOfItems == 1)
                        {
                            if (buildDirection == builddirections.Backwards)
                            {
                                _scroller.maxY = (0 - ob.y() - _height) + size + _spacing;
                                _scroller.minY = _scroller.maxY;
                            }
                            else {
                                _scroller.maxY = _scroller.minY;
                            }
                        }
					}
				}
				else {
					if (direction == directions.Horizontal)
					{
						//Debug.LogError(ob.x());
						_scroller.minX = (0-ob.x()+_width)-size-_spacing;
					}
					else
					{

                        //_scroller.maxY = (0-ob.y()-_height)+size+_spacing;
                        //float newheight = _height;

                        float scrollDistance = ((0 - ob.y()) + size + _spacing);
                        float dis = scrollDistance - _height;
                        RKLog.Log("maxy = " + scrollDistance + " height " + _height + " dis " + dis);
                        if (dis < 0)
                        {
                            dis = 0;
                        }


                       

                        _scroller.maxY = dis;


                       

                        RKLog.LogError("setting maxy, item "+ nextkey + " maxy:" + _scroller.maxY + " oby: "+ ob.y() + ", obj height " + size + " height: "+_height + " size: "+size + " spacing: "+_spacing + "" );
                        //RKLog.LogError("maxy "+_scroller.GetComponent<RectTransform>().rect.height + " size: "+size + " spacing: "+ _spacing);
                    }
				}

				if (_scroller.minX > _scroller.maxX) {
					_scroller.minX = _scroller.maxX;
				}

			}

            if (_scroller.minY > _scroller.maxY) {
                if (buildDirection == builddirections.Backwards)
                {
                    _scroller.minY = _scroller.maxY;
                }
                else
                {
                    _scroller.maxY = _scroller.minY;
                }

            }




//            Debug.LogError("SET SCROLLER "+ _scroller.minY +"x" +_scroller.maxY);


			counter++;





			if ((direction == directions.Horizontal && dir == "down") || direction == directions.Vertical && dir == "up") {
				current = current + size + _spacing;

			}
			if ((direction == directions.Horizontal && dir == "up") || direction == directions.Vertical && dir == "down")
			{
				current = current - size - _spacing;
			}

			if (dir == "down") {
//                RKLog.LogError("nextkey down" + nextkey, "infinitescroller");
                nextkey++;
			}
			else if (dir == "up") {
//                RKLog.LogError("nextkey up" +nextkey, "infinitescroller");
                nextkey--;
			}


			///}


		}


		var currentpos = 0;
	}

    public bool outerItem( int key )
    {

        if (_nrOfItems == -1)
        {
            return false;
        }

        if (key == 0)
        {
            return true;
        }
        else if (key == _nrOfItems-1)
        {
            return true;
        }
        return false;
    }

    private bool CheckCanRenderNext(string dir, float current, float offset, float totalvalue, bool broken) {
      
        if (direction == directions.Horizontal && dir == "up")
		{

			if ((current + offset) > 0) {
//                RKLog.LogError("canrender 1 " + this.gameObject.name, "infinitescroller");
                return true;
			}
		}
		if (direction == directions.Vertical && dir == "up")
		{

			if ((current + offset) < 0 + GetPadding())
			{
 //               RKLog.LogError("canrender 2 " + this.gameObject.name, "infinitescroller");
                return true;
			}
		}
		else if (direction == directions.Horizontal && dir == "down")
		{
			if ((current + offset) < (totalvalue) && broken == false)
			{
//                RKLog.LogError("canrender 3 " + this.gameObject.name + " current+offset = "+ ( current + offset ) + " totalvalue: "+totalvalue, "infinitescroller");
                return true;
			}
		}
		else if (direction == directions.Vertical && dir == "down")
		{
			if ((current + offset) > (0 - totalvalue) && broken == false) {
//                RKLog.LogError("canrender 4 " + this.gameObject.name, "infinitescroller");
                return true;
			}
		}
		return false;
	}

    void Update()
    {
        if (_destroyed)
        {
            return;
        }
        if (_nrOfItems == 0) {
            RKLog.LogError("INFINITESCROLLER2 HEEFT GEEN DATA","infinitescroller");
        }
        if (lastCount == 0 && _nrOfItems > 0) {
            Rebuild();
        }

		if (started) {
			fill();
		}
       
        if (_nrOfItems != 0)
        {
            lastCount = _nrOfItems;
        }
    }

    private void ObjectClicked(int id) {
        clicked.Invoke(id);
    }

    private void createCache(){
        int i = 0;
        while (i < _cachenumber) {
            
            GameObject go = HelperFunctions.GetPrefab2d(_prefab, gameObject, 0, 0);
            ClickBaseClass obinterface = go.GetComponent<ClickBaseClass>();
            if (obinterface != null) {
                obinterface.Clicked.AddListener(ObjectClicked);
            }
            _objectPool.Add(go);
            i++;
        }
    }

	private void OnDestroy()
	{
        _destroyed = true;
		foreach (GameObject ob in _objectPool.GetObjects())
        {
            ClickBaseClass obinterface = ob.GetComponent<ClickBaseClass>();
            if (obinterface)
            {
                obinterface.Clicked.RemoveAllListeners();
            }
        }
	}
}