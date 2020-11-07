using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUserController : Photon.MonoBehaviour
{
    private List<GameObject> _users = new List<GameObject>();
    private string _otherUserPrefab;
    private GameObject _parent;
    public bool AutoConnect = true;
    public byte Version = 1;
    private bool ConnectInUpdate = true;
    public string _roomName = "VirtualSpacesTest";


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
        PhotonNetwork.JoinRoom(_roomName);
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("Multi: Joined lobby");
        PhotonNetwork.JoinRoom(_roomName);
    }

    public virtual void OnPhotonJoinFailed()
    {
        Debug.Log("Multi: Room joined failed!");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Multi: Room joined!");
    }
}
