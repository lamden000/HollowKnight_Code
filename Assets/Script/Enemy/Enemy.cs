using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBase : MonoBehaviour
{
    public int health=50;
    public int coinRelease=3;
    public GameObject bloodPrefab;

    public float moveSpeedX;
    public float accelerationX;
    public float moveSpeedY;
    public float deathForceX = 250f;
    public float deathForceY = 50f;
    [HideInInspector] public bool isDead=false;

    protected Animator animator; 
    protected Rigidbody2D rb;
    protected int attackPower;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void TakeDamage(int amount,int directionX,float knockBackForce)
    {
        if(!isDead)
        {
            health -= amount;
            if (health <= 0)
            {
                Die(directionX);
            }
            else 
            {
                KnockBackOnHit(directionX,knockBackForce);              
            }
            if(bloodPrefab!=null)
            {
                GameObject lightInstance = Instantiate(bloodPrefab, transform.position, Quaternion.identity);

                lightInstance.SetActive(true);
            }    
        }
    }
    private void KnockBackOnHit(int directionX, float knockBackForce)
    {
        rb.AddForce(new Vector2(directionX*knockBackForce,0));
    }

    protected virtual void Die(int directionX)
    {
        rb.drag = 0;
        rb.AddForce(new Vector2(directionX* deathForceX, deathForceY));
        CoinEmitter.Instance.EmitCoins(transform.position+new Vector3(0,0.5f,0),coinRelease);
        isDead = true;
        animator.SetBool("dead", true);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        Destroy(gameObject, 15);
    }
}