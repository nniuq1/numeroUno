using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class inventory : MonoBehaviour
{
    GameObject inventoryUI;
    public List<itemCombo> combinations;
    public List<itemClass> itemClasses = new List<itemClass>();
    public float itemSelected = 0;

    private void Start()
    {
        inventoryUI = GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            itemSelected += Input.mouseScrollDelta.y * 2;
            if (itemSelected < 0)
            {
                itemSelected = itemClasses.Count - 0.001f;
            }
            if (itemSelected > itemClasses.Count - 0.0001f)
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

        if (itemClasses.Count > 0)
        {
            inventoryUI.SetActive(true);

            inventoryUI.transform.GetChild(3).GetComponent<Text>().text = itemClasses[(int)itemSelected].Name;


            inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected].sprite;

            if (itemClasses[(int)itemSelected].weaponSize.x > itemClasses[(int)itemSelected].weaponSize.y)
            {
                inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = itemClasses[(int)itemSelected].weaponSize/ itemClasses[(int)itemSelected].weaponSize.x * 0.8f;
            }
            else
            {
                inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = itemClasses[(int)itemSelected].weaponSize / itemClasses[(int)itemSelected].weaponSize.y * 0.8f;
            }

            if (itemClasses.Count > 1)
            {
                if ((int)itemSelected == itemClasses.Count - 1)
                {
                    inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[0].sprite;
                }
                else
                {
                    inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected + 1].sprite;
                }
            }
            else
            {
                inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected].sprite;
            }
            if (itemClasses.Count > 1)
            {
                if ((int)itemSelected == 0)
                {
                    inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[itemClasses.Count - 1].sprite;
                }
                else
                {
                    inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected - 1].sprite;
                }
            }
            else
            {
                inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected].sprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemClasses.Add(collision.transform.GetComponent<itemScript>().itemclass);
            CheckComboes();
        }
    }

    public void CheckComboes()
    {
        for (int c = 0; c < combinations.Count; c++)
        {
            bool makes = true;
            for (int i = 0; i < combinations[c].ingredients.Count; i++)
            {
                bool hasIt = false;
                for (int clas = 0; clas < itemClasses.Count; clas++)
                {
                    if (itemClasses[clas] == combinations[c].ingredients[i])
                    {
                        hasIt = true;
                    }
                }
                if (hasIt == false)
                {
                    makes = false;
                }
            }
            if (makes == true)
            {
                for (int i = 0; i < combinations[c].ingredients.Count; i++)
                {
                    itemClasses.Remove(combinations[c].ingredients[i]);
                }
                itemClasses.Add(combinations[c].result);
                CheckComboes();
            }
        }
    }
}
