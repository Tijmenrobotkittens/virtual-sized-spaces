using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePRController : MonoBehaviour
{
    private TestSettings _testSettings = new TestSettings();
    private TestingResult _currentTest;
    private string _currentTestName = "test1";
    private GameObject _wallLeft;
    private GameObject _wallRight;
    private GameObject _wallFront;
    private GameObject _wallBack;
    private Scatter _scatter = new Scatter();

    private void Init() {
        _wallLeft = transform.Find("Walls/left").gameObject;
        _wallRight = transform.Find("Walls/right").gameObject;
        _wallFront = transform.Find("Walls/front").gameObject;
        _wallBack = transform.Find("Walls/back").gameObject;
    }

    void Start()
    {
        Init();
        _currentTest = _testSettings.GetSettings(_currentTestName);
        ApplySettings();

       
    }

    private void ApplySettings() {
       
        _wallLeft.transform.position = new Vector3(_currentTest.XWallDistance,500,0);
        _wallRight.transform.position = new Vector3(-_currentTest.XWallDistance, 500, 0);
        _wallFront.transform.position = new Vector3(0, 500, _currentTest.YWallDistance);
        _wallBack.transform.position = new Vector3(0, 500, -_currentTest.YWallDistance);
        _scatter.Make("Cube", this.gameObject, _currentTest.NrBoxes, 0.5, 1000, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
