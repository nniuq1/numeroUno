using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;

public class clientStarer : NetworkBehaviour
{
    public NetworkManager bees;
    public Camera tempCamera;

    public void ReleasetheBees()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
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
        NetworkManager.GetComponent<UnityTransport>().ConnectionData.Address = transform.GetChild(3).transform.GetComponent<InputField>().text;
        bees.StartClient();
        Destroy(tempCamera.gameObject);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }
    public void StartGame()
    {
        NetworkManager.IsListening = false;
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
