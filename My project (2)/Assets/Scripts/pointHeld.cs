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

    private void Update()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        transform.GetComponent<Animator>().enabled = false;
        Vector3 dir = transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
                        newBullet.GetComponent<projectileScript>().item.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value];
                        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].explodes)
                        {
                            newBullet.GetComponent<projectileScript>().explodes.Value = true;
                            newBullet.GetComponent<projectileScript>().explosionDelay.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].explosionDelay;
                        }
                        newBullet.GetComponent<projectileScript>().player.Value = transform.parent.parent.gameObject;
                        newBullet.GetComponent<projectileScript>().damage.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletDamage;
                    }
                    else if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].projectile.GetComponent<wandProjectile>() != null)
                    {
                        newBullet.GetComponent<wandProjectile>().item = Inventory.itemClasses[(int)Inventory.itemSelected.Value];
                        newBullet.GetComponent<wandProjectile>().player = transform.parent.parent.gameObject;
                        newBullet.GetComponent<wandProjectile>().damage = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletDamage;
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
                        newBullet.GetComponent<projectileScript>().item.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value];
                        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].explodes)
                        {
                            newBullet.GetComponent<projectileScript>().explodes.Value = true;
                            newBullet.GetComponent<projectileScript>().explosionDelay.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].explosionDelay;
                        }
                        newBullet.GetComponent<projectileScript>().player.Value = transform.parent.parent.gameObject;
                        newBullet.GetComponent<projectileScript>().damage.Value = Inventory.itemClasses[(int)Inventory.itemSelected.Value].bulletDamage;
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
        GameObject newBullet = Instantiate(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile, transform.position, transform.rotation);
        if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile.GetComponent<projectileScript>() != null)
        {
            newBullet.GetComponent<projectileScript>().item.Value = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value];
            if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].explodes)
            {
                newBullet.GetComponent<projectileScript>().explodes.Value = true;
                newBullet.GetComponent<projectileScript>().explosionDelay.Value = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].explosionDelay;
            }
            newBullet.GetComponent<projectileScript>().player.Value = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.parent.parent.gameObject;
            newBullet.GetComponent<projectileScript>().damage.Value = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].bulletDamage;
        }
        else if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].projectile.GetComponent<wandProjectile>() != null)
        {
            newBullet.GetComponent<wandProjectile>().item = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value];
            newBullet.GetComponent<wandProjectile>().player = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.parent.parent.gameObject;
            newBullet.GetComponent<wandProjectile>().damage = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].bulletDamage;
        }
        newBullet.transform.localScale = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].bulletSize;
        newBullet.GetComponent<Rigidbody2D>().gravityScale = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].gravityScale;
        newBullet.GetComponent<NetworkObject>().Spawn();
    }
}
