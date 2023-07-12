using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.Networking;

public class pointHeld : NetworkBehaviour
{
    bool canShoot = true;
    public inventory Inventory;
    public GameObject projectile;
    public NetworkVariable<float> _rotations = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);

    private void Start()
    {
        GetComponent<Animator>().enabled = false;
    }

    private void Update()
    {
        if (IsOwner)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.GetComponent<Animator>().enabled = false;
            Vector3 dir = transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _rotations.Value = angle;
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(_rotations.Value, Vector3.forward);
        }

        if (Quaternion.ToEulerAngles(transform.rotation).z < Mathf.PI / 2 && Quaternion.ToEulerAngles(transform.rotation).z > -Mathf.PI / 2)
        {
            transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize;
        }
        else
        {
            transform.localScale = new Vector3(Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize.x, -Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize.y, 1);
        }

        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].canHoldDown)
        {
            if (Input.GetMouseButton(0) && canShoot && IsOwner)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                if (IsServer)
                {
                    GameObject newBullet = Instantiate(Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile, transform.position, transform.rotation);
                    if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile.GetComponent<projectileScript>() != null)
                    {
                        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].explodes)
                        {
                            newBullet.GetComponent<projectileScript>().explodes = true;
                            newBullet.GetComponent<projectileScript>().explosionDelay = Inventory.itemClasses[(int)Inventory.itemSelected.Value].explosionDelay;
                        }
                        newBullet.GetComponent<projectileScript>().player = NetworkManager.Singleton.LocalClientId;
                        newBullet.GetComponent<projectileScript>().damage = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletDamage;
                        newBullet.GetComponent<projectileScript>().itemselect.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].itemID;
                    }
                    else if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile.GetComponent<wandProjectile>() != null)
                    {
                        newBullet.GetComponent<wandProjectile>().player = NetworkManager.Singleton.LocalClientId;
                    }
                    else
                    {
                        newBullet.GetComponent<kaboomarangScript>().player = NetworkManager.Singleton.LocalClientId;
                    }
                    newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletSize;
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].gravityScale;
                    newBullet.GetComponent<NetworkObject>().Spawn();
                }
                else
                {
                    spawningServerRpc(NetworkManager.Singleton.LocalClientId);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && canShoot && IsOwner)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                if (IsServer)
                {
                    GameObject newBullet = Instantiate(Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile, transform.position, transform.rotation);
                    if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile.GetComponent<projectileScript>() != null)
                    {
                        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].explodes)
                        {
                            newBullet.GetComponent<projectileScript>().explodes = true;
                            newBullet.GetComponent<projectileScript>().explosionDelay = Inventory.itemClasses[(int)Inventory.itemSelected.Value].explosionDelay;
                        }
                        newBullet.GetComponent<projectileScript>().player = NetworkManager.Singleton.LocalClientId;
                        newBullet.GetComponent<projectileScript>().damage = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletDamage;
                        newBullet.GetComponent<projectileScript>().itemselect.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].itemID;
                    }
                    else
                    {
                        newBullet.GetComponent<kaboomarangScript>().player = NetworkManager.Singleton.LocalClientId;
                    }
                    newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletSize;
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].gravityScale;
                    newBullet.GetComponent<NetworkObject>().Spawn();
                }
                else
                {
                    spawningServerRpc(NetworkManager.Singleton.LocalClientId);
                }
            }
        }
    }
    

    IEnumerator timeBetween()
    {
        yield return new WaitForSeconds(Inventory.itemClasses[(int)Inventory.itemSelected.Value].timeBetweenShots);
        canShoot = true;
    }

    [ServerRpc]
    void spawningServerRpc(ulong player)
    {
        GameObject newBullet = Instantiate(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetChild(2).GetChild(0).position, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetChild(2).GetChild(0).rotation);
        if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile.GetComponent<projectileScript>() != null)
        {
            if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].explodes)
            {
                newBullet.GetComponent<projectileScript>().explodes = true;
                newBullet.GetComponent<projectileScript>().explosionDelay = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].explosionDelay;
            }
            newBullet.GetComponent<projectileScript>().player = player;
            newBullet.GetComponent<projectileScript>().damage = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].bulletDamage;
            newBullet.GetComponent<projectileScript>().itemselect.Value = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].itemID;
        }
        else if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile.GetComponent<wandProjectile>() != null)
        {
            newBullet.GetComponent<wandProjectile>().player = player;
        }
        else
        {
            newBullet.GetComponent<kaboomarangScript>().player = player;
        }
        newBullet.transform.localScale = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].bulletSize;
        newBullet.GetComponent<Rigidbody2D>().gravityScale = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].gravityScale;
        newBullet.GetComponent<NetworkObject>().Spawn();
    }
}
