using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using Object = UnityEngine.Object;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkDiscovery))]
public class VSNetworkDiscovery : MonoBehaviour
{
    public NetworkDiscovery networkDiscovery;
    public bool autoConnect = true;
    private bool connected = false;
        
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
        
#if !UNITY_ANDROID
    private void Start()
    {
        if (!autoConnect)
        {
            return;
        }
        NetworkManager.singleton.StartServer();
        networkDiscovery.AdvertiseServer();
    }
#endif

#if UNITY_ANDROID
    private void Start()
    {
        if (!autoConnect)
        {
            return;
        }
        networkDiscovery.StartDiscovery();
    }
#endif

    void Connect(ServerResponse info)
    {
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        if (connected)
        {
            return;
        }
        Connect(info);
        connected = true;
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        //discoveredServers[info.serverId] = info;
    }
}
