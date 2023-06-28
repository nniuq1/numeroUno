using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("projectile"))
        {
            if (explodes)
            {
                StartCoroutine(explodeDelay());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator explodeDelay()
    {
        move = false;
        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(explosionDelay);
        Instantiate(explosion, transform.position, transform.rotation, null);
        Destroy(gameObject);
    }
}
