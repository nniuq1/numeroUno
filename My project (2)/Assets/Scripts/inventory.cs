using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class inventory : NetworkBehaviour
{
    GameObject inventoryUI;
    public List<itemCombo> combinations;
    public List<itemClass> itemClasses = new List<itemClass>();
    public NetworkVariable<float> itemSelected = new NetworkVariable<float>();

    private void Start()
    {
        itemSelected.Value = 0;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "testing")
        {
            inventoryUI = GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            itemSelected.Value += Input.mouseScrollDelta.y * 2;
            if (itemSelected.Value < 0)
            {
                itemSelected.Value = itemClasses.Count - 0.001f;
            }
            if (itemSelected.Value > itemClasses.Count - 0.0001f)
            {
                itemSelected.Value = 0;
            }
        }

        if (itemClasses.Count > 0 && IsOwner)
        {
            transform.GetChild(2).GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = itemClasses[(int)itemSelected.Value].sprite;
            if (itemClasses[(int)itemSelected.Value].weapontype == itemClass.WeaponType.Ranged)
            {
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).localScale = new Vector3(0.8f, 0.8f, 1);
                transform.GetChild(2).GetChild(0).position = transform.position;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = true;
            }
            else if (itemClasses[(int)itemSelected.Value].weapontype == itemClass.WeaponType.Melee)
            {
                transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = false;
                transform.GetChild(2).GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
                transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = true;
            }
            else if (itemClasses[(int)itemSelected.Value].weapontype == itemClass.WeaponType.Hamburguesa)
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

        if (IsOwner)
        {
            if (itemClasses.Count > 0)
            {
                inventoryUI.SetActive(true);
                inventoryUI.transform.GetChild(0).gameObject.SetActive(true);

                inventoryUI.transform.GetChild(3).GetComponent<Text>().text = itemClasses[(int)itemSelected.Value].Name;


                inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected.Value].sprite;

                if (itemClasses[(int)itemSelected.Value].weaponSize.x > itemClasses[(int)itemSelected.Value].weaponSize.y)
                {
                    inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = itemClasses[(int)itemSelected.Value].weaponSize / itemClasses[(int)itemSelected.Value].weaponSize.x * 0.8f;
                }
                else
                {
                    inventoryUI.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = itemClasses[(int)itemSelected.Value].weaponSize / itemClasses[(int)itemSelected.Value].weaponSize.y * 0.8f;
                }

                if (itemClasses.Count > 1)
                {
                    inventoryUI.transform.GetChild(1).gameObject.SetActive(true);
                    if ((int)itemSelected.Value == itemClasses.Count - 1)
                    {
                        inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[0].sprite;
                    }
                    else
                    {
                        inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected.Value + 1].sprite;
                    }
                }
                else
                {
                    inventoryUI.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected.Value].sprite;
                }
                if (itemClasses.Count > 2)
                {
                    inventoryUI.transform.GetChild(2).gameObject.SetActive(true);
                    if ((int)itemSelected.Value == 0)
                    {
                        inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[itemClasses.Count - 1].sprite;
                    }
                    else
                    {
                        inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected.Value - 1].sprite;
                    }
                }
                else
                {
                    inventoryUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = itemClasses[(int)itemSelected.Value].sprite;
                }
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
                Object.FindObjectOfType<Canvas>().transform.GetChild(4).gameObject.active = true;
                Object.FindObjectOfType<Canvas>().transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().sprite = combinations[c].ingredients[0].sprite;
                Object.FindObjectOfType<Canvas>().transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Image>().sprite = combinations[c].ingredients[1].sprite;
                Object.FindObjectOfType<Canvas>().transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Image>().sprite = combinations[c].result.sprite;
                Object.FindObjectOfType<Canvas>().transform.GetChild(4).GetComponent<Animator>().SetBool("combotime", true);
                StartCoroutine(stopCombo());
                for (int i = 0; i < combinations[c].ingredients.Count; i++)
                {
                    itemClasses.Remove(combinations[c].ingredients[i]);
                }
                itemClasses.Add(combinations[c].result);
                CheckComboes();
            }
        }
    }

    IEnumerator stopCombo()
    {
        yield return new WaitForSeconds(1f);
        Object.FindObjectOfType<Canvas>().transform.GetChild(4).GetComponent<Animator>().SetBool("combotime", false);
    }
}
