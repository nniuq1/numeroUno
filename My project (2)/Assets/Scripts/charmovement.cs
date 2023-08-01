using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class charmovement : NetworkBehaviour
{
    bool stunStateChange = false;

    public bool isPaused;

    public GameObject dustparticle;
    public float footstepChance = 0.05f;

    public GameObject roomAranger;
    public bool stunned = false;

    private Rigidbody2D rb;
    public float speed = 5;
    public float jumpHeight = 1;
    bool canJump = false;

    public Vector2 jumpTestDimentions;
    public LayerMask jumpMask;

    public Vector2 wallStopDimentions;
    public LayerMask sideMask;
    int t = 0;

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
        if (isPaused || stunned)
        {
            transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = false;
            transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = false;
            transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = false;
            transform.GetChild(2).GetChild(0).GetComponent<lazers>().enabled = false;
            stunStateChange = true;
        }
        else if (stunStateChange)
        {
            transform.GetChild(2).GetChild(0).GetComponent<pointHeld>().enabled = true;
            transform.GetChild(2).GetChild(0).GetComponent<meleeHeld>().enabled = true;
            transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().enabled = true;
            transform.GetChild(2).GetChild(0).GetComponent<lazers>().enabled = true;
        }

        if (SceneManager.GetActiveScene().name == "testing" && t == 0)
        {
            t = 1;
            GetComponent<playerHealth>().enabled = true;
            if (IsServer && IsOwner)
            {
                GameObject aranger = Instantiate(roomAranger, Vector3.zero, Quaternion.EulerAngles(0, 0, 0));
                aranger.GetComponent<NetworkObject>().Spawn();
            }
        }
        else if(t == 1)
        {
            t = 2;
            transform.position = Object.FindObjectOfType<respawnPoint>().transform.position;
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

        if (Input.GetAxisRaw("Horizontal") != 0 && !stunned && !isPaused)
        {
            if (Input.GetAxisRaw("Horizontal") == 1 && rightCollision.Length == 0)
            {
                rb.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
                int random = Random.Range(1, (int)(1 / footstepChance) + 1);
                if (random == (int)(1 / footstepChance) && canJump)
                {
                    Instantiate(dustparticle, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.Euler(-90, 0, 0));
                }
            }
            if (Input.GetAxisRaw("Horizontal") == -1 && leftCollision.Length == 0)
            {
                rb.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
                int random = Random.Range(1, (int)(1 / footstepChance) + 1);
                if (random == (int)(1 / footstepChance) && canJump)
                {
                    Instantiate(dustparticle, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.Euler(-90, 0, 0));
                }
            }
        }
        else
        {
            if (!stunned)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (Input.GetAxisRaw("Vertical") == 1 && canJump && rb.velocity.y <= 0 && !stunned && !isPaused)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if (transform.position.y < -10)
        {
            //transform.position = Vector3.zero;
            GetComponent<playerHealth>().TakeDamage(10000);
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
        //print(nockback);
        NetworkManager.LocalClient.PlayerObject.GetComponent<Rigidbody2D>().velocity = nockback;
        Stun(0.75f);
    }

    public void Pause()
    {
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }
}
