using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class explosionDamage : NetworkBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the shooter is protected from the explosion
        if (player != null)
        {
            if (player == collision.gameObject)
            {

            }
            else if (collision.GetComponent<Rigidbody2D>() != null)
            {

                //Vector2 nockback = new Vector2(9 * Mathf.Cos(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)), 9 * Mathf.Sin(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)));
                Vector2 nockback;
                if (collision.transform.position.x > player.transform.position.x)
                {
                    nockback = new Vector2(9, 0);
                }
                else
                {
                    nockback = new Vector2(-9, 0);
                }
                //print(nockback);
                if (collision.CompareTag("Player"))
                {
                    if (!IsOwner)
                    {
                        if (Object.FindObjectOfType<NetworkManager>().GetComponent<NetworkManager>().IsServer)
                        {
                            ClientRpcParams clientRpcParams = new ClientRpcParams
                            {
                                Send = new ClientRpcSendParams
                                {
                                    TargetClientIds = new ulong[] { collision.GetComponent<NetworkObject>().OwnerClientId }
                                    
                                }
                            };
                            collision.GetComponent<charmovement>().explodeClientRpc(nockback , clientRpcParams);
                        }
                    }
                    collision.GetComponent<playerHealth>().TakeDamage(5);
                    
                }
                collision.GetComponent<Rigidbody2D>().velocity = nockback;
            }
        }
        // if the shooter is not protected from the explosion
        else if (collision.GetComponent<Rigidbody2D>() != null)
        {
            Vector2 nockback = new Vector2(9 * Mathf.Cos(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)), 9 * Mathf.Sin(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)));
            //print(nockback);
            if (collision.CompareTag("Player"))
            {
                if (Object.FindObjectOfType<NetworkManager>().GetComponent<NetworkManager>().IsServer)
                {
                    ClientRpcParams clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { collision.GetComponent<NetworkObject>().OwnerClientId }

                        }
                    };
                    collision.GetComponent<charmovement>().explodeClientRpc(nockback, clientRpcParams);
                    collision.GetComponent<playerHealth>().TakeDamage(5);
                }
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().velocity = nockback;
            }
        }
    }

    
}
