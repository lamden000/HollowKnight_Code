using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Crawlid : EnemyBase
{
    private float speed = 2.0f;
    public float deathForce = 200f;
    private Vector2 attackedDirection;
    // Start is called before the first frame update
    enum CrawlidState
    {
        Walking_left =0, 
        Walking_right =1,
        Dead =-1,

    }

    protected override void Start()
    {
        base.Start();
        health = 50;
        attackPower = 5;
        SetState(0); // Walk left
    }

    void Update()
    {
        
        if (GetState() == -1)
        {
            return;
        }
        Move();
        Render();
    }
    public override void Render()
    {
        switch (state)
        {
            case 0:
                spriteRenderer.flipX = false;
                break;
            case 1:
                spriteRenderer.flipX = true;
                break;
            case 2:
                nx = -1;
                break;
            case 3:

                break;
            case -1:
                animator.SetTrigger("Dead");
                //StartCoroutine(WaitForAnimation(animator, "Dead"));
                break;
        }
    }
    public override void SetState(int state)
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
            case (int)CrawlidState.Walking_left:
                nx = -1;
                break;
            case (int)CrawlidState.Walking_right:
                nx = 1;
                break;
            case 2:
                nx = -1;
                break;
            case 3:
                nx = 1;
                break;
            case (int)CrawlidState.Dead:
                Render();
                break;
            default:
                Debug.Log("Error SetState Crawlid");
                break;
        }

        base.SetState(state);
    }
    protected override void Die(Vector2 attackDirection)
    {
        animator.SetTrigger("Dead");
		SetState((int)CrawlidState.Dead);
		rb.velocity = Vector2.zero;
		rb.AddForce(attackDirection.normalized*deathForce);
        Destroy(gameObject, 3);
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
        if(GetState() == -1)
        {
            return;
        }
       
    }
}
