using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerHealth : NetworkBehaviour
{
    public NetworkVariable<float> _netHealth = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    public float startHealth = 10;
    public float health;

    private void Start()
    {
        if (IsOwner)
        {
            health = startHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsServer)
        {
            health -= damage;
        }
    }

    void Update()
    {
        if (IsServer)
        {
            _netHealth.Value = health;
        }
        else
        {
            health = _netHealth.Value;
            print(health);
        }
    
    }
}
