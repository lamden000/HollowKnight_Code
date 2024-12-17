using UnityEngine;

public class CoinEmitter : MonoBehaviour
{
    public static CoinEmitter Instance { get; private set; } // Singleton instance

    [Header("Coin Settings")]
    public GameObject coinPrefab;  // Assign your coin prefab in the Inspector
    public float emitForce = 5f;   // Force applied to each coin
    public float spreadAngle = 45f; // Maximum angle of random spread (in degrees)

    private void Awake()
    {
        // Ensure this is the only instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    /// <summary>
    /// Emits a specified number of coins upward with random directions.
    /// </summary>
    /// <param name="emitPosition">The position to emit coins from.</param>
    /// <param name="coinCount">The number of coins to emit.</param>
    public void EmitCoins(Vector3 emitPosition, int coinCount)
    {
        for (int i = 0; i < coinCount; i++)
        {
            // Instantiate the coin prefab at the emit position
            GameObject coin = Instantiate(coinPrefab, emitPosition, Quaternion.identity);

            // Get the Rigidbody2D component of the coin (ensure your prefab has one)
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate a random direction within the spread angle
                Vector2 randomDirection = Quaternion.Euler(0, 0, Random.Range(-spreadAngle, spreadAngle)) * Vector2.up;

                // Apply force to the coin in the random direction
                rb.AddForce(randomDirection * emitForce, ForceMode2D.Impulse);
            }
        }
    }
}