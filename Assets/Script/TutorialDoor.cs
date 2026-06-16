using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialDoor : MonoBehaviour
{
    [Header("Scene Target")]
    [Tooltip("Ketik nama scene Level 1 kamu di sini persis seperti di Unity")]
    public string nextSceneName = "Level1"; 

    [Header("References")]
    [Tooltip("Titik tengah pintu tempat player akan berjalan otomatis")]
    public Transform enterPoint;
    [Tooltip("Masukkan objek teks (misal: tulisan 'Tekan E untuk Masuk')")]
    public GameObject interactPromptText; 
    [Tooltip("Masukkan objek Screen Transition yang ada di scene Tutorial")]
    public ScreenTransition transition;

    private bool playerIsNear = false;
    private GameObject playerObj;
    private bool isEntering = false; // Mencegah pencet E berkali-kali saat animasi berjalan

    void Start()
    {
        // Awal game, tulisan "Tekan E" disembunyikan
        if (interactPromptText != null) 
            interactPromptText.SetActive(false);
    }

    void Update()
    {
        // Hanya bisa masuk jika player di dekat pintu, menekan E, dan tidak sedang dalam proses masuk
        if (playerIsNear && Input.GetKeyDown(KeyCode.E) && !isEntering)
        {
            StartCoroutine(EnterDoorSequence());
        }
    }

    IEnumerator EnterDoorSequence()
    {
        isEntering = true;
        
        // Sembunyikan tulisan "Tekan E"
        if (interactPromptText != null) 
            interactPromptText.SetActive(false);

        // Matikan kontrol pergerakan player
        PlayerMovement move = playerObj.GetComponent<PlayerMovement>();
        if (move != null)
        {
            move.SetCanMove(false); // Menggunakan fungsi SetCanMove sesuai script kamu sebelumnya
        }

        // 1. Player berjalan otomatis ke titik tengah pintu (enterPoint)
        if (enterPoint != null)
        {
            while (Vector2.Distance(playerObj.transform.position, enterPoint.position) > 0.05f)
            {
                playerObj.transform.position = Vector2.MoveTowards(
                    playerObj.transform.position,
                    enterPoint.position,
                    3f * Time.deltaTime
                );
                yield return null;
            }
        }

        // 2. Animasi masuk pintu (Skala badan Player mengecil menjadi 0)
        float duration = 1f;
        float timer = 0f;
        Vector3 startScale = playerObj.transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            playerObj.transform.localScale = Vector3.Lerp(
                startScale,
                Vector3.zero,
                timer / duration
            );
            yield return null;
        }

        // 3. Layar menutup menjadi hitam (Fade Out)
        if (transition != null)
        {
            yield return StartCoroutine(transition.CloseScreen());
        }

        // 4. Pindah ke Scene Level 1
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEntering)
        {
            playerIsNear = true;
            playerObj = collision.gameObject;
            
            // Munculkan tulisan "Tekan E" saat mendekat
            if (interactPromptText != null) 
                interactPromptText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEntering)
        {
            playerIsNear = false;

            // Sembunyikan tulisan "Tekan E" saat menjauh
            if (interactPromptText != null) 
                interactPromptText.SetActive(false);
        }
    }
}