using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hamburgesaClock : MonoBehaviour
{
    public float time = 60;

    private void Update()
    {
        transform.GetChild(0).GetComponent<Image>().fillAmount = time / 60f;
    }
}
