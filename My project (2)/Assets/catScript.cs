using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class catScript : NetworkBehaviour
{
    public float speed = 10;
    public GameObject player;
    public float duration = 10;
    public float damage = 3;
    bool goRight = true;

    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(TimetoCheck());
            StartCoroutine(destroy());
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            if (goRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
            }
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator TimetoCheck()
    {
        CheckDirection();
        yield return new WaitForSeconds(1);
        StartCoroutine(TimetoCheck());
    }

    void CheckDirection()
    {
        GameObject targetPlayer = null;
        float distanceToPlayer = 100000;
        charmovement[] players = Object.FindObjectsOfType<charmovement>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gameObject != player)
            {
                if (Vector2.Distance(transform.position, players[i].transform.position) < distanceToPlayer)
                {
                    distanceToPlayer = Vector2.Distance(transform.position, players[i].transform.position);
                    targetPlayer = players[i].gameObject;
                }
            }
        }
        if (targetPlayer != null)
        {
            if (targetPlayer.transform.position.x > transform.position.x)
            {
                goRight = true;
            }
            else
            {
                goRight = false;
            }
        }
        else
        {
            goRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.layer == 3 && collision.gameObject != player)
            {
                collision.GetComponent<playerHealth>().TakeDamage(damage);
            }
        }
    }
}
