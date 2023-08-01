using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class furniturePlacer : NetworkBehaviour
{
    public GameObject chair;
    public GameObject table;

    private void Start()
    {
        print(1);
        if (IsOwnedByServer)
        {
            print(2);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "chairLocation")
                {
                    GameObject newChair = Instantiate(chair, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newChair.GetComponent<NetworkObject>().Spawn();
                }
                else if (transform.GetChild(i).name == "tableLocation")
                {
                    GameObject newTable = Instantiate(table, transform.GetChild(i).transform.position, Quaternion.Euler(0, 0, 0), transform);
                    newTable.GetComponent<NetworkObject>().Spawn();
                }
            }
        }
    }
}
