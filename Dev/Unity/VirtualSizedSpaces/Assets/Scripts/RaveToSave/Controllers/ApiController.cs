using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using BestHTTP;
using BestHTTP.JSON;
using Newtonsoft.Json;
using RobotKittens;
using UnityEngine;
using UnityEngine.Networking;

public class ApiController:MonoBehaviour
{
    private static ApiController _instance;
    public enum SendTypes {
        GET_EVENTS,
        
    }

    public enum ReturnTypes
    {
        SUCCESS,
        ERROR
    }
    public const string PATH_EVENTS = "api/events";
    


    public static ApiController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("ApiController").AddComponent<ApiController>();
                _instance.Init();
            }
            return _instance;
        }
    }

    private void Init()
    {
        RKLog.Log("Load data from "+Config.GetURL(),"apicontroller");
    }

    public string GetParsedUrl(string url, GenericData replacements = null) {
        string baseurl = Config.GetURL() + url;
        if (replacements != null) {
            

            foreach (KeyValuePair<string, int> keyval in replacements.Ints)
            {
                baseurl = baseurl.Replace("{" + keyval.Key + "}", keyval.Value.ToString());
                RKLog.Log("replace {" + keyval.Key + "} with " + keyval.Value.ToString(),"apicontroller");
            }

            foreach (KeyValuePair<string, string> keyval in replacements.Strings)
            {
                baseurl = baseurl.Replace("{" + keyval.Key + "}", keyval.Value);
                RKLog.Log("replace {" + keyval.Key + "} with " + keyval.Value.ToString(), "apicontroller");
            }

            foreach (KeyValuePair<string, float> keyval in replacements.Floats)
            {
                baseurl = baseurl.Replace("{" + keyval.Key + "}", keyval.Value.ToString());
                RKLog.Log("replace {" + keyval.Key + "} with " + keyval.Value.ToString(), "apicontroller");
            }

            foreach (KeyValuePair<string, bool> keyval in replacements.Bools)
            {
                baseurl = baseurl.Replace("{" + keyval.Key + "}", keyval.Value.ToString());
                RKLog.Log("replace {" + keyval.Key + "} with " + keyval.Value.ToString(), "apicontroller");
            }
           
        }

        return baseurl;
    }


    public void Request<T>(SendTypes type, GenericData values, Action<ReturnTypes, T, int> action) {
        string url = "";

        switch (type)
        {

            case SendTypes.GET_EVENTS:
                url = GetParsedUrl(PATH_EVENTS, values);
                Post(url, type, values, action);
                break;
           
        }

    }

    public void Get<T>(string url, SendTypes type, Action<ReturnTypes, T, int> action) {
        HTTPRequest request = new HTTPRequest(new Uri(url), (HTTPRequest originalRequest, HTTPResponse response) => ParseReturn<T>(originalRequest, response, type, action));
       

        request.Send();
    }

    public void Post<T>(string url, SendTypes type, GenericData data, Action<ReturnTypes, T, int> action)
    {

        HTTPRequest request = new HTTPRequest(new Uri(url),HTTPMethods.Post,(HTTPRequest originalRequest, HTTPResponse response) => ParseReturn<T>(originalRequest, response, type, action));
        //if (UserController.Instance.IsLoggedIn())
        //{
        //    request.SetHeader("User-Token", UserController.Instance.Token);
        //}

        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        Dictionary<string, string> vals = new Dictionary<string, string>();

        foreach (KeyValuePair<string, int> keyval in data.Ints)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, string> keyval in data.Strings)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, float> keyval in data.Floats)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

        foreach (KeyValuePair<string, bool> keyval in data.Bools)
        {
            vals[keyval.Key] = keyval.Value.ToString();
        }

         string json = JsonConvert.SerializeObject(vals, Formatting.Indented);
        
//        Debug.Log("tosend = "+json);
        request.RawData = System.Text.Encoding.UTF8.GetBytes(json);
        request.Send();
    }



    private void ParseReturn<T>(HTTPRequest originalRequest, HTTPResponse response, SendTypes type, Action<ReturnTypes, T, int> action)
    {

        if (response == null) {
            RKLog.LogError("Parsereturn, response null ","apicontroller");
            action(ReturnTypes.ERROR, (T)(object)null, 1);
            return;
        }

        GenericData returnResponse = new GenericData();
        SendValueResponse res;
        int returnerror;

        //Debug.Log("parsereturn 2!");
        //        Debug.LogError("dit is iets ervoor");
        //        Debug.LogError(response.DataAsText);

   

        ReturnTypes returntype;
        switch (type)
        {
            default:
                RKLog.Log("response: "+ response.DataAsText);

                res = JsonUtility.FromJson<SendValueResponse>(response.DataAsText);
                foreach (ResponseStringList val in res.strings) {
                    Debug.Log("set3: " + val.key + " to " + val.value);
                    returnResponse.Set(val.key, val.value);
                }

                foreach (ResponseIntList val in res.ints)
                {
                    returnResponse.Set(val.key, val.value);
                }

                returntype = ReturnTypes.ERROR;
                 returnerror = -1;

                if (res.success) {
                    returntype = ReturnTypes.SUCCESS;
                }

                RKLog.Log("returntype: " + returntype + " returnresponse: " + returnResponse + " res error: " + res.error);
                action(returntype, (T)(object)returnResponse, res.error);


                break;
        }
    }
}