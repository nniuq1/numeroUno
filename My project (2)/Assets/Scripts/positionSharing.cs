using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class positionSharing : NetworkBehaviour
{
    public NetworkVariable<Vector3> _netPos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);

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
