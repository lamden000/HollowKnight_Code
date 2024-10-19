using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskGuardScript : EnemyBase
{
    //if the player pass this line, the guard will attack
    public int boundaryX;

    private float speed = 2.0f;
    public float deathForce = 200f;
    // Start is called before the first frame update
    enum HuskGuardState
    {
        Walking_left = 0,
        Walking_right = 1,
        Waiting=2,
        Dead = -1,
    }

    protected override void Start()
    {
        base.Start();
        health = 50;
        attackPower = 5;
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
                animator.SetTrigger("Dead");
                //StartCoroutine(WaitForAnimation(animator, "Dead"));
                break;
        }
    }
    public override void SetState(int state)
    {
        switch (state)
        {
            case (int)HuskGuardState.Walking_left:
                nx = -1;
                break;
            case (int)HuskGuardState.Walking_right:
                nx = 1;
                break;
            case 2:
                nx = -1;
                break;
            case 3:
                nx = 1;
                break;
            case (int)HuskGuardState.Dead:
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
        SetState((int)HuskGuardState.Dead);
        rb.velocity = Vector2.zero;
        rb.AddForce(attackDirection.normalized * deathForce);
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
        if (GetState() == -1)
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(transform.position.x+boundaryX, transform.position.y + 10),new Vector2(transform.position.x+boundaryX, transform.position.y - 10));
    }
}
