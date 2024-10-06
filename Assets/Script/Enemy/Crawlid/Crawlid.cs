using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawlid : EnemyBase
{
    private float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        health = 50; // Crawlid c� �t m�u h?n so v?i Zombie ho?c Alien
        attackPower = 5; // Crawlid c� s?c m?nh t?n c�ng y?u h?n
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Move()
    {
        // Crawlid di chuy?n ch?m tr�n m?t ??t
        Debug.Log("Crawlid is crawling.");
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public override void Attack()
    {
        // Crawlid c� th? t?n c�ng b?ng c�ch va ch?m v?i nh�n v?t ch�nh
        Debug.Log("Crawlid attacks by colliding with the player.");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Crawlid was attack");
        }
    }
}
