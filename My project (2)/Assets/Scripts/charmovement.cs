using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class charmovement : NetworkBehaviour
{
    public GameObject roomAranger;
    bool stunned = false;

    private Rigidbody2D rb;
    public float speed = 5;
    public float jumpHeight = 1;
    bool canJump = false;

    public Vector2 jumpTestDimentions;
    public LayerMask jumpMask;

    public Vector2 wallStopDimentions;
    public LayerMask sideMask;
    bool t = true;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            transform.GetChild(1).GetComponent<Camera>().targetDisplay = 2;
            transform.GetChild(1).GetComponent<AudioListener>().enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(gameObject);
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "testing" && t)
        {
            t = false;
            transform.position = Vector3.zero;
            GetComponent<playerHealth>().enabled = true;
            if (IsServer && IsOwner)
            {
                GameObject aranger = Instantiate(roomAranger, Vector3.zero, Quaternion.EulerAngles(0, 0, 0));
                aranger.GetComponent<NetworkObject>().Spawn();
            }
        }

        Collider2D[] jumpCollision = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 - jumpTestDimentions.y / 2), jumpTestDimentions, 0, jumpMask);
        if (jumpCollision.Length > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        Collider2D[] rightCollision = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + transform.localScale.x / 2 + wallStopDimentions.x / 2, transform.position.y), wallStopDimentions, 0, sideMask);
        Collider2D[] leftCollision = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - transform.localScale.x / 2 - wallStopDimentions.x / 2, transform.position.y), wallStopDimentions, 0, sideMask);

        if (Input.GetAxisRaw("Horizontal") != 0 && !stunned)
        {
            if (Input.GetAxisRaw("Horizontal") == 1 && rightCollision.Length == 0)
            {
                rb.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            }
            if (Input.GetAxisRaw("Horizontal") == -1 && leftCollision.Length == 0)
            {
                rb.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            }
        }
        else
        {
            if (!stunned)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (Input.GetAxisRaw("Vertical") == 1 && canJump && rb.velocity.y <= 0 && !stunned)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if (transform.position.y < -10)
        {
            transform.position = Vector3.zero;
        }
    }

    public void Stun(float stuntime)
    {
        StartCoroutine(Stunned(stuntime));
    }

    IEnumerator Stunned(float stuntime)
    {
        stunned = true;
        yield return new WaitForSeconds(stuntime);
        stunned = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 - jumpTestDimentions.y / 2), new Vector3(jumpTestDimentions.x, jumpTestDimentions.y, 0));
        Gizmos.DrawWireCube(new Vector2(transform.position.x + transform.localScale.x / 2 + wallStopDimentions.x / 2, transform.position.y), wallStopDimentions);
        Gizmos.DrawWireCube(new Vector2(transform.position.x - transform.localScale.x / 2 - wallStopDimentions.x / 2, transform.position.y), wallStopDimentions);
    }

    [ClientRpc]
    public void explodeClientRpc(Vector2 nockback, ClientRpcParams clientRpcParams = default)
    {
        print("yes");
        //if (IsOwner) return;
        print("no");

        // Run your client-side logic here!!
        NetworkManager.LocalClient.PlayerObject.GetComponent<Rigidbody2D>().velocity = nockback;
    }
}
