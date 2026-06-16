using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicCutsceneManager : MonoBehaviour
{
    [Header("Kumpulan Panel Komik")]
    [Tooltip("Masukkan objek Panel 1 sampai 6 di sini secara berurutan sesuai urutan muncul")]
    [SerializeField] private GameObject[] comicPanels;

    [Header("Pengaturan Transisi Memudar (Fade)")]
    [Tooltip("Berapa detik durasi memudar masuk (fade-in) untuk setiap gambar")]
    [SerializeField] private float fadeDuration = 0.8f;

    [Tooltip("Ceklis jika ingin panel komik muncul otomatis satu per satu tanpa perlu diklik pemain")]
    [SerializeField] private bool autoPlay = false;

    [Tooltip("Jeda waktu tunggu (detik) sebelum panel berikutnya muncul otomatis (jika Auto Play aktif)")]
    [SerializeField] private float autoPlayDelay = 3.0f;

    [Header("Pengaturan Khusus Panel Terakhir")]
    [Tooltip("Tentukan apakah panel terakhir akan menyembunyikan panel lain dan memenuhi layar (Fullscreen)")]
    [SerializeField] private bool makeLastPanelFullscreen = true;
    [Tooltip("Masukkan objek folder penampung panel komik kecil agar bisa disembunyikan saat panel terakhir muncul")]
    [SerializeField] private GameObject comicPanelsParent;

    [Header("Efek Suara (SFX)")]
    [Tooltip("Masukkan komponen AudioSource yang akan memutar suara")]
    [SerializeField] private AudioSource sfxSource;
    [Tooltip("Masukkan file audio klik/letupan komik (.mp3/.wav)")]
    [SerializeField] private AudioClip popSound;
    [Tooltip("Suara khusus yang lebih dramatis untuk adegan klimaks panel terakhir (opsional)")]
    [SerializeField] private AudioClip lastPanelSpecialSound;

    [Header("Nama Scene Selanjutnya")]
    [Tooltip("Nama scene gameplay utama kamu secara persis")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    private int currentPanelIndex = -1;
    private CanvasGroup[] panelCanvasGroups;
    private Coroutine activeFadeCoroutine;
    private Coroutine autoPlayCoroutine;
    private bool isTransitioning = false; // Mencegah klik ganda merusak transisi fade

    void Start()
    {
        // Menyiapkan array penyimpanan komponen Canvas Group untuk memudarkan gambar
        panelCanvasGroups = new CanvasGroup[comicPanels.Length];

        for (int i = 0; i < comicPanels.Length; i++)
        {
            if (comicPanels[i] != null)
            {
                // Ambil atau tambahkan komponen CanvasGroup secara otomatis agar praktis di Unity
                CanvasGroup cg = comicPanels[i].GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = comicPanels[i].AddComponent<CanvasGroup>();
                }
                panelCanvasGroups[i] = cg;
                
                // Mulai dalam keadaan mati (transparan penuh)
                cg.alpha = 0f;
                comicPanels[i].SetActive(false);
            }
        }

        // Pastikan kontainer panel kecil aktif di awal
        if (comicPanelsParent != null)
        {
            comicPanelsParent.SetActive(true);
        }

        // Tampilkan panel pertama secara otomatis di awal scene
        NextPanel();

        // Jika fitur Auto Play aktif, jalankan coroutine hitung mundur otomatis
        if (autoPlay)
        {
            autoPlayCoroutine = StartCoroutine(AutoPlaySequence());
        }
    }

    /// <summary>
    /// Fungsi utama untuk memunculkan panel selanjutnya.
    /// Pasang fungsi ini pada tombol klik layar penuh (FullScreenButton).
    /// </summary>
    public void NextPanel()
    {
        // Jika gambar sebelumnya masih dalam proses memudar, abaikan input klik sejenak
        if (isTransitioning) return;

        currentPanelIndex++;

        // Jika semua panel sudah terbuka dan diklik sekali lagi, langsung masuk gameplay
        if (currentPanelIndex >= comicPanels.Length)
        {
            LoadGameplayScene();
            return;
        }

        // Deteksi apakah ini merupakan panel terakhir (panel ke-6)
        bool isLastPanel = (currentPanelIndex == comicPanels.Length - 1);

        if (isLastPanel && makeLastPanelFullscreen)
        {
            // Sembunyikan folder penampung panel komik kecil agar layar menjadi bersih
            if (comicPanelsParent != null)
            {
                comicPanelsParent.SetActive(false);
            }

            // Aktifkan panel terakhir (panel ke-6)
            if (comicPanels[currentPanelIndex] != null)
            {
                comicPanels[currentPanelIndex].SetActive(true);
                
                // Memaksa RectTransform panel terakhir memenuhi seluruh layar (Stretch Full)
                RectTransform rect = comicPanels[currentPanelIndex].GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }
            }

            // Mainkan suara spesial adegan klimaks
            if (sfxSource != null && lastPanelSpecialSound != null)
            {
                sfxSource.PlayOneShot(lastPanelSpecialSound);
            }
            else if (sfxSource != null && popSound != null)
            {
                sfxSource.PlayOneShot(popSound);
            }
        }
        else
        {
            // Untuk panel biasa (panel 1 sampai 5), aktifkan di posisinya masing-masing
            if (comicPanels[currentPanelIndex] != null)
            {
                comicPanels[currentPanelIndex].SetActive(true);
            }

            // Mainkan suara pop standar
            if (sfxSource != null && popSound != null)
            {
                sfxSource.PlayOneShot(popSound);
            }
        }

        // Jalankan animasi memudar masuk (Fade In)
        if (panelCanvasGroups[currentPanelIndex] != null)
        {
            if (activeFadeCoroutine != null)
            {
                StopCoroutine(activeFadeCoroutine);
            }
            activeFadeCoroutine = StartCoroutine(FadeInPanel(panelCanvasGroups[currentPanelIndex]));
        }
    }

    // Coroutine internal untuk menaikkan kejelasan gambar (Alpha 0 ke 1) secara bertahap
    private IEnumerator FadeInPanel(CanvasGroup cg)
    {
        isTransitioning = true;
        float counter = 0f;
        cg.alpha = 0f;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
        isTransitioning = false;
    }

    // Coroutine hitung mundur otomatis untuk memicu panel berikutnya secara mandiri
    private IEnumerator AutoPlaySequence()
    {
        // Berikan jeda awal setelah panel 1 muncul di awal
        yield return new WaitForSeconds(autoPlayDelay);

        while (currentPanelIndex < comicPanels.Length)
        {
            // Tunggu sampai animasi memudar saat ini selesai sebelum lanjut menghitung jeda berikutnya
            while (isTransitioning)
            {
                yield return null;
            }

            NextPanel();
            yield return new WaitForSeconds(autoPlayDelay);
        }
    }

    /// <summary>
    /// Fungsi untuk melewati seluruh cutscene secara instan.
    /// Pasang fungsi ini pada tombol Skip (SkipButton).
    /// </summary>
    public void SkipAll()
    {
        // Matikan semua coroutine aktif agar tidak terjadi tabrakan memori saat pindah scene
        if (autoPlayCoroutine != null) StopCoroutine(autoPlayCoroutine);
        if (activeFadeCoroutine != null) StopCoroutine(activeFadeCoroutine);

        LoadGameplayScene();
    }

    private void LoadGameplayScene()
    {
        if (!string.IsNullOrEmpty(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogError("Nama Gameplay Scene belum diisi!");
        }
    }
}