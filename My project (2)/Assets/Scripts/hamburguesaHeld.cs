using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class hamburguesaHeld : NetworkBehaviour
{
    public float startTime = 60;
    public float timeRemaining;

    private void Start()
    {
        timeRemaining = startTime;
    }

    private void Update()
    {
        transform.GetComponent<Animator>().enabled = false;
        if (Input.GetMouseButton(0) && IsOwner)
        {
            timeRemaining -= Time.deltaTime;
            SetTimeServerRPC(timeRemaining);
            //print(timeRemaining);
        }

        if (timeRemaining <= 0)
        {
            VictoryServerRPC();
            //print("youWin");
            //Destroy(transform.parent.parent.gameObject);
        }
    }

    [ServerRpc]
    public void SetTimeServerRPC(float timeremaining)
    {
        SetTimeClientRPC(timeremaining);
    }
    [ClientRpc]
    public void SetTimeClientRPC(float timeremaining)
    {
        timeRemaining = timeremaining;
        Object.FindObjectOfType<hamburgesaClock>().time = timeremaining;
    }

    [ServerRpc]
    public void VictoryServerRPC()
    {
        VictoryClientRPC();
    }

    [ClientRpc]
    public void VictoryClientRPC()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.gameObject);
        SceneManager.LoadScene("Main Menu");
    }
}