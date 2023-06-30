using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class clientStarer : MonoBehaviour
{
    public NetworkManager bees;
    public Camera tempCamera;

    public void ReleasetheBees()
    {
        bees.StartClient();
        Destroy(tempCamera.gameObject);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void formTheHive()
    {
        bees.StartHost();
        Destroy(tempCamera.gameObject);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

    }
}
