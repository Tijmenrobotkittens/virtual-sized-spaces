using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePRController : MonoBehaviour
{
    private TestSettings _testSettings = new TestSettings();
    private TestingResult _currentTest;
    private string _currentTestName = "test1";

    void Start()
    {
        _currentTest = _testSettings.GetSettings(_currentTestName);


        new Scatter("Cube",this.gameObject,500,0.5f,800,800);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
