using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class projectileScript : NetworkBehaviour
{
    public float damage;
    public GameObject player;
    bool move = true;
    public float explosionDelay = 0;
    public GameObject explosion;
    public bool explodes = false;
    public itemClass item;

    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = item.projectileSprite;
        transform.GetComponent<SpriteRenderer>().color = item.bulletColor;
        StartCoroutine(death());
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), item.projectileSpeed * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
    }

    private void Update()
    {
        if (move)
        {
            //transform.position = new Vector2(transform.position.x + item.projectileSpeed * Time.deltaTime * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), transform.position.y + item.projectileSpeed * Time.deltaTime * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(5);
        if (move)
        {
            if (IsOwnedByServer)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject != player && collision.gameObject.layer == 3 || collision.gameObject.layer == 0 || collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
        {
            if (explodes)
            {
                StartCoroutine(explodeDelay(collision.gameObject));
            }
            else
            {
                if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<playerHealth>().TakeDamage(damage);
                }
                if (IsOwnedByServer)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (!move && collision.CompareTag("Player") && collision.gameObject != player)
        {
            Instantiate(explosion, transform.position, transform.rotation, null);
            if (IsOwnedByServer)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator explodeDelay(GameObject collision)
    {
        move = false;
        Destroy(transform.GetComponent<Rigidbody2D>());
        //transform.SetParent(collision.transform);
        yield return new WaitForSeconds(explosionDelay);
        Instantiate(explosion, transform.position, transform.rotation, null);
        if (IsOwnedByServer)
        {
            Destroy(gameObject);
        }
    }
}
