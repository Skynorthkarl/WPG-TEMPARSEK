using UnityEngine;
using System.Collections;

public class FakeItem : MonoBehaviour
{
    private bool playerNearby = false;
    private bool used = false;

    public GameObject pressEText;

    public GameObject jumpScareImage;
    public GameObject scaryText;

    public LightManager lightManager;
    public PlayerMovement player;

    void Start() {
        pressEText.SetActive(false);
    }

    void Update() {
        if (playerNearby &&
            !used &&
            Input.GetKeyDown(KeyCode.E)) {
            used = true;
            StartCoroutine(FakeItemEvent());
        }
    }

    IEnumerator FakeItemEvent()
    {
        pressEText.SetActive(false);
        player.SetCanMove(false);
        jumpScareImage.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        jumpScareImage.SetActive(false);
        lightManager.MakeDark();
        scaryText.SetActive(true);
        yield return new WaitForSeconds(2f);
        scaryText.SetActive(false);
        player.SetCanMove(true);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerNearby = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerNearby = false;
            pressEText.SetActive(false);
        }
    }
}