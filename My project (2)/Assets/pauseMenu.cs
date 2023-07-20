using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class pauseMenu : NetworkBehaviour
{
    public GameObject pausemenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.gameObject);
        SceneManager.LoadScene("Main Menu");
    }
}
