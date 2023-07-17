using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class clientStarer : NetworkBehaviour
{
    public NetworkManager bees;
    public Camera tempCamera;

    public void ReleasetheBees()
    {
        bees.StartClient();
        Destroy(tempCamera.gameObject);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    public void formTheHive()
    {
        bees.StartHost();
        Destroy(tempCamera.gameObject);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);

    }
    public void placeTheHive()
    {
        NetworkManager.GetComponent<UnityTransport>().ConnectionData.Address = "";
        bees.StartClient();
    }
    public void StartGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        sceneClientRpc();
        SceneManager.LoadScene("testing");
    }

    [ClientRpc]
    public void sceneClientRpc()
    {
        SceneManager.LoadScene("testing");
    }
}
