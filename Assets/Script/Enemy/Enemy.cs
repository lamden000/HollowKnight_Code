using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Move();
    void Attack();
    void TakeDamage(int amount);
    void SetState(int state);
    void Render();
}
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    protected int health;
    protected int attackPower;
    protected int state;

    protected float vx;
    protected float vy;

    protected int nx;
    protected int ny;
    protected Animator animator; // Tham chiếu đến Animator
    protected SpriteRenderer spriteRenderer;
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Lấy component Animator
    }
    public virtual void SetState(int state){
        this.state = state;
    }
    public int GetState()
    {
        return state;
    }
    public abstract void Move();

    public abstract void Attack();
    public abstract void Render();
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Debug.Log($"{this.GetType().Name} has died.");
        // Logic for enemy death
    }
}

