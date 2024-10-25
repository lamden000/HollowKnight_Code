using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Crawlid : EnemyBase
{
    public float deathForce = 200f;
    // Start is called before the first frame update
    protected override void Start()
    {      
        base.Start();       
        attackPower = 5;
    }

    void Update()
    {             
    }
    
    protected override void Die(Vector2 attackDirection)
    {
		rb.velocity = Vector2.zero;
		rb.AddForce(attackDirection.normalized*deathForce);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
