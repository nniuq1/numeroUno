using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
            print(timeRemaining);
        }

        if (timeRemaining <= 0)
        {
            print("youWin");
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
        NetworkManager.LocalClient.PlayerObject.transform.GetChild(2).GetChild(0).GetComponent<hamburguesaHeld>().timeRemaining = timeremaining;
        Object.FindObjectOfType<hamburgesaClock>().time = timeRemaining;

    }
}