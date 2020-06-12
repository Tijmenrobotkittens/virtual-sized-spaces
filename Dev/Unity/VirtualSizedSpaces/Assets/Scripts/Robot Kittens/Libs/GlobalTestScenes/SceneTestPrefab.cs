using System.Collections;
using System.Collections.Generic;
using RobotKittens;
using UnityEngine;


public class SceneTestPrefab : UiElement
{
	public string PrefabToLoad = "";
	private Canvas _canvas;
	protected override void Init()
	{
		RKLog.Log("Load prefab " + PrefabToLoad);
		_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        HelperFunctions.GetPrefab2d(PrefabToLoad, _canvas.gameObject);
		base.Init();
	}
}
