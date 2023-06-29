using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerHealth : NetworkBehaviour
{
    public NetworkVariable<float> _netHealth = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    public float startHealth = 10;

    private void Start()
    {
        if (IsOwner)
        {
            _netHealth.Value = startHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        print("owie wowie");
        _netHealth.Value -= damage;
    }

    void Update()
    {
        if (!IsOwner)
        {
            print(_netHealth.Value);
        }
    
    }
}
