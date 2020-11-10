using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiUserController : Photon.MonoBehaviour
{
    private List<GameObject> _users = new List<GameObject>();
    private string _otherUserPrefab;
    private GameObject _parent;
    public bool AutoConnect = true;
    public byte Version = 1;
    private bool ConnectInUpdate = true;
    public string _roomName = "VirtualSpacesTest";
    private UnityEvent UserConnected = new UnityEvent();
    private bool _userConnected = false;


    public void SetOtherUser(string prefab,GameObject parent) {
        _otherUserPrefab = prefab;
        _parent = parent;
    }

    void Start()
    {
        PhotonNetwork.autoJoinLobby = false;

    }

    internal void AddUser(GameObject user)
    {
        _users.Add(user);
    }

    private void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Multi: Connect to multiuser server");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("Multi: Connect to master");
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("Multi: Joined lobby");
        PhotonNetwork.JoinRoom(_roomName);
    }


    public virtual void OnPhotonRoomJoinFailed()
    {
        Debug.Log("Multi: Room joined failed!");
       
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Multi: Room joined!");
        _userConnected = true;
        UserConnected.Invoke();
    }
}
