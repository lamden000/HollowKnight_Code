using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFly : MonoBehaviour
{
    public Transform ground;
    public float bounceDistance = 1f; 
    public float bounceSpeed = 10f;  
    public float damping = 2f;
    private Vector3 originalPosition;
    private bool isBouncing = false;
    private AudioSource audioSource;

    void Start()
    {
        if (ground == null)
        {
            ground = transform; 
        }
        originalPosition = ground.position;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isBouncing)
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();

            if (playerScript != null) // Ensure the PlayerScript component exists
            {
                StartCoroutine(SpringEffect());
                audioSource.Play();
            }
        }
    }

    IEnumerator SpringEffect()
    {
        isBouncing = true;
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            float yOffset = Mathf.Exp(-damping * elapsedTime) * Mathf.Cos(bounceSpeed * elapsedTime) * bounceDistance;
            ground.position = originalPosition - new Vector3(0, yOffset, 0);

            Debug.Log($"Ground Position: {ground.position}");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ground.position = originalPosition;
        isBouncing = false;
    }
}
