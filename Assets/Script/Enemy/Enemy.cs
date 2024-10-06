using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Move();
    void Attack();
    void TakeDamage(int amount);
}
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    protected int health;
    protected int attackPower;

    public abstract void Move();

    public abstract void Attack();

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

