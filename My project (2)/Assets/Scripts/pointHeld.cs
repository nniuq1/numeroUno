using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointHeld : MonoBehaviour
{
    bool canShoot = true;
    public inventory Inventory;
    public GameObject projectile;

    private void Update()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
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
            if (Input.GetMouseButton(0) && canShoot)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                GameObject newBullet = Instantiate(projectile, transform.position, transform.rotation);
                newBullet.GetComponent<projectileScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].bulletSize;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                canShoot = false;
                StartCoroutine(timeBetween());
                GameObject newBullet = Instantiate(projectile, transform.position, transform.rotation);
                newBullet.GetComponent<projectileScript>().item = Inventory.itemClasses[(int)Inventory.itemSelected];
                newBullet.transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].bulletSize;
            }
        }
    }

    IEnumerator timeBetween()
    {
        yield return new WaitForSeconds(Inventory.itemClasses[(int)Inventory.itemSelected].timeBetweenShots);
        canShoot = true;
    }
}
