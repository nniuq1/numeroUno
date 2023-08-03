using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectVolume : MonoBehaviour
{
    public float baseVolume = 1;

    private void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfxVolume", 1) * baseVolume;
    }
}
