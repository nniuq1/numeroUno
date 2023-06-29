using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public NetworkVariable<float> _netHealth = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    public float startHealth = 10;

    private void Start()
    {
        _netHealth.Value = startHealth;
    }

    public void TakeDamage(float damage)
    {
        _netHealth.Value -= damage;
        print(_netHealth.Value);
    }
}
