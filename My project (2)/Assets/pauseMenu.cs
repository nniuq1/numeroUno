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
        }
    }

    public void Resume()
    {
        pausemenu.SetActive(false);
    }

    public void QuitGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }
}
