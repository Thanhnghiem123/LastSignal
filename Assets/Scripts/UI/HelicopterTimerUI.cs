using UnityEngine;
using TMPro;
using System;

public class HelicopterTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float duration;
    private float elapsedTime;
    private bool isActive;

    public event Action<float> OnTimerStart;
    public float RemainingTime
    {
        get
        {
            float time = Mathf.Max(0f, duration - elapsedTime);
            return time;
        }
    }

    void Start()
    {
        timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;
        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        float timeLeft = RemainingTime;
        timerText.text = $"{(int)(timeLeft / 60):00}:{(int)(timeLeft % 60):00}";
    }

    public void Activate(float timerDuration)
    {
        duration = timerDuration;
        elapsedTime = 0f;
        isActive = true;
        timerText.gameObject.SetActive(true);
        Debug.Log($"Timer activated with duration: {timerDuration}");
        OnTimerStart?.Invoke(duration);
    }
}