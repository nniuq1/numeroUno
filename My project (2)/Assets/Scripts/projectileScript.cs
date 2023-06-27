using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public GameObject explosion;
    public bool explodes = false;
    public itemClass item;

    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = item.projectileSprite;
        transform.GetComponent<SpriteRenderer>().color = item.bulletColor;
        StartCoroutine(death());
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + item.projectileSpeed * Time.deltaTime * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), transform.position.y + item.projectileSpeed * Time.deltaTime * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
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
                Instantiate(explosion, transform.position, transform.rotation, null);
            }
            Destroy(gameObject);
        }
    }
}
