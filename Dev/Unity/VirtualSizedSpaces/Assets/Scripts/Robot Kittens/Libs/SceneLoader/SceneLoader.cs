using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using RobotKittens;

public class SceneEvent : UnityEvent<States.State> { }

public class SceneLoader : MonoBehaviour
{   
    private static SceneLoader _instance;
    private Dictionary<States.State, SceneData> scenes = new Dictionary<States.State, SceneData>();
    public SceneEvent sceneloaded = new SceneEvent();
    public UnityEvent loaded = new UnityEvent();

    public static SceneLoader Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("SceneLoader").AddComponent<SceneLoader>();
            }
            return _instance;
        }
    }

    public List<GameObject> GetActiveGameObjects(){
        List<GameObject> objects = new List<GameObject>();
        foreach (KeyValuePair<States.State, SceneData> entry in scenes)
        {
            if (entry.Value.gameobject.activeSelf) {
                objects.Add(entry.Value.gameobject);
            }
            // do something with entry.Value or entry.Key
        }
        return objects;
    }


    private bool SceneExists(States.State name){
        if (scenes.ContainsKey(name)) {
            return true;
        }
        else {
            RKLog.Log("Scene "+ name + " does not exist", "sceneloader");
            return false;
        }
    }

   


   
    //public void setData(States.State name, GenericData data){
    //    if (SceneExists(name)) {

    //        if (IsSceneLoaded(name))
    //        {
    //            SceneBaseClass baseclass = scenes[name].gameobject.GetComponent<SceneBaseClass>();
    //            if (baseclass) {
    //                Debug.LogError("Scenebaseclass found in " + name + " - " + scenes[name].gameobject + " and data set!");
    //                baseclass.setData(data);
    //            }
    //            else {
    //                Debug.LogError("Scenebaseclass not found in "+ name + " - "+ scenes[name].gameobject);
    //            }
    //        }
    //        else {
    //            Debug.LogError("Scene " + name + " not loaded, set tempdata ");
    //            scenes[name].tempdata = data;
    //        }
    //    }
    //    else {
    //        Debug.LogError("Scene " + name + " does not exist ");
    //    }

    //}

    public void Init(States.State name)
    {
        
        if (SceneExists(name))
        {
            scenes[name].Scene.BaseInit();
        }
    }

    public void Preload(States.State name, Action callback) {
        if (scenes.ContainsKey(name))
        {
            if (scenes[name].loaded == false && scenes[name].loading == false)
            {
   
                LoadScene(name);
                scenes[name].gameobject.transform.SetParent(scenes[name].target.transform);
                scenes[name].gameobject.SetActive(true);
                scenes[name].gameobject.absolutePosition(new Vector2(-5000, -5000));
                scenes[name].Scene.Preload(()=>Preloaded(name,callback));

            }

            
        }
    }

    private void Preloaded(States.State name, Action callback)
    {
        Debug.LogError("Preloading done! Supervet!");
        callback();
        
    }

    public bool IsSceneLoaded(States.State name)
    {
        if(this.SceneExists(name))
        {
            return scenes[name].loaded;
        }
        return false;
    }

    private void CheckLoadedAll()
    {
        bool allloaded = true;
        foreach (KeyValuePair<States.State, SceneData> entry in scenes)
        {
            SceneData scene = entry.Value;
            if (scene.load == true)
            {
                allloaded = false;
            }
            if (scene.loading == true) {
                allloaded = false;
            }
        }
        if (allloaded) {
            loaded.Invoke();
        }
        else {
            Debug.LogError("NOT ALL LOADED");
        }
    }

    public void Hide(States.State name) {
        if (scenes.ContainsKey(name)) {
            if (scenes[name].DestroyOnMoveOut)
            {
                Unload(name);
               


            }
            else
            {
                scenes[name].gameobject.SetActive(false);
            }
        }
    }

    public void Show(States.State name)
    {
        if (scenes.ContainsKey(name))
        {
            scenes[name].gameobject.absolutePosition(new Vector2(0, 0));
            scenes[name].gameobject.SetActive(true);
        }
    }

    public  GameObject GetGameObject(States.State name) {
        if (scenes.ContainsKey(name)) {
            if (scenes[name].loaded == false && scenes[name].loading == false) {
                LoadScene(name);
            }


           // await checkSceneLoaded();

            return scenes[name].gameobject;
        }
        else {
            Debug.LogError("get gameobject "+ name + " does not exist!");
            return new GameObject();
        }
    }

    public SceneBaseClass GetScene(States.State name)
    {
        if (scenes.ContainsKey(name))
        {
            if (scenes[name].loaded == false && scenes[name].loading == false)
            {
                LoadScene(name);
            }


            // await checkSceneLoaded();

            return scenes[name].Scene;
        }
        else
        {
            Debug.LogError("get scene " + name + " does not exist!");
            return null;
        }
    }

    IEnumerator CheckSceneLoaded(States.State scene)
    {
        int maxticks = 10000;
        int i = 0;
        while (scenes[scene].loaded == false && i < maxticks) {
            yield return new WaitForSeconds(0.1f);    
        }
    }

    public SceneData Scene(States.State name)
    {
        if (SceneExists(name))
        {
            return scenes[name];
        }
        else
        {
            RKLog.Log("get scene " + name + " does not exist!", "sceneloader");
            return null;
        }
    }

    public void Unload(States.State scene) {
        RKLog.Log("unload "+scene,"sceneloader");
        Destroy(scenes[scene].gameobject);
        scenes[scene].load = false;
        scenes[scene].loaded = false;
        scenes[scene].loading = false;
        scenes[scene].gameobject = null;
        //scenes.Remove(scene);
    }


    public void LoadScene(States.State name) {
        RKLog.Log("load scene "+name, "sceneloader");
        if (scenes[name].loading == false && scenes[name].loaded == false)
        {
           // Debug.LogError(scenes[name].name);
            scenes[name].loading = true;
            GameObject g = HelperFunctions.GetPrefab2d(scenes[name].name, scenes[name].target);

            if (g == null) {
                Debug.LogError("NO GAMEOBJECT LOADED "+name);
                return;
            }

            SceneBaseClass sb = g.GetComponent<SceneBaseClass>();

            if (sb == null)
            {
                Debug.LogError("NO SCENEBASECLASS LOADED " + name);
                return;
            }




            scenes[name].gameobject = g;
            scenes[name].Scene = sb;


            scenes[name].loaded = true;
            scenes[name].load = false;
            scenes[name].loading = false;

            bool issceneloaded = false;

            sceneloaded.Invoke(name);

            CheckLoadedAll();
        }
        else if (scenes[name].loaded) {
            Debug.LogError("Scene "+name + " already loaded");
        }
    }


    public SceneLoader AddScene(States.State state, string scene, GameObject target, bool hideOnStart = false, bool load = true, bool destroyOnMoveOut = false) {
        scenes[state] = new SceneData();
        scenes[state].target = target;
        scenes[state].State = state;
        scenes[state].name = scene;
        scenes[state].hideOnStart = hideOnStart;
        scenes[state].load = load;
        scenes[state].DestroyOnMoveOut = destroyOnMoveOut;
        //if (scenes[state].loadOnStart)
        //{
        //    LoadScene(state);
        //}

        return this;
    }

    public void Load() {
        foreach (KeyValuePair<States.State, SceneData> entry in scenes)
        {
            SceneData scene = entry.Value;
            if (scene.load == true && scene.loaded == false && scene.loading == false) {
                RKLog.Log("Load "+scene.name);
                LoadScene(entry.Key);
            }

        }
    }

}
