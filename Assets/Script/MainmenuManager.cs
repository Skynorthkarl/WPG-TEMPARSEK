using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // WAJIB DIISI untuk mengatur Audio Mixer
using UnityEngine.UI;    // WAJIB DIISI untuk membaca UI Slider

public class MainMenuManager : MonoBehaviour
{
    [Header("Pengaturan Transisi (Fade)")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 1.0f;

    [Header("Pengaturan Scene")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("Panel UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Pengaturan Audio Mixer")]
    [Tooltip("Masukkan file GameMixer kamu ke sini")]
    [SerializeField] private AudioMixer audioMixer;
    [Tooltip("Masukkan komponen Slider Volume dari panel Options ke sini")]
    [SerializeField] private Slider volumeSlider;

    private CanvasGroup mainMenuCanvasGroup;

    void Start()
    {
        if (mainMenuPanel != null)
        {
            mainMenuCanvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
            if (mainMenuCanvasGroup == null)
            {
                mainMenuCanvasGroup = mainMenuPanel.AddComponent<CanvasGroup>();
            }
        }

        ShowMainMenu();

        // Mengatur agar slider mendengarkan fungsi SetVolume saat digeser
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.0001f; // Agar tidak terjadi log(0) yang bikin eror
            volumeSlider.maxValue = 1f;
            volumeSlider.value = PlayerPrefs.GetFloat("VolumeSimpanan", 0.75f); // Ambil data volume terakhir
            
            // Set volume awal
            SetVolume(volumeSlider.value);

            // Hubungkan fungsi geser slider ke kodingan otomatis
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0f;
            StartCoroutine(FadeInSequence());
        }
    }

    // Fungsi khusus untuk mengubah volume berdasarkan nilai Slider (0 sampai 1)
    public void SetVolume(float nilaiSlider)
    {
        // Mengubah nilai linear slider (0-1) menjadi desibel matematika (-80dB sampai 0dB)
        float desibel = Mathf.Log10(nilaiSlider) * 20;
        
        // "NamaVolume" harus sama persis dengan nama di Exposed Parameters Audio Mixer tadi
        audioMixer.SetFloat("NamaVolume", desibel);

        // Simpan preferensi volume pemain agar pas game dibuka lagi warnanya gak reset
        PlayerPrefs.SetFloat("VolumeSimpanan", nilaiSlider);
    }

    public void PlayGame()
    {
        if (menuCanvasGroup != null) StartCoroutine(FadeOutAndLoadGameplay());
        else SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenOptions()
    {
        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.interactable = false;
            mainMenuCanvasGroup.blocksRaycasts = false;
            mainMenuCanvasGroup.alpha = 0.5f;
        }
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
            mainMenuCanvasGroup.alpha = 1f;
        }
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();
    }

    IEnumerator FadeInSequence()
    {
        float counter = 0f;
        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, counter / fadeInDuration);
            yield return null;
        }
        menuCanvasGroup.alpha = 1f;
    }

    IEnumerator FadeOutAndLoadGameplay()
    {
        float counter = 0f;
        while (counter < fadeOutDuration)
        {
            counter += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, counter / fadeOutDuration);
            yield return null;
        }
        menuCanvasGroup.alpha = 0f;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(gameplaySceneName);
    }
}