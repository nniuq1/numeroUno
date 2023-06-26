using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class charmovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5;
    public float jumpHeight = 1;
    bool canJump = false;

    public Vector2 jumpTestDimentions;
    public LayerMask jumpMask;

    public Vector2 wallStopDimentions;
    public LayerMask sideMask;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            transform.GetChild(1).GetComponent<Camera>().targetDisplay = 1;
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetAxisRaw("Horizontal") != 0)
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
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") == 1 && canJump && rb.velocity.y <= 0)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if (transform.position.y < -10)
        {
            transform.position = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 - jumpTestDimentions.y / 2), new Vector3(jumpTestDimentions.x, jumpTestDimentions.y, 0));
        Gizmos.DrawWireCube(new Vector2(transform.position.x + transform.localScale.x / 2 + wallStopDimentions.x / 2, transform.position.y), wallStopDimentions);
        Gizmos.DrawWireCube(new Vector2(transform.position.x - transform.localScale.x / 2 - wallStopDimentions.x / 2, transform.position.y), wallStopDimentions);
    }
}
