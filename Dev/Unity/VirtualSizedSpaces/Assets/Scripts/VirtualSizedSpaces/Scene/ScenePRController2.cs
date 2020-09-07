using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePRController2 : MonoBehaviour
{
    private TestSettings _testSettings = new TestSettings();
    private TestingResult _currentTest;
    private int _currentTestKey = 22;
    private GameObject _wallLeft;
    private GameObject _wallRight;
    private GameObject _wallFront;
    private GameObject _wallBack;
    private Scatter _scatter = new Scatter();
    private UserController2 _userController;
    private Button _selectButton;
    private SceneRotaterController _sceneRotator;
    private GameObject _allContainer;

    private void Init() {
        _allContainer = transform.Find("AllContainer").gameObject;
        _wallLeft = transform.Find("AllContainer/Walls/left").gameObject;
        _wallRight = transform.Find("AllContainer/Walls/right").gameObject;
        _wallFront = transform.Find("AllContainer/Walls/front").gameObject;
        _wallBack = transform.Find("AllContainer/Walls/back").gameObject;
        _userController = transform.Find("OVRCameraRig").GetComponent<UserController2>();
        _selectButton = GameObject.Find("SelectButton").GetComponent<Button>();
        _selectButton.onClick.AddListener(NextTest);
        _sceneRotator = this.gameObject.AddComponent<SceneRotaterController>();
        _sceneRotator.set(_userController.gameObject,_allContainer.gameObject);
        _userController.setRotator(_sceneRotator);
    }

    private void NextTest()
    {
        _currentTestKey++;
        if (_testSettings.tests.Count > _currentTestKey)
        {
            UpdateButton();
            StartTest(_currentTestKey);
        }

    }

    void Start()
    {
        Init();
        StartTest(_currentTestKey);
       
    }

    private void UpdateButton() {
        if (_testSettings.tests.Count - 1 > _currentTestKey)
        {
            _selectButton.transform.Find("Text").GetComponent<Text>().text = _testSettings.tests[_currentTestKey + 1];
            _selectButton.gameObject.SetActive(true);
        }
        else {
            _selectButton.gameObject.SetActive(false);
        }
    }

    private void StartTest(int testnr) {
        string _currentTestName = _testSettings.tests[testnr];
        UpdateButton();

        _currentTest = _testSettings.GetSettings(_currentTestName);
        ApplySettings();
    }

    private void ApplySettings() {
       
        //_wallLeft.transform.position = new Vector3(_currentTest.XWallDistance,500,0);
        //_wallRight.transform.position = new Vector3(-_currentTest.XWallDistance, 500, 0);
        //_wallFront.transform.position = new Vector3(0, 500, _currentTest.YWallDistance);
        //_wallBack.transform.position = new Vector3(0, 500, -_currentTest.YWallDistance);
        //_scatter.Make("Cube", this.transform.Find("AllContainer").gameObject, _currentTest.NrBoxes, 0.5f, 1000, 1000);
        _userController.setSettings(_currentTest);
        _sceneRotator._threshold = _currentTest.threshold;
        _sceneRotator._factor = _currentTest.factor;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            NextTest();
        }
    }
}
