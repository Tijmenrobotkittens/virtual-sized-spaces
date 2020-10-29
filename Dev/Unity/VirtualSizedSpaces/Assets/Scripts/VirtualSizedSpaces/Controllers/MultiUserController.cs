﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUserController : MonoBehaviour
{
    private List<GameObject> _users = new List<GameObject>();
    private NetworkDisable _networkDisable;
    private string _otherUserPrefab;
    private GameObject _parent;


    public void SetOtherUser(string prefab,GameObject parent) {
        _otherUserPrefab = prefab;
        _parent = parent;
    }

    void Start()
    {
        _networkDisable = gameObject.AddComponent<NetworkDisable>();
    }

    internal void AddUser(GameObject user)
    {
        _users.Add(user);
    }
}
