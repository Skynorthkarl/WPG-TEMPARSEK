using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk perpindahan scene

public class MainMenuManager : MonoBehaviour
{
    [Header("Pengaturan Panel UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Pengaturan Transisi")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private float fadeInDuration = 1.0f;

    [Header("Nama Scene Tujuan")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    void Start()
    {
        // Memastikan panel berada di posisi yang benar saat awal masuk menu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        // Jika memakai Canvas Group, jalankan efek memudar masuk (Fade In)
        if (menuCanvasGroup != null)
        {
            StartCoroutine(FadeInMenu());
        }
    }

    // Fungsi untuk tombol START GAME / PLAY
    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Fungsi untuk tombol OPTIONS (Membuka menu opsi)
    public void OpenOptions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Fungsi untuk tombol BACK (Kembali dari opsi ke menu utama)
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    // Fungsi untuk tombol QUIT GAME / EXIT
    public void QuitGame()
    {
        Debug.Log("Game Keluar! (Fungsi ini berjalan saat game sudah di-build)");
        Application.Quit();
    }

    // Coroutine untuk transisi Fade In
    IEnumerator FadeInMenu()
    {
        float counter = 0f;
        menuCanvasGroup.alpha = 0f;
        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, counter / fadeInDuration);
            yield return null;
        }
        menuCanvasGroup.alpha = 1f;
    }
}