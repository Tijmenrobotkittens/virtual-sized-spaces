﻿using System.Collections;
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
    public List<string> tests = new List<string> { { "test1" }, { "test2" }, { "test3" }, { "test4" }, { "test5" }, { "test6" }, { "test7" }, { "test8" }, { "test9" }, { "test10" }, { "test11" }, { "test12" }, { "test13" }, { "test14" }, { "test15" }, { "test16" }, { "test17" }, { "test18" }, { "test19" }, { "test20" }, { "test21" }, { "test22" } };

    public TestingResult GetSettings(string testname)
    {
        TestingResult settings = new TestingResult();
        switch (testname)
        {
            case "test1":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 0f;
                break;
            case "test2":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 1f;
                break;
            case "test3":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 2f;
                break;
            case "test4":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 3f;

                break;
            case "test5":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 4f;
                break;
            case "test6":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 5f;
                break;
            case "test7":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 6f;
                break;
            case "test8":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 7f;
                break;
            case "test9":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 8f;
                break;
            case "test10":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 9f;
                break;
            case "test11":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 9.5f;
                break;
            //Test day 2
            case "test12":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 10f;
                break;
            case "test13":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 12f;
                break;
            case "test14":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 14f;
                break;
            case "test15":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 15f;

                break;
            case "test16":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 16f;
                break;
            case "test17":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 17f;
                break;
            case "test18":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 18f;
                break;
            case "test19":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 19f;
                break;
            case "test20":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 20f;
                break;
            case "test21":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 21f;
                break;
            case "test22":
                settings.XWallDistance = 1000;
                settings.YWallDistance = 1000;
                settings.MaxAngle = 22f;
                break;
        }
        return settings;
    }
}

  
