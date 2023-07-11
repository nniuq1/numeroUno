using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class projectileScript : NetworkBehaviour
{
    public float damage;
    //public NetworkVariable<GameObject> player = new NetworkVariable<GameObject>();
    public ulong player;
    bool move;
    public float explosionDelay;
    public GameObject explosion;
    public bool explodes;
    itemClass item;
    public NetworkVariable<Vector3> _netpos = new NetworkVariable<Vector3>();
    public List<itemClass> itemClasses;
    public NetworkVariable<int> itemselect = new NetworkVariable<int>();


    private void Start()
    {
        item = itemClasses[itemselect.Value];

        move = true;
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
        if (move)
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

            if (!move && collision.CompareTag("Player") && collision.gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject)
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
        move = false;
        Destroy(transform.GetComponent<Rigidbody2D>());
        //transform.SetParent(collision.transform);
        yield return new WaitForSeconds(explosionDelay);
        if (IsOwnedByServer)
        {
            GameObject explodingson = Instantiate(explosion, transform.position, transform.rotation, null);
            explodingson.GetComponent<NetworkObject>().Spawn();
            Destroy(gameObject);
        }
    }
}
