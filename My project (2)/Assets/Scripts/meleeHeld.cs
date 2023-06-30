using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHeld : MonoBehaviour
{
    public Animator animator;
    public GameObject explosion;
    public inventory Inventory;
    public LayerMask meleeMask;

    private void Update()
    {
        transform.GetComponent<Animator>().enabled = true;
        if (transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x > transform.parent.position.x)
        {
            transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize;
            animator.SetBool("right", true);
            transform.position = new Vector2(transform.parent.position.x + transform.localScale.y / 2, transform.parent.position.y);
        }
        else
        {
            transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected].weaponSize;
            animator.SetBool("right", false);
            transform.position = new Vector2(transform.parent.position.x - transform.localScale.y / 2, transform.parent.position.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("attacking", true);
            StartCoroutine(stopAttacking());

            Collider2D[] attackBox;

            if (animator.GetBool("right") == true)
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x + (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea, 0, meleeMask);
            }
            else
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x - (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected].MeleeAtackArea, 0, meleeMask);
            }

            for (int i = 0; i < attackBox.Length; i++)
            {
                if (Inventory.itemClasses[(int)Inventory.itemSelected].explodes)
                {
                    if (animator.GetBool("right") == true)
                    {
                        GameObject explodesing = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
                        explodesing.GetComponent<explosionDamage>().player = transform.parent.parent.gameObject;
                    }
                    else
                    {
                        GameObject explodesing = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
                        explodesing.GetComponent<explosionDamage>().player = transform.parent.parent.gameObject;
                    }
                }
                if (!attackBox[i].CompareTag("Player"))
                {
                    Destroy(attackBox[i].gameObject);
                }
                else
                {
                    attackBox[i].GetComponent<playerHealth>().TakeDamage(Inventory.itemClasses[(int)Inventory.itemSelected].MeleeDamage);
                }
            }
        }
    }

    IEnumerator stopAttacking()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("attacking", false);
    }
}
