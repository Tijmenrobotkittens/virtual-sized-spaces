using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
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
    private int ownId = -1;
    public Dictionary<int, GameObject> _playerObjects = new Dictionary<int, GameObject>();
    private PhotonView _photonView;
    private GameObject _me;
    long microseconds = 0;

    public const byte MoveUserEvent = 1;


    public void SetOtherUser(string prefab,GameObject parent) {
        _otherUserPrefab = prefab;
        _parent = parent;
    }

    public void SetMe(GameObject me) {
        _me = me;
    }

    void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
        _photonView = gameObject.AddComponent<PhotonView>();
        PhotonNetwork.OnEventCall += OnEvent;
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
        else {
            object[] content = new object[] {_me.transform.position,_me.transform.rotation }; // Array contains the target position and the IDs of the selected units
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(MoveUserEvent, content,true,raiseEventOptions);
        }
    }

    private void CreateUser(int id)
    {
        GameObject otherUser = HelperFunctions.GetPrefab(_otherUserPrefab, _parent);
        _playerObjects[id] = otherUser;
    }

    private void OnEvent(byte _eventCode, object _content, int _senderID)
    {
        //if (_senderID == ownId) {
        //    return;
        //}
        if (!_playerObjects.ContainsKey(_senderID)) {
            CreateUser(_senderID);
        }
        object[] mydata = (object[])_content;
        Vector3 position = (Vector3)mydata[0];
        Quaternion rotation = (Quaternion)mydata[1];

        //for (int i = 0; i < positions.Length; i++) {
        //    Debug.Log(positions[i]);
        //}

        _playerObjects[_senderID].transform.position = position;
        _playerObjects[_senderID].transform.rotation = rotation;
        //_playerObjects[_senderID].transform.rotation = Quaternion.Euler(positions[1]);
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
        
        if (PhotonNetwork.playerList.Length == 1)
        {
            ownId = PhotonNetwork.player.ID;
            Debug.Log("Multi: Room joined! "+ownId);
        }
        _userConnected = true;
        UserConnected.Invoke();
    }

    public void MoveUser() {

    }



    private void Reset()
    {
        Debug.Log("Reset all");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected: " + player);
    }
}
