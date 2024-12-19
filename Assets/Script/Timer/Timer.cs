using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float countdownTime = 10f;
    [SerializeField] private TextMeshProUGUI countdownText; 
    private float currentTime;
    private bool isTimerRunning = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        ResetTimer();
        HuskGuardScript.BossDeathEvent += OnBossDeath;
    }

    void Update()
    {
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0);
            UpdateCountdownText();
            if (currentTime <= 0)
            {
                OnCountdownEnd();
            }
        }
    }

    void UpdateCountdownText()
    {
        countdownText.text = currentTime.ToString("F2");
    }

    void OnBossDeath()
    {
        ResetTimer();
        currentTime = countdownTime;
        isTimerRunning = true;
        countdownText.gameObject.SetActive(true);
        UpdateCountdownText();
        audioSource.Play();

    }

    void OnCountdownEnd()
    {
        countdownText.transform.localScale = Vector3.one;
        audioSource.Stop();
        GameManager.Instance.SubmitPoints();
        GameManager.Instance.ResetGame();
    }

    void ResetTimer()
    {
        currentTime = countdownTime;
        isTimerRunning = false;
        audioSource.Stop();
        countdownText.gameObject.SetActive(false); 
        UpdateCountdownText();
    }

    void OnDestroy()
    {
        HuskGuardScript.BossDeathEvent -= OnBossDeath;
    }
}
