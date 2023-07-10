using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class kaboomarangScript : NetworkBehaviour
{
    bool leaving = true;
    public GameObject explosion;
    public NetworkVariable<ulong> player = new NetworkVariable<ulong>();

    private void Start()
    {
        player.Value = 0;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(30 * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), 30 * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));
    }

    private void Update()
    {
        if (leaving)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.GetComponent<Rigidbody2D>().velocity.x - transform.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime * 1.25f, transform.GetComponent<Rigidbody2D>().velocity.y - transform.GetComponent<Rigidbody2D>().velocity.y * Time.deltaTime * 1.25f);
        }
        else
        {
            Vector3 dir = NetworkManager.Singleton.ConnectedClients[player.Value].PlayerObject.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(30 * Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), 30 * Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z));

            if (Vector2.Distance(transform.position, NetworkManager.Singleton.ConnectedClients[player.Value].PlayerObject.transform.position) < 0.4f)
            {
                Destroy(gameObject);
            }
        }

        if (Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.x) + Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.y) < 15 && leaving)
        {
            leaving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (leaving)
        {
            if (collision.gameObject != NetworkManager.Singleton.ConnectedClients[player.Value].PlayerObject.gameObject && collision.gameObject.layer == 3 || collision.gameObject.layer == 0 || collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
            {
                print(NetworkManager.Singleton.ConnectedClients[player.Value].PlayerObject.transform.position);
                leaving = false;
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }
    }
}
