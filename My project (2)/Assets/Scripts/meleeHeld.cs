using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class meleeHeld : NetworkBehaviour
{
    public GameObject objectBreaking;
    bool canHit = true;
    public Animator animator;
    public GameObject explosion;
    public inventory Inventory;
    public LayerMask meleeMask;
    NetworkVariable<bool> rightFacing = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
    int catHitCombo = 0;
    int catHitRequired = 4;
    public GameObject cat;
    float comboTimer = 0;

    private void Update()
    {
        transform.GetComponent<Animator>().enabled = true;

        if (IsOwner)
        {
            if (transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x > transform.parent.position.x)
            {
                rightFacing.Value = true;
                transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize;
                animator.SetBool("right", true);
                transform.position = new Vector2(transform.parent.position.x + Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea.x / 2, transform.parent.position.y);
                transform.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                rightFacing.Value = false;
                transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize;
                animator.SetBool("right", false);
                transform.position = new Vector2(transform.parent.position.x - Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea.x / 2, transform.parent.position.y);
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (rightFacing.Value)
            {
                transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize;
                animator.SetBool("right", true);
                transform.position = new Vector2(transform.parent.position.x + Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea.x / 2, transform.parent.position.y);
                transform.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                transform.localScale = Inventory.itemClasses[(int)Inventory.itemSelected.Value].weaponSize;
                animator.SetBool("right", false);
                transform.position = new Vector2(transform.parent.position.x - Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea.x / 2, transform.parent.position.y);
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        if (IsOwner)
        {
            if (IsServer)
            {
                if (Input.GetMouseButtonDown(0) && canHit && !Inventory.itemClasses[(int)Inventory.itemSelected.Value].canHoldDown || Input.GetMouseButton(0) && canHit && Inventory.itemClasses[(int)Inventory.itemSelected.Value].canHoldDown)
                {
                    animationClientRPC(NetworkManager.Singleton.LocalClientId);
                    StartCoroutine(timeBetweenHits());
                    animator.speed = 1f / Inventory.itemClasses[(int)Inventory.itemSelected.Value].attackSpeed;
                    animator.SetBool("attacking", true);
                    StartCoroutine(stopAttacking());

                    Collider2D[] attackBox;

                    if (animator.GetBool("right") == true)
                    {
                        attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x + (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea, 0, meleeMask);
                    }
                    else
                    {
                        attackBox = Physics2D.OverlapBoxAll(new Vector2(transform.parent.position.x - (transform.localScale.x + 1.55f) / 2, transform.parent.position.y), Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeAtackArea, 0, meleeMask);
                    }

                    for (int i = 0; i < attackBox.Length; i++)
                    {
                        if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].explodes)
                        {
                            if (animator.GetBool("right") == true)
                            {
                                GameObject explodesing = Instantiate(explosion, new Vector3(transform.position.x + 1.75f, transform.position.y, 0), Quaternion.Euler(0, 0, 0));
                                explodesing.GetComponent<explosionDamage>().player = transform.parent.parent.gameObject;
                                explodesing.GetComponent<NetworkObject>().Spawn();
                            }
                            else
                            {
                                GameObject explodesing = Instantiate(explosion, new Vector3(transform.position.x - 1.75f, transform.position.y, 0), Quaternion.Euler(0, 0, 0));
                                explodesing.GetComponent<explosionDamage>().player = transform.parent.parent.gameObject;
                                explodesing.GetComponent<NetworkObject>().Spawn();
                            }
                        }
                        bool hiting = false;
                        if (!attackBox[i].CompareTag("Player"))
                        {
                            Instantiate(objectBreaking, attackBox[i].transform.position, attackBox[i].transform.rotation);
                            Destroy(attackBox[i].gameObject);
                            hiting = true;
                        }
                        else if (attackBox[i].gameObject != transform.parent.parent.gameObject)
                        {
                            attackBox[i].GetComponent<playerHealth>().TakeDamage(Inventory.itemClasses[(int)Inventory.itemSelected.Value].MeleeDamage);
                            hiting = true;
                        }
                        if (hiting && Inventory.itemClasses[(int)Inventory.itemSelected.Value].Name == "Excalipurr")
                        {
                            catHitCombo++;
                            comboTimer = 1.75f;
                        }
                    }
                }

            }
            else
            {
                if (Input.GetMouseButtonDown(0) && canHit && !Inventory.itemClasses[(int)Inventory.itemSelected.Value].canHoldDown || Input.GetMouseButton(0) && canHit && Inventory.itemClasses[(int)Inventory.itemSelected.Value].canHoldDown)
                {
                    StartCoroutine(timeBetweenHits());
                    animator.speed = 1f / Inventory.itemClasses[(int)Inventory.itemSelected.Value].attackSpeed;
                    animator.SetBool("attacking", true);
                    StartCoroutine(stopAttacking());
                    animationServerRPC(NetworkManager.Singleton.LocalClientId, animator.GetBool("right"));
                }
            }

            if (Inventory.itemClasses[(int)Inventory.itemSelected.Value].Name == "Excalipurr")
            {
                if (catHitCombo > 0)
                {
                    comboTimer -= Time.deltaTime;
                    //print(comboTimer);
                    if (comboTimer <= 0)
                    {
                        catHitCombo = 0;
                    }
                    if (catHitCombo >= catHitRequired)
                    {
                        CatSummonServerRPC();
                        catHitCombo = 0;
                    }
                }

            }
            else
            {
                catHitCombo = 0;
                comboTimer = 0;
            }
        }
    }

    IEnumerator timeBetweenHits()
    {
        canHit = false;
        yield return new WaitForSeconds(Inventory.itemClasses[(int)Inventory.itemSelected.Value].attackSpeed + Inventory.itemClasses[(int)Inventory.itemSelected.Value].timeBetweenMeleeAtack - 0.01f);
        canHit = true;
    }

    IEnumerator stopAttacking()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("attacking", false);
    }

    [ClientRpc]
    void animationClientRPC(ulong player)
    {

    }

    [ServerRpc]
    void animationServerRPC(ulong player, bool right)
    {
            animationClientRPC(NetworkManager.Singleton.LocalClientId);
            StartCoroutine(timeBetweenHits());
            animator.speed = 1f / NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].attackSpeed;
            animator.SetBool("attacking", true);
            StartCoroutine(stopAttacking());

            Collider2D[] attackBox;

            if (right)
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.x + (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetChild(2).GetChild(0).localScale.x + 1.55f) / 2, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.y), NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].MeleeAtackArea, 0, meleeMask);
            }
            else
            {
                attackBox = Physics2D.OverlapBoxAll(new Vector2(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.x - (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.GetChild(2).GetChild(0).localScale.x + 1.55f) / 2, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.y), NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].MeleeAtackArea, 0, meleeMask);
            }

            for (int i = 0; i < attackBox.Length; i++)
            {
                if (NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].explodes)
                {
                    if (animator.GetBool("right") == true)
                    {
                        GameObject explodesing = Instantiate(explosion, new Vector3(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.x + 1.75f, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.y, 0), Quaternion.Euler(0, 0, 0));
                        explodesing.GetComponent<explosionDamage>().player = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject;
                    explodesing.GetComponent<NetworkObject>().Spawn();
                }
                    else
                    {
                        GameObject explodesing = Instantiate(explosion, new Vector3(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.x - 1.75f, NetworkManager.Singleton.ConnectedClients[player].PlayerObject.transform.position.y, 0), Quaternion.Euler(0, 0, 0));
                        explodesing.GetComponent<explosionDamage>().player = NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject;
                    explodesing.GetComponent<NetworkObject>().Spawn();
                }
                }
            bool hiting = false;
                if (!attackBox[i].CompareTag("Player"))
                {
                    Instantiate(objectBreaking, attackBox[i].transform.position, attackBox[i].transform.rotation);
                    Destroy(attackBox[i].gameObject);
                hiting = true;
                }
                else if (attackBox[i].gameObject != NetworkManager.Singleton.ConnectedClients[player].PlayerObject.gameObject)
                {
                    attackBox[i].GetComponent<playerHealth>().TakeDamage(NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemClasses[(int)NetworkManager.Singleton.ConnectedClients[player].PlayerObject.GetComponent<inventory>().itemSelected.Value].MeleeDamage);
                hiting = true;
                }
            if (hiting)
            {
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { player }

                    }
                };
                CatClientRPC(clientRpcParams);
            }
        }
    }

    [ClientRpc]
    void CatClientRPC(ClientRpcParams clientRpcParams = default)
    {
        catHitCombo++;
        comboTimer = 1.75f;
    }

    [ServerRpc]
    void CatSummonServerRPC()
    {
        GameObject catObject = Instantiate(cat, transform.position, new Quaternion(0, 0, 0, 0));
        catObject.GetComponent<catScript>().player = transform.parent.parent.gameObject;
        catObject.GetComponent<NetworkObject>().Spawn();
    }
}
