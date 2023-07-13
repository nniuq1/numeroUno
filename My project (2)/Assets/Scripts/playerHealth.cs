using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHealth : NetworkBehaviour
{
    public GameObject hamburguesaItem;
    public NetworkVariable<float> _netHealth = new NetworkVariable<float>();
    public float startHealth = 10;
    public float health;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "testing")
        {
            this.enabled = false;
        }
        else
        {
            Object.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.SetActive(true);
            if (IsServer)
            {
                health = startHealth;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsServer)
        {
            health -= damage;
        }
    }
    public void ResetHealth()
    {
        if (IsServer)
        {
            health = startHealth;
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
        }

        if (IsOwner)
        {
            Object.FindObjectOfType<Canvas>().transform.GetChild(3).GetChild(1).transform.localScale = new Vector2(_netHealth.Value / startHealth, 1);
            if (_netHealth.Value <= 0)
            {
                Vector2 deathpos = transform.position;
                //_netHealth.Value = startHealth;
                TestServerRpc(NetworkManager.Singleton.LocalClientId);
                health = startHealth;
                transform.position = Vector3.zero;
                for (int i = 0; i < GetComponent<inventory>().itemClasses.Count; i++)
                {
                    if (GetComponent<inventory>().itemClasses[i].weapontype == itemClass.WeaponType.Hamburguesa)
                    {
                        GetComponent<inventory>().itemClasses.RemoveAt(i);
                        if (i < GetComponent<inventory>().itemSelected.Value)
                        {
                            GetComponent<inventory>().itemSelected.Value--;
                        }
                        CreateHamburguesaServerRPC(deathpos, NetworkManager.Singleton.LocalClientId);
                    }
                }
            }
        }
    }

    [ServerRpc]
    void TestServerRpc(ulong clientId) {
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<playerHealth>().ResetHealth();
        
    }

    [ServerRpc]
    void CreateHamburguesaServerRPC(Vector2 pos, ulong player)
    {
        print("I am served");
        GameObject ham = Instantiate(hamburguesaItem, pos, Quaternion.Euler(0, 0, 0));
        ham.GetComponent<NetworkObject>().Spawn();
        //for (int i = 0; i < NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetComponent<inventory>().itemClasses.Count; i++)
        //{
            //if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetComponent<inventory>().itemClasses[i].weapontype == itemClass.WeaponType.Hamburguesa)
            //{
                //NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetComponent<inventory>().itemClasses.RemoveAt(i);
            //}
        //}
    }
}
