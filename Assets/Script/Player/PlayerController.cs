using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100; // Máu của nhân vật

    // Phương thức để giảm máu
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Phương thức khi nhân vật chết
    void Die()
    {
        Debug.Log("Player has died.");
        // Xử lý logic khi nhân vật chết, như chơi animation hoặc reset game
    }
}
