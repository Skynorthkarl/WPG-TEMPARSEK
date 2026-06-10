using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitDoor : MonoBehaviour
{
    public Transform enterPoint;
    public GameObject enterText;
    public ScreenTransition transition;
    public string nextScene;
    private bool playerNearby;
    private GameObject player;

    void Update()
    {
        if (playerNearby &&
            Input.GetKeyDown(KeyCode.E) &&
            GameManager.instance.itemCount >= GameManager.instance.totalItems)
        {
            StartCoroutine(EnterDoor());
        }
    }

    IEnumerator EnterDoor()
    {
        enterText.SetActive(false);

        PlayerMovement move =
            player.GetComponent<PlayerMovement>();

        if (move != null)
        {
            move.enabled = false;
        }

        // Jalan ke titik pintu
        while (Vector2.Distance(
            player.transform.position,
            enterPoint.position) > 0.05f)
        {
            player.transform.position =
                Vector2.MoveTowards(
                    player.transform.position,
                    enterPoint.position,
                    3f * Time.deltaTime);

            yield return null;
        }

        // Animasi masuk pintu (mengecil)
        float duration = 1f;
        float timer = 0f;

        Vector3 startScale =
            player.transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            player.transform.localScale =
                Vector3.Lerp(
                    startScale,
                    Vector3.zero,
                    timer / duration);

            yield return null;
        }

        // JANGAN langsung matikan player
        // supaya obor masih menyala

        yield return StartCoroutine(
            transition.CloseScreen()
        );

        SceneManager.LoadScene(nextScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.gameObject;

            if (GameManager.instance.itemCount >=
                GameManager.instance.totalItems)
            {
                enterText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            enterText.SetActive(false);
        }
    }
}