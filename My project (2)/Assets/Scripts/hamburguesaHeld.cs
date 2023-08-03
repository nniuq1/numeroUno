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
            timeRemaining = 1000;
            SetTimeServerRPC(timeRemaining);
            for (int i = 0; i < transform.parent.parent.GetComponent<inventory>().itemClasses.Count; i++)
            {
                if (transform.parent.parent.GetComponent<inventory>().itemClasses[i].weapontype == itemClass.WeaponType.Hamburguesa)
                {
                    transform.parent.parent.GetComponent<inventory>().itemClasses.RemoveAt(i);
                    if (i < transform.parent.parent.GetComponent<inventory>().itemSelected.Value)
                    {
                        transform.parent.parent.GetComponent<inventory>().itemSelected.Value--;
                    }
                }
            }
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
        SceneManager.LoadScene("victory");
        transform.parent.parent.position = new Vector3(0, 3, 0);
    }
}