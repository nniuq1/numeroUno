using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerHealth : NetworkBehaviour
{
    public NetworkVariable<float> _netHealth = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Server);
    public float startHealth = 10;
    public float health;

    private void Start()
    {
        if (IsServer)
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
            TestClientRpc();
        }
        else
        {
            health = _netHealth.Value;
            print(_netHealth.Value);
        }
    
    }

    [ClientRpc]
    void TestClientRpc() {
        print("Ugly");
    }
}
