using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : NetworkBehaviour
{
    public GameObject optionsMenu;
    public GameObject pausemenu;

    private void Start()
    {
        if (PlayerPrefs.GetInt("clouds") == 1)
        {
            optionsMenu.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
        }
        else
        {
            optionsMenu.transform.GetChild(1).GetComponent<Toggle>().isOn = false;
        }

        optionsMenu.transform.GetChild(6).GetComponent<Slider>().value = PlayerPrefs.GetFloat("sfxVolume");
        optionsMenu.transform.GetChild(3).GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && NetworkManager.LocalClient != null)
        {
            pausemenu.SetActive(true);
            NetworkManager.LocalClient.PlayerObject.GetComponent<charmovement>().Pause();
        }
    }

    public void Resume()
    {
        pausemenu.SetActive(false);
        NetworkManager.LocalClient.PlayerObject.GetComponent<charmovement>().Resume();
    }

    public void QuitGame()
    {
        QuitClientRPC();
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.gameObject);
        SceneManager.LoadScene("Main Menu");
    }

    [ClientRpc]
    void QuitClientRPC()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.gameObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void Options()
    {
        optionsMenu.active = true;
        pausemenu.transform.GetChild(0).gameObject.active = false;
        pausemenu.transform.GetChild(1).gameObject.active = false;
        pausemenu.transform.GetChild(2).gameObject.active = false;
    }

    public void UnOptions()
    {
        optionsMenu.active = false;
        pausemenu.transform.GetChild(0).gameObject.active = true;
        pausemenu.transform.GetChild(1).gameObject.active = true;
        pausemenu.transform.GetChild(2).gameObject.active = true;
    }

    public void Clouds()
    {
        if (optionsMenu.transform.GetChild(1).GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("clouds", 1);
        }
        else
        {
            PlayerPrefs.SetInt("clouds", 0);
        }
    }

    public void SFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", optionsMenu.transform.GetChild(6).GetComponent<Slider>().value);
    }

    public void MusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", optionsMenu.transform.GetChild(3).GetComponent<Slider>().value);
    }
}
