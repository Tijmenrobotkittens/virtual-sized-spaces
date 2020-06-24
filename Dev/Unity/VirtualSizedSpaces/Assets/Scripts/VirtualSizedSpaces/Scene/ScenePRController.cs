using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePRController : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        new Scatter("Cube",this.gameObject,500,0.5f,800,800);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
