using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{    
    public static float width = 1080;
    public static float height = HelperFunctions.GetHeight(width);
    public static bool StartedIsolated = true;
    public static AppController.Environments Environment;
    public static float SafetyY;
    public static float SafetyBottom;
    public static string AppVersion = "0.1";
    public static string URL_LOCAL = "http://localhost:3000/dev/v1/";
    public static string URL_STAGING = "https://vccs1d3ja6.execute-api.eu-west-1.amazonaws.com/dev/v1/";
    public static string URL_PRODUCTION = "https://api.vidate.app/v1/";
    public static string SOCKET_STAGING = "wss://bn9nvnzalc.execute-api.eu-west-1.amazonaws.com/dev";
    public static string SOCKET_LOCAL = "wss://bn9nvnzalc.execute-api.eu-west-1.amazonaws.com/dev";
    public static string SOCKET_PRODUCTION = "wss://socket.vidate.app";
    public static string DEMO_VIDEO_PATH = "https://vidate-staging-pub.s3-eu-west-1.amazonaws.com/Demos/";

    public static string GetURL()
    {
        switch (Environment)
        {
            case AppController.Environments.Local:
                return URL_LOCAL;
                break;
            case AppController.Environments.Staging:
                return URL_STAGING;
                break;
            case AppController.Environments.Production:
                return URL_PRODUCTION;
                break;
        }
        return URL_LOCAL;
    }

    public static string GetSocket()
    {
        switch (Environment)
        {
            case AppController.Environments.Local:
                return SOCKET_LOCAL;
                break;
            case AppController.Environments.Staging:
                return SOCKET_STAGING;
                break;
            case AppController.Environments.Production:
                return SOCKET_PRODUCTION;
                break;
        }
        return SOCKET_STAGING;
    }
}