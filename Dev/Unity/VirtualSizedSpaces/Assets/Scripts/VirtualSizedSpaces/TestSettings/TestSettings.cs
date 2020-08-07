using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingResult {
    public int XWallDistance = 1000;
    public int YWallDistance = 1000;
    public float Distance = 1;
    public float MaxAngle = 50;
    public int NrBoxes = 500;
}

public class TestSettings
{
    public TestingResult GetSettings(string testname) {
        TestingResult settings = new TestingResult();
        switch (testname) {
            case "test1":
                settings.XWallDistance = 400;
                settings.YWallDistance = 200;
                break;
        }
        return settings;
    }

   
}
