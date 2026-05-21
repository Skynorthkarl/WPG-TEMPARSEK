using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("Pengaturan UI")]
    [SerializeField] private CanvasGroup introCanvasGroup; // Hubungkan Canvas Group ke sini

    [Header("Pengaturan Waktu (Detik)")]
    [SerializeField] private float fadeInDuration = 1.5f;  // Durasi muncul (transparan ke jelas)
    [SerializeField] private float stayDuration = 2.0f;    // Durasi diam/menahan layar
    [SerializeField] private float fadeOutDuration = 1.5f; // Durasi memudar (jelas ke transparan)

    [Header("Pengaturan Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (introCanvasGroup != null)
        {
            // Memulai rangkaian animasi fade dan perpindahan scene
            StartCoroutine(IntroSequence());
        }
        else
        {
            Debug.LogError("Waduh! Canvas Group belum dipasang di Inspector IntroManager!");
        }
    }

    IEnumerator IntroSequence()
    {
        // 1. FADE IN (Layar memunculkan logo persegi pelan-pelan)
        float counter = 0f;
        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(0f, 1f, counter / fadeInDuration);
            yield return null; // Tunggu ke frame berikutnya
        }
        introCanvasGroup.alpha = 1f; // Pastikan penuh

        // 2. STAY (Menahan layar agar logo persegi tetap terlihat jelas)
        yield return new WaitForSeconds(stayDuration);

        // 3. FADE OUT (Layar memudarkan logo persegi kembali ke transparan)
        counter = 0f;
        while (counter < fadeOutDuration)
        {
            counter += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(1f, 0f, counter / fadeOutDuration);
            yield return null;
        }
        introCanvasGroup.alpha = 0f; // Pastikan benar-benar transparan

        // Beri jeda sedikit setelah fade out agar transisinya manis
        yield return new WaitForSeconds(0.5f);

        // 4. PINDAH SCENE
        SceneManager.LoadScene(mainMenuSceneName);
    }
}