using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class colorRandom : NetworkBehaviour
{
    public NetworkVariable<Color> _netColor = new NetworkVariable<Color>(writePerm: NetworkVariableWritePermission.Owner);
    private bool t = true;

    void Start()
    {
        if (IsOwner)
        {
            _netColor.Value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            transform.GetComponent<SpriteRenderer>().color = _netColor.Value;
        }
    }
    void Update()
    {
        if (!IsOwner && t && _netColor.Value != new Color(0, 0, 0, 0))
        {
            transform.GetComponent<SpriteRenderer>().color = _netColor.Value;
            t = false;
        }
    }
}
