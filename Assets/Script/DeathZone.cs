using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    private bool isDead = false;

    public ScreenTransition transition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            isDead = true;

            StartCoroutine(RestartLevel(other.gameObject));
        }
    }

    IEnumerator RestartLevel(GameObject player)
    {
        // Matikan kontrol player
        PlayerMovement move =
            player.GetComponent<PlayerMovement>();

        if (move != null)
        {
            move.SetCanMove(false);
        }

        // Tunggu sebentar biar terasa jatuh
        yield return new WaitForSeconds(0.5f);

        // Animasi layar menutup
        if (transition != null)
        {
            yield return StartCoroutine(
                transition.CloseScreen()
            );
        }

        // Reload level saat ini
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}