using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public List<itemClass> itemClasses = new List<itemClass>();
    public float itemSelected = 0;

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            itemSelected += Input.mouseScrollDelta.y;
            if (itemSelected < 0)
            {
                itemSelected = itemClasses.Count - 0.001f;
            }
            if (itemSelected > itemClasses.Count - 0.001f)
            {
                itemSelected = 0;
            }
        }

        if (itemClasses.Count > 0)
        {
            transform.GetChild(2).GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = itemClasses[(int)itemSelected].sprite;
            if (itemClasses[(int)itemSelected].weapontype == itemClass.WeaponType.Ranged)
            {
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).localScale = new Vector3(0.8f, 0.8f, 1);
                transform.GetChild(2).GetChild(0).position = transform.position;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = true;
            }
            else if (itemClasses[(int)itemSelected].weapontype == itemClass.WeaponType.Melee)
            {
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = true;
            }
            else if (itemClasses[(int)itemSelected].weapontype == itemClass.WeaponType.Hamburguesa)
            {
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).localScale = new Vector3(0.8f, 0.8f, 1);
                transform.GetChild(2).GetChild(0).position = transform.position;
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = true;
            }
            else
            {
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).localScale = new Vector3(0.8f, 0.8f, 1);
                transform.GetChild(2).GetChild(0).position = transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemClasses.Add(collision.transform.GetComponent<itemScript>().itemclass);
        }
    }
}
