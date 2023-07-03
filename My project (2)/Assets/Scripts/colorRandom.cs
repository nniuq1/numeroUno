using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class colorRandom : NetworkBehaviour
{
    public NetworkVariable<Color> _netColor = new NetworkVariable<Color>(writePerm: NetworkVariableWritePermission.Owner);
    private bool t = false;

    void Start()
    {
        if (IsOwner)
        {
            _netColor.Value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            transform.GetComponent<SpriteRenderer>().color = _netColor.Value;
        }
        StartCoroutine(waitchangeColour());
    }
    void Update()
    {
        if (!IsOwner && t)
        {
            t = false;
            transform.GetComponent<SpriteRenderer>().color = _netColor.Value;
        }
    }

    IEnumerator waitchangeColour()
    {
        yield return new WaitForSeconds(0.01f);
        t = true;
    }
}
