using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuBullet : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector2(transform.position.x + Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z) * Time.deltaTime * 15, transform.position.y + Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z) * Time.deltaTime * 15);
    }
}
