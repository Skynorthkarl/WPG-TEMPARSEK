using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Spike : MonoBehaviour
{
    private bool isDead = false;

    public ScreenTransition transition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            isDead = true;
            StartCoroutine(KillPlayer(other.gameObject));
        }
    }

    IEnumerator KillPlayer(GameObject player)
    {
        PlayerMovement move =
            player.GetComponent<PlayerMovement>();

        if (move != null)
        {
            move.SetCanMove(false);
        }

        yield return new WaitForSeconds(0.3f);

        if (transition != null)
        {
            yield return StartCoroutine(
                transition.CloseScreen()
            );
        }

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}