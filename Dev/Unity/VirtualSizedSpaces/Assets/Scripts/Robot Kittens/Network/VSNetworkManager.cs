using System.Collections;
using Mirror;
using UnityEngine;


public class VSNetworkManager : NetworkManager
{
    private MultiUserController _controller;
    public static  MultiUserController _multiUserController;
    private MultiUserController Controller
    {
        get
        {
            if (!_controller)
            {
                _controller = FindObjectOfType<MultiUserController>();
            }

            _multiUserController = transform.Find("Stage").GetComponent<MultiUserController>();
            return _controller;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
       
            
        Transform startPos = GetStartPosition();
        GameObject user = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, user);
        _multiUserController.AddUser(user);


        StartCoroutine(DelayAddPlayer(3, user));
    }
        
    private IEnumerator DelayAddPlayer(float waitTime, GameObject user)
    {
        yield return new WaitForSeconds(waitTime);
            
        Controller.AddUser(user);
    }
}

