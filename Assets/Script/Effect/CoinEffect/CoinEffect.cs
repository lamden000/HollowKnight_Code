using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> listob;
    private Rigidbody rb;
    private Transform trans;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
    }
    public void Coineffect()
    {
        StartCoroutine(CreateCoin());
    }
    IEnumerator CreateCoin()
    {
        foreach (GameObject go in listob)
        {
            Instantiate(go, trans);
        }
        yield return null;

    }
}
