using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hamburguesaHeld : MonoBehaviour
{
    public float startTime = 60;
    float timeRemaining;

    private void Start()
    {
        timeRemaining = startTime;
    }

    private void Update()
    {
        transform.GetComponent<Animator>().enabled = false;
        if (Input.GetMouseButton(0))
        {
            timeRemaining -= Time.deltaTime;
            print(timeRemaining);
        }

        if (timeRemaining <= 0)
        {
            print("youWin");
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
