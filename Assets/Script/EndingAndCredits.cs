using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingAndCredits : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject storyPanel;
    public GameObject creditsPanel;

    [Header("Story Settings")]
    public TMP_Text storyText;
    [TextArea(3, 10)]
    public string[] storyPages;
    public float typingSpeed = 0.05f;
    public float delayBetweenPages = 3f;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.8f;

    [Header("Credits Settings")]
    public RectTransform creditsContent; // Objek yang berisi teks credits
    public float scrollSpeed = 50f;
    public float AutoExitDelay = 3f;

    [Header("Scene Target")]
    public string splashScreenSceneName = "SplashScreen";

    void Start()
    {
        storyPanel.SetActive(true);
        creditsPanel.SetActive(false);
        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        // === TAHAP 1: CERITA ===
        for (int i = 0; i < storyPages.Length; i++)
        {
            storyText.alpha = 0f;
            yield return StartCoroutine(FadeTextAlpha(0f, 1f, fadeInDuration));
            yield return StartCoroutine(TypeText(storyPages[i]));
            yield return new WaitForSeconds(delayBetweenPages);
            yield return StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutDuration));
        }

        storyPanel.SetActive(false);

        // === TAHAP 2: ROLLING CREDITS ===
        creditsPanel.SetActive(true);
        
        // Pastikan posisi awal di bawah layar
        creditsContent.anchoredPosition = new Vector2(0, -creditsContent.rect.height);

        // Berhenti jika posisi Y sudah melewati tinggi layar + tinggi teks sendiri
        float targetY = creditsContent.rect.height + 550f; 

        while (creditsContent.anchoredPosition.y < targetY)
        {
            creditsContent.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        // === TAHAP 3: EXIT ===
        yield return new WaitForSeconds(AutoExitDelay);
        SceneManager.LoadScene(splashScreenSceneName);
    }

    IEnumerator TypeText(string message)
    {
        storyText.text = "";
        foreach (char letter in message)
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeTextAlpha(float start, float end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            storyText.alpha = Mathf.Lerp(start, end, timer / duration);
            yield return null;
        }
        storyText.alpha = end;
    }
}