using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class lazers : NetworkBehaviour
{
    public inventory Inventory;
    public GameObject projectile;
    public NetworkVariable<float> _rotations = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> shooting = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.GetComponent<Animator>().enabled = false;
            Vector3 dir = transform.parent.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _rotations.Value = angle;
            if (Input.GetMouseButtonDown(0))
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
            transform.rotation = Quaternion.EulerAngles(0, 0, _rotations.Value);
        }
        if (shooting.Value)
        {
            //draw raycast
            if (IsServer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z)));
                RaycastHit2D hit2 = Physics2D.Raycast(hit.point, new Vector2(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z), Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z)));
                print(hit2.transform);
            }
        }
    }
}
