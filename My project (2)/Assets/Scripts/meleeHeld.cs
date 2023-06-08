using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHeld : MonoBehaviour
{
    public inventory Inventory;
    public LayerMask meleeMask;

    private void Update()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.parent.position.x)
        {
            transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize;
            transform.rotation = Quaternion.Euler(0, 0, -90);
            transform.position = new Vector2(transform.parent.position.x + transform.localScale.y / 2, transform.parent.position.y);
        }
        else
        {
            transform.localScale = new Vector2((Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea.x) + 0.5f, Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea.x + 0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.position = new Vector2(transform.parent.position.x - transform.localScale.y / 2, transform.parent.position.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] attackBox;

            if (Quaternion.ToEulerAngles(transform.rotation).z == -Mathf.PI / 2)
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x + (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea, 0, meleeMask);
            }
            else
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x - (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea, 0, meleeMask);
            }

            for (int i = 0; i < attackBox.Length; i++)
            {
                Destroy(attackBox[i].gameObject);
            }
        }
    }
}
