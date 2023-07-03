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
        Object.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.SetActive(true);
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
        }
        else
        {
            health = _netHealth.Value;
            print(_netHealth.Value);
        }

        if (IsOwner)
        {
            Object.FindObjectOfType<Canvas>().transform.GetChild(3).GetChild(1).transform.localScale = new Vector2(_netHealth.Value / startHealth, 1);
            if (_netHealth.Value <= 0)
            {
                TakeDamage(-startHealth);
                TestServerRpc();
                transform.position = Vector3.zero;
            }
        }
    }

    [ServerRpc]
    void TestServerRpc() {
        _netHealth.Value = startHealth;
    }
}
