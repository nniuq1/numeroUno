using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class explosionDamage : NetworkBehaviour, IEnumerable
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null)
        {
            if (player == collision.gameObject)
            {

            }
            else if (collision.GetComponent<Rigidbody2D>() != null)
            {
                
                    Vector2 nockback = new Vector2(9 * Mathf.Cos(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)), 9 * Mathf.Sin(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)));
                
                if (collision.CompareTag("Player"))
                {
                    if (!IsOwner)
                    {
                        if (IsServer)
                        {
                            ClientRpcParams clientRpcParams = new ClientRpcParams { new ClientRpcSendParams { new ulong[] { collision.GetComponent<NetworkObject>().OwnerClientId } } };
                            explodeClientRpc(nockback , clientRpcParams);
                        }
                    }
                    else {
                        collision.GetComponent<Rigidbody2D>().velocity = nockback;
                        collision.GetComponent<charmovement>().Stun(0.75f);
                    }
                    collision.GetComponent<playerHealth>().TakeDamage(5);
                }
            }
        }
        else if (collision.GetComponent<Rigidbody2D>() != null)
        {
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(9 * Mathf.Cos(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)), 9 * Mathf.Sin(Mathf.Atan2(collision.transform.position.y - transform.position.y, collision.transform.position.x - transform.position.x)));

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<playerHealth>().TakeDamage(5);
                collision.GetComponent<charmovement>().Stun(0.75f);
            }
        }
    }
    [ClientRpc]
    public void explodeClientRpc(Vector2 nockback, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner) return;

        // Run your client-side logic here!!
        NetworkManager.LocalClient.PlayerObject.GetComponent<Rigidbody2D>().velocity = nockback;
    }
}
