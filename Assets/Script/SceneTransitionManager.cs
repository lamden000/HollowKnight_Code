using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public GameObject fadePanel;         // Reference to the black panel UI
    public float fadeDuration = 1.5f;    // Duration of the fade effect

    private Image panelImage;
    public bool loadLoadingScene = false;

    void Start()
    {
        panelImage = fadePanel.GetComponent<Image>();
        fadePanel.SetActive(false);  
    }

    public void StartSceneTransition(string targetSceneName)
    {
        PlayerPrefs.SetString("TargetScene", targetSceneName);
        PlayerPrefs.Save();
        StartCoroutine(FadeOutAndLoadScene(targetSceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string targetSceneName)
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

        // Load the loading scene asynchronously
        if (loadLoadingScene)
        {
            SceneManager.LoadSceneAsync("LoadingScene");
        }
        else
        {
            SceneManager.LoadSceneAsync(targetSceneName);
            GameManager.Instance.LastScene = SceneManager.GetActiveScene().name;
        }
        }
}