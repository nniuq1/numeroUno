using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class projectileScript : NetworkBehaviour
{
    public NetworkVariable<float> damage = new NetworkVariable<float>();
    //public NetworkVariable<GameObject> player = new NetworkVariable<GameObject>();
    public ulong player;
    NetworkVariable<bool> move = new NetworkVariable<bool>();
    public NetworkVariable<float> explosionDelay;
    public GameObject explosion;
    public NetworkVariable<bool> explodes = new NetworkVariable<bool>();
    public itemClass item;
    public NetworkVariable<Vector3> _netpos = new NetworkVariable<Vector3>();

    private void Start()
    {
        move.Value = true;
        //if (IsServer)
        //{
            transform.GetComponent<SpriteRenderer>().sprite = item.projectileSprite;
            transform.GetComponent<SpriteRenderer>().color = item.bulletColor;
            StartCoroutine(death());
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(item.projectileSpeed * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), item.projectileSpeed * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
        //}
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

    IEnumerator death()
    {
        yield return new WaitForSeconds(5);
        if (move.Value)
        {
            if (IsServer)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.transform.gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject && collision.gameObject.layer == 3 || collision.gameObject.layer == 0 || collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
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

            if (!move.Value && collision.CompareTag("Player") && collision.gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject)
            {
                if (IsOwnedByServer)
                {
                    GameObject explodingson = Instantiate(explosion, transform.position, transform.rotation, null);
                    explodingson.GetComponent<NetworkObject>().Spawn();
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator explodeDelay(GameObject collision)
    {
        move.Value = false;
        Destroy(transform.GetComponent<Rigidbody2D>());
        //transform.SetParent(collision.transform);
        yield return new WaitForSeconds(explosionDelay.Value);
        if (IsOwnedByServer)
        {
            GameObject explodingson = Instantiate(explosion, transform.position, transform.rotation, null);
            explodingson.GetComponent<NetworkObject>().Spawn();
            Destroy(gameObject);
        }
    }
}
