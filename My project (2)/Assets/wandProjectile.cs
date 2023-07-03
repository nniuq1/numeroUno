using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wandProjectile : MonoBehaviour
{
    public int bounces = 5;
    public float damage;
    public GameObject player;
    bool move = true;
    public float explosionDelay = 0;
    public GameObject explosion;
    public bool explodes = false;
    public itemClass item;

    private void Start()
    {
        StartCoroutine(death());
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), item.projectileSpeed * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(25);
        if (move)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject != player && collision.gameObject.layer == 3 || collision.gameObject.layer == 0 || collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
        {
            if (bounces > 0)
            {
                bounces--;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Mathf.Atan2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y) + Mathf.PI), item.projectileSpeed * Mathf.Sin(Mathf.Atan2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y) + Mathf.PI));
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Player") && collision.gameObject != player)
        {
            collision.GetComponent<playerHealth>().TakeDamage(damage);
        }
    }
}