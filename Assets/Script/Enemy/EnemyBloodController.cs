using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyBloodController : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Light2D lightSource;
    void Start()
    {
        lightSource = GetComponent<Light2D>();
        StartCoroutine(FadeOutLight());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator FadeOutLight()
    {
        float startIntensity = lightSource.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            lightSource.intensity = Mathf.Lerp(startIntensity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the intensity is set to 0 at the end
        lightSource.intensity = 0f;
        Destroy(gameObject);
    }
}
