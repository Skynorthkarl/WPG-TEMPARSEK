using TMPro;
using UnityEngine;
using System.Collections;

public class TutorialIntro : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text contentText;
    public TMP_Text enterText;
    public GameObject tutorialPanel;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private int page = 0;
    private bool isWaitingForInput = false;

    // Diganti menjadi fungsi publik yang dikontrol oleh LevelIntro
    public IEnumerator StartTutorialSequence()
    {
        tutorialPanel.SetActive(true);

        // Reset Alpha Text ke 0
        Color contentColor = contentText.color; contentColor.a = 0; contentText.color = contentColor;
        Color enterColor = enterText.color; enterColor.a = 0; enterText.color = enterColor;

        StartCoroutine(BlinkEnter());

        // Munculkan panel teks perlahan (Fade In)
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / 1f);

            contentColor.a = alpha; contentText.color = contentColor;
            enterColor.a = alpha; enterText.color = enterColor;
            yield return null;
        }

        ShowPage();

        // Tunggu di sini sampai pemain menyelesaikan semua halaman teks
        isWaitingForInput = true;
        while (isWaitingForInput)
        {
            yield return null;
        }

        // Hilangkan panel teks perlahan (Fade Out)
        timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / 0.5f);

            contentColor.a = alpha; contentText.color = contentColor;
            enterColor.a = alpha; enterText.color = enterColor;
            yield return null;
        }

        tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (!isWaitingForInput) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (page < 2)
            {
                page++;
                ShowPage();
            }
            else
            {
                // Halaman habis, lanjut ke tahap berikutnya di LevelIntro
                isWaitingForInput = false;
            }
        }
    }

    void ShowPage()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (page == 0)
        {
            contentText.text = "A / D = Bergerak\nW = Lompat\nE = Interaksi";
        }
        else if (page == 1)
        {
            typingCoroutine = StartCoroutine(TypeText("Kumpulkan seluruh kunci\nyang tersebar di dalam rumah.\n\nPintu keluar akan terbuka\nsetelah semua kunci ditemukan."));
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeText("Tidak semua kunci yang kau temukan\nakan membantumu.\n\nHindari bahaya dan\ntetap waspada."));
        }
    }

    IEnumerator TypeText(string message)
    {
        contentText.text = "";
        foreach (char letter in message)
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator BlinkEnter()
    {
        while (true)
        {
            enterText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            enterText.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}