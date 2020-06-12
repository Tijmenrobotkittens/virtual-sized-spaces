using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using BestHTTP.WebSocket;
using Newtonsoft.Json;

public class SocketEvent : UnityEvent<string,GenericData> { }

public class SocketController : MonoBehaviour
{
    public SocketEvent MessageReceived = new SocketEvent();
    public DataControllerEvent Updated = new DataControllerEvent();
    private static SocketController _instance;
    public enum DataTypes {MATCHES};
    private WebSocket _websocket;
    private bool _connected = false;
    public int _lastStateCounter = 0;
    private WebSocketStates _lastState = WebSocketStates.Unknown;
    public static SocketController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("SocketController").AddComponent<SocketController>();
                _instance.Init();
            }
            
            return _instance;
        }
    }

    private void Disconnect() {
        RKLog.Log("disconnect socket server", "socketcontroller");
        if (_websocket != null)
        {
            _websocket.OnOpen -= OnWebSocketOpen;
            _websocket.OnMessage -= OnMessageReceived;
            _websocket.OnClosed -= OnWebSocketClosed;
            _websocket.OnError -= OnError;
            _websocket.Close();
            _websocket = null;
        }
    }

    private void Connect()
    {
        RKLog.Log("connect socket server", "socketcontroller");
        Disconnect();

        _websocket = new WebSocket(new Uri(Config.GetSocket()));
        _websocket.OnOpen += OnWebSocketOpen;
        _websocket.OnMessage += OnMessageReceived;
        _websocket.OnClosed += OnWebSocketClosed;
        _websocket.OnError += OnError;
        _websocket.StartPingThread = true;
        _websocket.PingFrequency = 2000;
        _websocket.CloseAfterNoMesssage = TimeSpan.FromSeconds(4);
        _websocket.Open();
    }

    private void Init()
    {
        RKLog.Log("init","socketcontroller");

        Connect();

        InvokeRepeating("CheckState", 3.0f, 3.0f);
        RKLog.Log("connect to "+ Config.GetSocket(),"socketcontroller");
    }

    private void CheckState() {
        if (_websocket != null)
        {
            Debug.Log("websocket");
            Debug.Log(_websocket);
            _lastState = _websocket.State;
            if (_websocket.State == WebSocketStates.Open)
            {
                RKLog.Log("checkstate " + _websocket.State + " counter " + _lastStateCounter + "connected: " + _connected + " latency: " + _websocket.Latency, "socketcontroller");
            }
            else {
                RKLog.Log("checkstate " + _websocket.State + " counter " + _lastStateCounter + "connected: " + _connected, "socketcontroller");
            }
        }
        if (_connected == false) {
            Connect();
        }

    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Disconnect();
            _connected = false;
        }
        else {
            Connect();
        }
    }

    private void OnWebSocketOpen(WebSocket webSocket)
    {
        _connected = true;
        _lastStateCounter = 0;
        RKLog.Log("WebSocket is now Open!", "socketcontroller");
        SetToken();
    }

    public void SetToken() {
       
    }

    public void Send(int userid, string type, GenericData data) {
        GenericData send = new GenericData();
        send.Set("to", userid);
        send.Set("type", type);
        send.Set("msg",JsonConvert.SerializeObject(data.GetValues()));
        string finaldata = JsonConvert.SerializeObject(send.GetValues());
        RKLog.Log("send request to socket "+finaldata, "socketcontroller");

        _websocket.Send(finaldata);
    }

    private void OnMessageReceived(WebSocket webSocket, string message)
    {
        RKLog.Log("OnMessageReceived: " + message, "socketcontroller");
        SocketResponse data = JsonUtility.FromJson<SocketResponse>(message);

        GenericData gd = new GenericData();
        foreach (ResponseStringList item in data.data) {
            gd.Set(item.key, item.value);
        }

        RKLog.Log("invoke: " + gd.ToString(), "socketcontroller");
        
        MessageReceived.Invoke(data.type,gd);

        if (data.type == "update") {
            RKLog.Log("onmessagereceived, update: " + gd.ToString() + " dus poll", "socketcontroller");
           
        }

    }

    private void OnWebSocketClosed(WebSocket webSocket, UInt16 code, string message)
    {
        RKLog.Log("OnWebSocketClosed", "socketcontroller");
        _connected = false;
    }

    private void OnError(WebSocket ws, string error)
    {
        RKLog.Log("OnError "+error + " "+ _websocket.State, "socketcontroller");
        _connected = false;
    }
}
