using System;
using System.Collections.Generic;
using RobotKittens;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBaseClass: MonoBehaviour, IPointerClickHandler
{
	public IntEvent Clicked = new IntEvent();

	public virtual void OnPointerClick( PointerEventData eventData )
	{
		Debug.LogError("CLICKED");
	}
}
