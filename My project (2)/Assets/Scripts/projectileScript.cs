using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class projectileScript : NetworkBehaviour
{
    public NetworkVariable<float> damage = new NetworkVariable<float>();
    public NetworkVariable<GameObject> player = new NetworkVariable<GameObject>();
    NetworkVariable<bool> move = new NetworkVariable<bool>();
    move.Value = true;
    public NetworkVariable<float> explosionDelay;
    explosionDelay.Value = 0;
    public NetworkVariable<GameObject> explosion = new NetworkVariable<GameObject>();
    public NetworkVariable<bool> explodes = new NetworkVariable<bool>();
    explodes.Value = false;
    public NetworkVariable<itemClass> item = new NetworkVariable<itemClass>();

    private void Start()
    {
        if (IsServer)
        {
            transform.GetComponent<SpriteRenderer>().sprite = item.Value.projectileSprite;
            transform.GetComponent<SpriteRenderer>().color = item.Value.bulletColor;
            StartCoroutine(death());
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.Value.projectileSpeed * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), item.Value.projectileSpeed * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
        }
    }

    private void Update()
    {
        if (move.Value)
        {
            //transform.position = new Vector2(transform.position.x + item.projectileSpeed * Time.deltaTime * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), transform.position.y + item.projectileSpeed * Time.deltaTime * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(5);
        if (move.Value)
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
            if (explodes.Value)
            {
                StartCoroutine(explodeDelay(collision.gameObject));
            }
            else
            {
                if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<playerHealth>().TakeDamage(damage.Value);
                }
                if (IsOwnedByServer)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (!move.Value && collision.CompareTag("Player") && collision.gameObject != player)
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
        move.Value = false;
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
