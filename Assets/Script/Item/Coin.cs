using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private ItemData coindata;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GetMoney();
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rb.bodyType = RigidbodyType2D.Static; 
        }
    }
}
