using System;
using Mirror;
using UnityEngine;

public class NetworkDisable : NetworkBehaviour
{
    [SerializeField]
    private MonoBehaviour[] scripts;
    [SerializeField] 
    private Renderer[] renderers;
    [SerializeField] 
    private Collider[] colliders;

    private bool? queueEnabled = null;
        
    public void Disable()
    {
        SetEnabled(false);
    }
        
    public void Enable()
    {
        SetEnabled(true);
    }

    public void SetEnabled(bool enable)
    {
        if (isServer)
        {
            RpcSetEnabled(enable);
            Enable(enable);
        }
        else
        {
            CmdSetEnabled(enable);
        }
    }
        
    [Command (ignoreAuthority = true)]
    public void CmdSetEnabled(bool enable)
    {
        RpcSetEnabled(enable);
        Enable(enable);
    }
        
    [ClientRpc]
    public void RpcSetEnabled(bool enable)
    {
        if (!isServer)
        {
            return;
        }
        Enable(enable);
    }

    private void Enable(bool enable)
    {
        if (!NetworkClient.active)
        {
            queueEnabled = enable;
            return;
        }
            
        foreach (MonoBehaviour monoBehaviour in scripts)
        {
            monoBehaviour.enabled = enable;
        }

        foreach (Renderer otherRenderer in renderers)
        {
            otherRenderer.enabled = enable;
        }

        foreach (Collider otherCollider in colliders)
        {
            otherCollider.enabled = enable;
        }
    }

    private void Update()
    {
        if (queueEnabled == null || !NetworkClient.active)
        {
            return;
        }
            
        Enable((bool)queueEnabled);
        queueEnabled = null;
    }
}
