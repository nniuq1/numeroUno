using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class wandProjectile : NetworkBehaviour
{
    public int bounces = 5;
    float damage = 1;
    public ulong player;
    bool move = true;
    public itemClass item;
    NetworkVariable<Vector2> _netpos = new NetworkVariable<Vector2>();

    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(death());
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), item.projectileSpeed * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(25);
        if (move)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            _netpos.Value = transform.position;
        }
        else
        {
            transform.position = _netpos.Value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.transform.gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject && collision.gameObject.layer == 3 || collision.gameObject.layer == 0 || collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
            {
                if (bounces > 0)
                {
                    bounces--;
                    int random = (Random.Range(0, 1) * 2) - 1;
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Mathf.Atan2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y) + random * Mathf.PI), item.projectileSpeed * Mathf.Sin(Mathf.Atan2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y) + random * Mathf.PI));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            if (collision.CompareTag("Player") && collision.gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject)
            {
                collision.GetComponent<playerHealth>().TakeDamage(damage);
            }
        }
    }
}
