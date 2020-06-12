using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxChild : MonoBehaviour {
    public float factor = 0;
    private Vector2 _position;
    private RectTransform _rect;


	// Use this for initialization
	void Start () {
        _rect = GetComponent<RectTransform>();
        _position = _rect.anchoredPosition;
        
	}


    public void update(float y)
    {
        //        Debug.Log(position);
        if (_rect != null) {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(_position[0]+(y*factor), _position[1]);
            //transform.localPosition = new Vector2(_position[0], _position[1]);
        }
        //Debug.Log(transform.localPosition);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
