using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawlid : EnemyBase
{
    private float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        health = 50; // Crawlid có ít máu h?n so v?i Zombie ho?c Alien
        attackPower = 5; // Crawlid có s?c m?nh t?n công y?u h?n
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Move()
    {
        // Crawlid di chuy?n ch?m trên m?t ??t
        Debug.Log("Crawlid is crawling.");
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public override void Attack()
    {
        // Crawlid có th? t?n công b?ng cách va ch?m v?i nhân v?t chính
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
