using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.parent.GetChild(1).GetComponent<Camera>().WorldToViewportPoint(Vector2.zero);
        if ((pos.x<0 || pos.x > 1) || (pos.y<0 || pos.y>1))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Vector3 dir = transform.parent.GetChild(1).GetComponent<Camera>().WorldToViewportPoint(Vector2.zero) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector2 arrow = pos;
            if (arrow.x < .1)
            {
                double d = (arrow.x - .5) / 0.4;
                arrow.x = .1f;
                arrow.y = (float)((arrow.y - 0.5) / (-d) + .5);
            }
            else if (arrow.x > .9)
            {
                double d = (arrow.x - 0.5)/0.4;
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
                arrow.x = (float)((arrow.x - 0.5)/ d +.5);
            }
            arrow.y = arrow.y * transform.parent.GetChild(1).GetComponent<Camera>().pixelHeight;
            arrow.x = arrow.x * transform.parent.GetChild(1).GetComponent<Camera>().pixelWidth;
            Vector2 p = transform.parent.GetChild(1).GetComponent<Camera>().ScreenToWorldPoint(arrow);
            transform.GetChild(0).transform.position = p;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
