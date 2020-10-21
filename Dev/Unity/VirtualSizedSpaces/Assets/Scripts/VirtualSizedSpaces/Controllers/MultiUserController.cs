using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUserController : MonoBehaviour
{
    private List<GameObject> _users = new List<GameObject>();
    void Start()
    {
        Debug.Log("start multiUserController");   
    }

    internal void AddUser(GameObject user)
    {
        _users.Add(user);
    }
}
