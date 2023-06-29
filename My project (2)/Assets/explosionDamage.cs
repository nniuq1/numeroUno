using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<playerHealth>().TakeDamage(5);
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(9 * Mathf.Cos(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)), 9 * Mathf.Sin(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)));
            collision.GetComponent<charmovement>().Stun(0.75f);
        }
    }
}
