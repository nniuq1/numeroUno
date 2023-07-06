using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clientStarer : MonoBehaviour
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
        //transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);

    }
    public void StartGame()
    {
        sceneClientRpc();
        SceneManager.LoadScene("testing");
    }

    [ClientRpc]
    public void sceneClientRpc()
    {
        SceneManager.LoadScene("testing");
    }
}
