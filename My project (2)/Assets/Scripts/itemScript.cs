using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    public List<itemClass> itemclasses;
    public itemClass itemclass;
    public float spinSpeed = 1;
    bool spinDir = true;

    private void Start()
    {
        if (itemclass == null)
        {
            itemclass = itemclasses[Random.Range(0, itemclasses.Count)];
        }
        transform.GetComponent<SpriteRenderer>().sprite = itemclass.sprite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemclass.sprite;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = itemclass.sprite;
        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = itemclass.sprite;
        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = itemclass.sprite;
        transform.localScale = itemclass.weaponSize;
    }

    private void Update()
    {
        if (spinDir)
        {
            if (transform.localScale.x <= -itemclass.weaponSize.x)
            {
                spinDir = false;
            }
            transform.localScale = new Vector2(transform.localScale.x - spinSpeed * Time.deltaTime, transform.localScale.y);
        }
        else
        {
            if (transform.localScale.x >= itemclass.weaponSize.x)
            {
                spinDir = true;
            }
            transform.localScale = new Vector2(transform.localScale.x + spinSpeed * Time.deltaTime, transform.localScale.y);
        }
    }

    private void OnDestroy()
    {
        if (itemclass.Name == "La Hamburguesa")
        {
            print("ow");
        }
    }
}
