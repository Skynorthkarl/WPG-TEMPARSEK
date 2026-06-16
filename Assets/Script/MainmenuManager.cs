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

        // ================= BARIS PENGATUR VOLUME YANG DIUBAH =================
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.0001f; 
            volumeSlider.maxValue = 1f;
            
            // 1. Ubah default 0.75f menjadi 1f agar suara langsung kencang di awal
            float defaultVolume = 1f; 
            float savedVolume = PlayerPrefs.GetFloat("VolumeSimpanan", defaultVolume);
            
            volumeSlider.value = savedVolume;

            // 2. Langsung tembak volume ke Audio Mixer tanpa nunggu slider digeser
            SetVolume(savedVolume);

            // 3. Hubungkan fungsi geser slider
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        // =====================================================================

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0f;
            StartCoroutine(FadeInSequence());
        }
    }

    // Fungsi khusus untuk mengubah volume berdasarkan nilai Slider (0 sampai 1)
    public void SetVolume(float nilaiSlider)
    {
        if (audioMixer == null) return;

        // Mengubah nilai linear slider (0-1) menjadi desibel matematika (-80dB sampai 0dB)
        // Nilai 1 akan menjadi 0dB (suara asli/kencang)
        float desibel = Mathf.Log10(nilaiSlider) * 20;
        
        // Pastikan "NamaVolume" sudah di-Exposed di Audio Mixer kamu
        audioMixer.SetFloat("NamaVolume", desibel);

        // Simpan preferensi volume pemain
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