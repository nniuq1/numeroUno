using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class positionSharing : NetworkBehaviour
{
    public NetworkVariable<Vector2> _netPos = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            _netPos.Value = transform.position;
        }
        else
        {
            transform.position = _netPos.Value;
        }
    }
}
