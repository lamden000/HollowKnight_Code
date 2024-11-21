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
    
    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            /*PlayerScore playerScore = other.GetComponent<PlayerScore>();
            if (playerScore != null)
            {
                playerScore.AddScore(coinValue);
            }*/
            Destroy(gameObject);
        }
        else if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            rb.bodyType = RigidbodyType2D.Static; 
        }
    }
}
