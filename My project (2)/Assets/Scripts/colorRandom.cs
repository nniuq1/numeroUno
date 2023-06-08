using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorRandom : MonoBehaviour
{
    Color charColor;

    void Start()
    {
        charColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        transform.GetComponent<SpriteRenderer>().color = charColor;
    }
}
