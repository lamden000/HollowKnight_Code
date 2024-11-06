using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    private string targetSceneName;    // Name of the scene to load after loading screen
    public float minimumLoadTime = 3f; // Minimum duration for the loading screen
    private Image panelImage;
    public GameObject fadePanel;         // Reference to the black panel UI
    public float fadeDuration = 1.5f;    // Duration of the fade effect
    AsyncOperation operation;

    private void Start()
    {
        targetSceneName = PlayerPrefs.GetString("TargetScene", "DefaultScene"); // Add a default in case
        StartCoroutine(LoadTargetSceneAsync());
        panelImage = fadePanel.GetComponent<Image>();
        fadePanel.SetActive(false);
    }
    public void StartSceneTransition()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator LoadTargetSceneAsync()
    {
        // Start async operation
        operation = SceneManager.LoadSceneAsync(targetSceneName);
        operation.allowSceneActivation = false;

        // Wait for the minimum load time
        float elapsedTime = 0f;
        while (elapsedTime < minimumLoadTime || operation.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Activate the target scene
        StartSceneTransition();
    }
    private IEnumerator FadeOutAndLoadScene()
    {
        // Enable the panel and set alpha to 0
        fadePanel.SetActive(true);
        Color color = panelImage.color;
        color.a = 0;
        panelImage.color = color;

        // Gradually increase the alpha to create fade-to-black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            panelImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure fully opaque
        color.a = 1;
        panelImage.color = color;
        operation.allowSceneActivation = true;
    }
}