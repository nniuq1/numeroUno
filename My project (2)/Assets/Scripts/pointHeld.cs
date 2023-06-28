using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class pointHeld : NetworkBehaviour
{
    bool canShoot = true;
    public inventory Inventory;
    public GameObject projectile;

    private void Update()
    {
        transform.GetComponent<Animator>().enabled = false;
        Vector3 dir = transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Quaternion.ToEulerAngles(transform.rotation).z < Mathf.PI / 2 && Quaternion.ToEulerAngles(transform.rotation).z > -Mathf.PI / 2)
        {
            transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize;
        }
        else
        {
            transform.localScale = new Vector3(Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize.x, -Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize.y, 1);
        }

        if (Inventory.itemClasses[(int)Inventory.itemSelected].canHoldDown)
        {
            if (Input.GetMouseButton(0) && canShoot && IsOwner)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                GameObject newBullet = Instantiate(Inventory.itemClasses[(int)Inventory.itemSelected].projectile, transform.position, transform.rotation);
                if (Inventory.itemClasses[(int)Inventory.itemSelected].projectile.GetComponent<projectileScript>() != null)
                {
                    newBullet.GetComponent<projectileScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                    if (Inventory.itemClasses[(int)Inventory.itemSelected].explodes)
                    {
                        newBullet.GetComponent<projectileScript>().explodes = true;
                        newBullet.GetComponent<projectileScript>().explosionDelay = Inventory.itemClasses[(int)Inventory.itemSelected].explosionDelay;
                    }
                    newBullet.GetComponent<projectileScript>().player = transform.parent.parent.gameObject;
                }
                else
                {
                    newBullet.GetComponent<kaboomarangScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                }
                newBullet.GetComponent<kaboomarangScript>().player = transform.parent.parent.gameObject;
                newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].bulletSize;
                newBullet.GetComponent<Rigidbody2D>().gravityScale = Inventory.itemClasses[(int)Inventory.itemSelected].gravityScale;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && canShoot && IsOwner)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                GameObject newBullet = Instantiate(Inventory.itemClasses[(int)Inventory.itemSelected].projectile, transform.position, transform.rotation);
                if (Inventory.itemClasses[(int)Inventory.itemSelected].projectile.GetComponent<projectileScript>() != null)
                {
                    newBullet.GetComponent<projectileScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                    if (Inventory.itemClasses[(int)Inventory.itemSelected].explodes)
                    {
                        newBullet.GetComponent<projectileScript>().explodes = true;
                        newBullet.GetComponent<projectileScript>().explosionDelay = Inventory.itemClasses[(int)Inventory.itemSelected].explosionDelay;
                    }
                    newBullet.GetComponent<projectileScript>().player = transform.parent.parent.gameObject;
                }
                else
                {
                    newBullet.GetComponent<kaboomarangScript>().player = transform.parent.parent.gameObject;
                    newBullet.GetComponent<kaboomarangScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                }
                newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].bulletSize;
                newBullet.GetComponent<Rigidbody2D>().gravityScale = Inventory.itemClasses[(int)Inventory.itemSelected].gravityScale;
            }
        }
    }

    IEnumerator timeBetween()
    {
        yield return new WaitForSeconds(Inventory.itemClasses[(int)Inventory.itemSelected].timeBetweenShots);
        canShoot = true;
    }
}
