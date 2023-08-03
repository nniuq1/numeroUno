using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudToggleScript : MonoBehaviour
{
    private void Update()
    {
        if (PlayerPrefs.GetInt("clouds") == 1)
        {
            transform.GetChild(0).gameObject.active = true;
        }
        else
        {
            transform.GetChild(0).gameObject.active = false;
        }
    }
}
