using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawlid : EnemyBase
{
    private float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        health = 50;
        attackPower = 5;
        SetState(0); // Walk left
    }

    void Update()
    {
        Move();
        Render();
    }
    public override void Render()
    {
        switch (state)
        {
            case 0:
                spriteRenderer.flipX = false;
                animator.SetBool("Walk_left", true);
                break;
            case 1:
                spriteRenderer.flipX = true;
                animator.SetBool("Walk_left", true);
                break;
            case 2:
                nx = -1;
                break;
            case 3:

                break;
            case -1:

                break;
        }
    }
    public void SetState(int state)
    {
        /*
            WALK_LEFT == 0
            WALK_RIGHT == 1
            WAS ATTACK LEFT == 2
            WAS ATTACK RIGHT == 3
            DEAD == -1
        */
        switch (state)
        {
            case 0:
                nx = -1;
                break;
            case 1:
                nx = 1;
                break;
            case 2:
                nx = -1;
                break;
            case 3:
                nx = 1;
                break;
            case -1:
                
                break;
            default:
                Debug.Log("Error SetState Crawlid");
                break;
        }

        base.SetState(state);
    }
    public override void Move()
    {
        transform.Translate(Vector2.right * nx * speed * Time.deltaTime);
    }

    public override void Attack()
    {
        Debug.Log("Crawlid attacks by colliding with the player.");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (nx == -1)
            {
                SetState(1);
            }
            else if (nx == 1)
            {
                SetState(0);
            }
            Debug.Log("Crawlid was attack");
        }
    }
}
