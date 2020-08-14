using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingResult {
    public int XWallDistance = 1000;
    public int YWallDistance = 1000;
    public float Distance = 10;
    public float MaxAngle = 1;
    public int NrBoxes = 500;
}

public class TestSettings
{
    public List<string> tests = new List<string>{{"test1"}, { "test2" } , { "test3" } , { "test4" } , { "test5" } , { "test6" } , { "test7" } , { "test8" } };

    public TestingResult GetSettings(string testname) {
        TestingResult settings = new TestingResult();
        switch (testname) {
            case "test1":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0.1f;
                break;
            case "test2":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0.2f;
                break;
            case "test3":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0.3f;
                settings.NrBoxes = 2000;
                break;
            case "test4":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0.4f;
                settings.NrBoxes = 2000;
                break;
            case "test5":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0.5f;
                settings.NrBoxes = 2000;
                break;
            case "test6":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 1f;
                break;
            case "test7":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 1.5f;
                break;
            case "test8":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 2f;
                break;
        }
        return settings;
    }

   
}
