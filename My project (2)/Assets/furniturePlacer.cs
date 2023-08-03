using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class furniturePlacer : NetworkBehaviour
{
    public GameObject backwardschair;
    public GameObject chair;
    public GameObject table;
    public GameObject sofa;
    public GameObject bed;
    public GameObject oven;

    private void Start()
    {
        if (IsOwnedByServer)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "chairLocation")
                {
                    GameObject newChair = Instantiate(chair, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newChair.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "chairLocation1")
                {
                    GameObject newTable = Instantiate(backwardschair, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "tableLocation")
                {
                    GameObject newTable = Instantiate(table, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "ovenLocation")
                {
                    GameObject newTable = Instantiate(oven, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "bedLocation")
                {
                    GameObject newTable = Instantiate(bed, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "sofaLocation")
                {
                    GameObject newTable = Instantiate(sofa, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
            }
        }
    }
}
