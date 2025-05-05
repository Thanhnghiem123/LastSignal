using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class IntroCutscene : MonoBehaviour
{
    public AudioSource radioAudio;
    public TextMeshProUGUI introText;
    public bool cutsceneFinished { get; private set; } // Make it public with a private setter

    public event Action OnCutsceneFinished; // Sự kiện khi cutscene kết thúc

    void Start()
    {
        StartCoroutine(StartCutsceneWithDelay());
    }

    IEnumerator StartCutsceneWithDelay()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        introText.text = "Be careful, zombies might hear your gunshots";
        yield return new WaitForSeconds(5f);

        introText.text = "Quickly escape from here";
        yield return new WaitForSeconds(5f);

        introText.text = "Radio sound ...";
        radioAudio.Play();
        yield return new WaitForSeconds(5f);

        introText.text = "\"This is the last rescue team. We will arrive at the northern highway in 10 minutes!\"";
        yield return new WaitForSeconds(5f);

        introText.text = "I repeat, this is the last rescue team. We will arrive at the northern highway in 10 minutes!";
        yield return new WaitForSeconds(5f);

        introText.text = "Please try to wait for us";
        yield return new WaitForSeconds(5f);

        introText.text = "ZOMBIES ARE APPROACHING! TRY TO SURVIVE";
        yield return new WaitForSeconds(5f);

        introText.text = "";
        cutsceneFinished = true;
        OnCutsceneFinished?.Invoke(); // Trigger the event when the cutscene ends
    }

    public void ShowText(string message, float duration)
    {
        StartCoroutine(DisplayText(message, duration));
    }

    IEnumerator DisplayText(string message, float duration)
    {
        introText.text = message;
        yield return new WaitForSeconds(duration);
        introText.text = "";
    }
}