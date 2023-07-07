using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class roomAranger : NetworkBehaviour
{
    public NetworkVariable<int> _netseed = new NetworkVariable<int>();

    public List<GameObject> roomsMid;
    public List<GameObject> roomsLeft;
    public List<GameObject> roomsRight;
    public List<GameObject> roomsStairs;
    public List<GameObject> fireEscapes;
    List<int> isStair = new List<int>();
    public GameObject floor;
    public GameObject fireEscapeFloor;
    public List<GameObject> roof;
    public GameObject roofExit;
    public GameObject roofExit2;
    int stairTypeTop;

    public GameObject leftHamburguesa;
    public GameObject midHamburguesa;
    public GameObject rightHamburguesa;
    Vector2 hamburguesaPos;

    public int length;
    public int height;
    int choose = 0;

    private void Start()
    {
        if (Object.FindObjectOfType<NetworkManager>().GetComponent<NetworkManager>().IsServer)
        {
            int seed = Random.Range(-10000, 10000);
            _netseed.Value = seed;
            Random.seed = seed;
            seedClientRpc(seed);
            print(seed + "serverSeed");

            
            hamburguesaPos = new Vector2(Random.Range(0, length), Random.Range(0, height));

            for (int h = 0; h < height; h++)
            {
                int addStair = 0;

                for (int i = 0; i < 1; i++)
                {
                    addStair = Random.Range(0, length - 2);
                    if (isStair.Count > 0)
                    {
                        if (addStair == isStair[isStair.Count - 1] || h == hamburguesaPos.y && addStair + 1 == hamburguesaPos.x)
                        {
                            i = -1;
                        }
                    }
                }

                isStair.Add(addStair);
            }

            for (int h = 0; h < height; h++)
            {
                if (hamburguesaPos.x == 0 && hamburguesaPos.y == h)
                {
                    Instantiate(leftHamburguesa, new Vector2(transform.position.x, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    choose = Random.Range(0, roomsLeft.Count);
                    Instantiate(roomsLeft[choose], new Vector2(transform.position.x, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }

                for (int i = 0; i < length - 2; i++)
                {
                    if (i + 1 == hamburguesaPos.x && h == hamburguesaPos.y)
                    {
                        Instantiate(midHamburguesa, new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                    }
                    else if (i == isStair[h])
                    {
                        stairTypeTop = Random.Range(0, roomsStairs.Count);
                        Instantiate(roomsStairs[stairTypeTop], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        if (h > 0)
                        {
                            if (isStair[h - 1] == i)
                            {
                                Instantiate(roomsMid[0], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            {
                                choose = Random.Range(0, roomsMid.Count);
                                Instantiate(roomsMid[choose], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                            }
                        }
                        else
                        {
                            choose = Random.Range(0, roomsMid.Count);
                            Instantiate(roomsMid[choose], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                        }
                    }
                }

                if (hamburguesaPos.x == length - 1 && hamburguesaPos.y == h)
                {
                    Instantiate(rightHamburguesa, new Vector2(transform.position.x + 17.75f * (length - 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    choose = Random.Range(0, roomsRight.Count);
                    Instantiate(roomsRight[choose], new Vector2(transform.position.x + 17.75f * (length - 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }
            }

            for (int i = 0; i < length; i++)
            {
                Instantiate(floor, new Vector2(transform.position.x + 17.75f * (i), transform.position.y), Quaternion.Euler(0, 0, 0));
            }

            for (int h = 0; h < height; h++)
            {
                Instantiate(fireEscapes[Random.Range(0, fireEscapes.Count)], new Vector2(transform.position.x - 17.75f, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
            }

            Instantiate(fireEscapeFloor, new Vector2(transform.position.x - 17.75f, transform.position.y), Quaternion.Euler(0, 0, 0));

            for (int i = 0; i < length; i++)
            {
                if (isStair[height - 1] == i - 1)
                {
                    if (stairTypeTop == 0)
                    {
                        Instantiate(roofExit, new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        Instantiate(roofExit2, new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
                    }
                }
                else
                {
                    Instantiate(roof[Random.Range(0, roof.Count)], new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }

    [ClientRpc]
    public void seedClientRpc(int seed)
    {
        print(seed + "clientSeed");
        _netseed.Value = seed;
        Random.seed = _netseed.Value;
        

        hamburguesaPos = new Vector2(Random.Range(0, length), Random.Range(0, height));

        for (int h = 0; h < height; h++)
        {
            int addStair = 0;

            for (int i = 0; i < 1; i++)
            {
                addStair = Random.Range(0, length - 2);
                if (isStair.Count > 0)
                {
                    if (addStair == isStair[isStair.Count - 1] || h == hamburguesaPos.y && addStair + 1 == hamburguesaPos.x)
                    {
                        i = -1;
                    }
                }
            }

            isStair.Add(addStair);
        }

        for (int h = 0; h < height; h++)
        {
            if (hamburguesaPos.x == 0 && hamburguesaPos.y == h)
            {
                Instantiate(leftHamburguesa, new Vector2(transform.position.x, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                choose = Random.Range(0, roomsLeft.Count);
                Instantiate(roomsLeft[choose], new Vector2(transform.position.x, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
            }

            for (int i = 0; i < length - 2; i++)
            {
                if (i + 1 == hamburguesaPos.x && h == hamburguesaPos.y)
                {
                    Instantiate(midHamburguesa, new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }
                else if (i == isStair[h])
                {
                    stairTypeTop = Random.Range(0, roomsStairs.Count);
                    Instantiate(roomsStairs[stairTypeTop], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    if (h > 0)
                    {
                        if (isStair[h - 1] == i)
                        {
                            Instantiate(roomsMid[0], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                        }
                        else
                        {
                            choose = Random.Range(0, roomsMid.Count);
                            Instantiate(roomsMid[choose], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                        }
                    }
                    else
                    {
                        choose = Random.Range(0, roomsMid.Count);
                        Instantiate(roomsMid[choose], new Vector2(transform.position.x + 17.75f * (i + 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
                    }
                }
            }

            if (hamburguesaPos.x == length - 1 && hamburguesaPos.y == h)
            {
                Instantiate(rightHamburguesa, new Vector2(transform.position.x + 17.75f * (length - 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                choose = Random.Range(0, roomsRight.Count);
                Instantiate(roomsRight[choose], new Vector2(transform.position.x + 17.75f * (length - 1), transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
            }
        }

        for (int i = 0; i < length; i++)
        {
            Instantiate(floor, new Vector2(transform.position.x + 17.75f * (i), transform.position.y), Quaternion.Euler(0, 0, 0));
        }

        for (int h = 0; h < height; h++)
        {
            Instantiate(fireEscapes[Random.Range(0, fireEscapes.Count)], new Vector2(transform.position.x - 17.75f, transform.position.y + 8.5f * h), Quaternion.Euler(0, 0, 0));
        }

        Instantiate(fireEscapeFloor, new Vector2(transform.position.x - 17.75f, transform.position.y), Quaternion.Euler(0, 0, 0));

        for (int i = 0; i < length; i++)
        {
            if (isStair[height - 1] == i - 1)
            {
                if (stairTypeTop == 0)
                {
                    Instantiate(roofExit, new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    Instantiate(roofExit2, new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
                }
            }
            else
            {
                Instantiate(roof[Random.Range(0, roof.Count)], new Vector2(transform.position.x + 17.75f * i, transform.position.y + 8.6f * height), Quaternion.Euler(0, 0, 0));
            }
        }
    }
}
