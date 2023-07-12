using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class lazers : NetworkBehaviour
{
    [SerializeField] public LineRenderer line;
    public inventory Inventory;
    public GameObject projectile;
    public NetworkVariable<float> _rotations = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> shooting = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

    void Update()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        transform.GetComponent<Animator>().enabled = false;
        if (IsOwner)
        {
            Vector3 dir = transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _rotations.Value = angle;
            if (Input.GetMouseButton(0))
            {
                shooting.Value = true;
            }
            else
            {
                shooting.Value = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, _rotations.Value);
        }
        if (shooting.Value)
        {
            //draw raycast
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, new Vector2(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z)));
            line.enabled = false;
            line.SetPosition(0, transform.position);
            line.SetPosition(0, hit[1].point);
            if (IsServer)
            {
                //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z)));
                if (hit[1].transform != null && hit[1].transform.GetComponent<playerHealth>() != null)
                {
                    hit[1].transform.GetComponent<playerHealth>().TakeDamage(3 * Time.deltaTime);
                }
            }
        }
        else
        {
            line.enabled = false;
        }
    }
}
