using TMPro;
using UnityEngine;
using System.Collections;

public class TutorialIntro : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text contentText;
    public TMP_Text pageText;

    public GameObject tutorialPanel;

    int page = 0;

    void Start()
    {
        ShowPage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (page < 2)
            {
                page++;
                ShowPage();
            }
            else
            {
                StartCoroutine(StartGame());
            }
        }
    }

    void ShowPage()
    {
        if (page == 0)
        {
            titleText.text = "KONTROL";

            contentText.text =
                "A / D = Bergerak\n" +
                "W = Lompat\n" +
                "E = Interaksi";

            pageText.text = "1 / 3";
        }
        else if (page == 1)
        {
            titleText.text = "MISI";

            contentText.text =
                "Kumpulkan seluruh item\n" +
                "yang tersebar di dalam rumah.\n\n" +
                "Pintu keluar akan terbuka\n" +
                "setelah semua item ditemukan.";

            pageText.text = "2 / 3";
        }
        else
        {
            titleText.text = "PERINGATAN";

            contentText.text =
                "Tidak semua yang kau temukan\n" +
                "akan membantumu.\n\n" +
                "Hindari bahaya dan\n" +
                "tetap waspada.";

            pageText.text = "3 / 3";
        }
    }

    IEnumerator StartGame()
    {
        tutorialPanel.SetActive(false);

        yield return null;
    }
}