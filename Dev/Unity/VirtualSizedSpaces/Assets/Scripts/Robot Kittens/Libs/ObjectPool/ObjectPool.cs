using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPoolDeactivateEvent : UnityEvent<GameObject> { }

public class ObjectPool  {
    private List<GameObject> _activeObjects = new List<GameObject>();
    private List<GameObject> _inactiveObjects = new List<GameObject>();
    private Dictionary<GameObject, int> _ObjectIds = new Dictionary<GameObject,int>();
    private string _autoRefill = "";
    private GameObject _autoRefillTarget = null;
    public ObjectPoolDeactivateEvent deactivated = new ObjectPoolDeactivateEvent();

    public bool reuseID = false; 

    public void Clear(){
        _activeObjects.Clear();
        _inactiveObjects.Clear();
        _ObjectIds.Clear();

    }

    public void Add(GameObject go) {
        _inactiveObjects.Add(go);
        go.SetActive(false);
    }

    public void AutoRefill(string prefab,GameObject target) {
        _autoRefill = prefab;
        _autoRefillTarget = target;
    }

    public GameObject getObject(int id = -1){
       // Debug.LogError("GetObject "+_inactiveObjects.Count + " - "+_activeObjects.Count);

        if (_inactiveObjects.Count > 0) {
            int key = 0;
            if (reuseID == true && id != -1){
                int newkey = 0;
                foreach (GameObject obj in _inactiveObjects) {
                    if (_ObjectIds.ContainsKey(obj)) {
                        if (_ObjectIds[obj] == id) {
                            key = newkey;
                          //  Debug.LogError("Ja, reuse "+ newkey);
                        }
                    }
                    newkey++;
                }
            }

            if (key == 0) {
               // Debug.LogError("do not reuse, pick first "+key);
            }

            GameObject ob = _inactiveObjects[key];
            _inactiveObjects.Remove(ob);
            _activeObjects.Add(ob);
            ob.SetActive(true);
            if (id != -1) {
                _ObjectIds[ob] = id;
            }

            return ob;
        }
        else {

            if (_autoRefill != "") {
//                Debug.LogError("Objects depleted! Autofill enabled! Creating!");
                Add(HelperFunctions.GetPrefab2d(_autoRefill, _autoRefillTarget));
                return getObject();
            }
            else {
  //              Debug.LogError("Objects depleted! problem! panic!");
                return null;    
            }


        }
    }



    public List<GameObject> getActive(){
        return _activeObjects;
    }



	public List<GameObject> GetObjects(){
		List<GameObject> objs = new List<GameObject>();
		foreach (GameObject go in _activeObjects) {
			objs.Add(go);
		}

		foreach (GameObject go in _inactiveObjects)
		{
			objs.Add(go);
		}

		return objs;
	}

    public void deactivate(GameObject obj) {

        deactivated.Invoke(obj);
        obj.SetActive(false);
        _activeObjects.Remove(obj);
        _inactiveObjects.Insert(0, obj);
    }

    public void deactivateAll()
    {
       
        List<GameObject> _tempactive = new List<GameObject>();

       

        foreach (GameObject go in _activeObjects) {
            _tempactive.Add(go);
        }

        foreach (GameObject go in _tempactive)
        {
            deactivate(go);
        }
    }

    public void Destroy() {
        deactivated.RemoveAllListeners();
    }

    

}