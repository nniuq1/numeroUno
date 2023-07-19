using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class pointingScript : NetworkBehaviour
{
    public Vector2 targetPos;
    public itemClass Hamburguesa;

    void Update()
    {
        if (IsOwner)
        {
            bool findInChar = false;
            for (int i = 0; i < Object.FindObjectsOfType<inventory>().Length; i++)
            {
                for (int h = 0; h < Object.FindObjectsOfType<inventory>()[i].itemClasses.Count; h++)
                {
                    if (Object.FindObjectsOfType<inventory>()[i].itemClasses[h] == Hamburguesa)
                    {
                        targetPos = transform.parent.GetChild(1).GetComponent<Camera>().WorldToViewportPoint(Object.FindObjectsOfType<inventory>()[i].transform.position);
                        findInChar = true;
                    }
                }
            }
            if (!findInChar)
            {
                targetPos = new Vector2(1000, 1000);
                for (int i = 0; i < Object.FindObjectsOfType<itemScript>().Length; i++)
                {
                    if (Object.FindObjectsOfType<itemScript>()[i].itemclass == Hamburguesa)
                    {
                        targetPos = transform.parent.GetChild(1).GetComponent<Camera>().WorldToViewportPoint(Object.FindObjectsOfType<itemScript>()[i].transform.position);
                    }
                }
            }

            //Vector2 pos = transform.parent.GetChild(1).GetComponent<Camera>().WorldToViewportPoint(Vector2.zero);
            if (targetPos != new Vector2(1000, 1000))
            {
                if ((targetPos.x < 0 || targetPos.x > 1) || (targetPos.y < 0 || targetPos.y > 1))
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    Vector3 dir = transform.parent.GetChild(1).GetComponent<Camera>().ViewportToWorldPoint(targetPos) - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Vector2 arrow = targetPos;
                    if (arrow.x < .1)
                    {
                        double d = (arrow.x - .5) / 0.4;
                        arrow.x = .1f;
                        arrow.y = (float)((arrow.y - 0.5) / (-d) + .5);
                    }
                    else if (arrow.x > .9)
                    {
                        double d = (arrow.x - 0.5) / 0.4;
                        arrow.x = .9f;
                        arrow.y = (float)((arrow.y - 0.5) / d + .5);
                    }
                    if (arrow.y < .1)
                    {
                        double d = (arrow.y - 0.5) / 0.4;
                        arrow.y = .1f;
                        arrow.x = (float)((arrow.x - 0.5) / (-d) + .5);
                    }
                    else if (arrow.y > .9)
                    {
                        double d = (arrow.y - 0.5) / 0.4;
                        arrow.y = .9f;
                        arrow.x = (float)((arrow.x - 0.5) / d + .5);
                    }
                    arrow.y = arrow.y * transform.parent.GetChild(1).GetComponent<Camera>().pixelHeight;
                    arrow.x = arrow.x * transform.parent.GetChild(1).GetComponent<Camera>().pixelWidth;
                    Vector2 p = transform.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(arrow);
                    transform.GetChild(0).transform.position = p;
                    transform.GetChild(1).transform.position = p;
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}
