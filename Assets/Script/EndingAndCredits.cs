using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingAndCredits : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject storyPanel;
    public GameObject creditsPanel;

    [Header("Story Settings (Typewriter)")]
    public TMP_Text storyText;
    [TextArea(3, 10)]
    public string[] storyPages; 
    public float typingSpeed = 0.05f; 
    public float delayBetweenPages = 3f;
    
    [Header("Fade Settings (Text Only)")]
    public float fadeInDuration = 0.5f;  // Kecepatan teks muncul halus di awal halaman
    public float fadeOutDuration = 0.8f; // Kecepatan teks menghilang halus di akhir halaman

    [Header("Credits Settings (Rolling)")]
    public RectTransform creditsContent; 
    public float scrollSpeed = 50f;      
    public float AutoExitDelay = 3f;     

    [Header("Scene Target")]
    public string splashScreenSceneName = "SplashScreen";

    void Start()
    {
        // Pastikan panel diatur dengan benar di awal
        storyPanel.SetActive(true);
        creditsPanel.SetActive(false);

        if (storyText != null)
        {
            // Set teks awal jadi transparan total (Alpha = 0) agar bisa di-Fade In
            Color textColor = storyText.color;
            textColor.a = 0f;
            storyText.color = textColor;
            storyText.text = "";
        }

        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        // === TAHAP 1: EFEK TYPEWRITER CERITA AKHIR + FADE IN/OUT TEXT ONLY ===
        for (int i = 0; i < storyPages.Length; i++)
        {
            // 1. Fade In Teks Kosong (Biar pas huruf pertama diketik tidak langsung mengagetkan)
            yield return StartCoroutine(FadeTextAlpha(0f, 1f, fadeInDuration));

            // 2. Ketik teks halaman saat ini dengan efek typewriter
            yield return StartCoroutine(TypeText(storyPages[i]));
            
            // Tunggu beberapa detik agar pemain sempat membaca
            yield return new WaitForSeconds(delayBetweenPages);

            // 3. Fade Out Teks saja (Background tetap hitam pekat)
            yield return StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutDuration));
        }

        // Matikan panel cerita setelah semua halaman selesai
        storyText.text = "";
        storyPanel.SetActive(false);

        // === TAHAP 2: ROLLING CREDITS ===
        creditsPanel.SetActive(true);
        
        if (creditsContent != null)
        {
            creditsContent.anchoredPosition = new Vector2(0, -700f); 
        }

        float screenHeight = Screen.height;
        float textHeight = (creditsContent != null && creditsContent.rect.height > 0) ? creditsContent.rect.height : 1500f;
        bool isCreditsFinished = false;
        
        while (!isCreditsFinished)
        {
            creditsContent.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsContent.anchoredPosition.y >= textHeight + 200f)
            {
                isCreditsFinished = true;
            }

            yield return null;
        }

        // === TAHAP 3: KEMBALI KE SPLASH SCREEN ===
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

    // Fungsi khusus untuk memudarkan atau memunculkan HANYA warna teks TextMeshPro
    IEnumerator FadeTextAlpha(float startAlpha, float targetAlpha, float duration)
    {
        if (storyText == null) yield break;

        float timer = 0f;
        Color textColor = storyText.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            // Mengubah nilai alpha (transparansi) warna teks secara halus
            textColor.a = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            storyText.color = textColor;
            yield return null;
        }

        // Pastikan nilai akhir benar-benar pas di target
        textColor.a = targetAlpha;
        storyText.color = textColor;
    }
}