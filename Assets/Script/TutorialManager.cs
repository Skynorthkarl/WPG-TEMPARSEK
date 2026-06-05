using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialText;

    private bool moved = false;
    private bool jumped = false;

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // A/D
        if (!moved && move != 0)
        {
            moved = true;
            tutorialText.text = "W = Lompat";
        }

        // W
        if (moved && !jumped && Input.GetKeyDown(KeyCode.W))
        {
            jumped = true;
            tutorialText.text = "E = Ambil Item";
        }
    }

    // Dipanggil saat item tutorial diambil
    public void ItemCollected()
    {
        tutorialText.text =
            "Masuk ke pintu untuk memulai permainan";
    }
}