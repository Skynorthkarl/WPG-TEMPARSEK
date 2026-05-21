using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private bool playerNearby = false;

    public GameObject pressEText;

    void Start()
    {
        pressEText.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        Debug.Log("Item Collected!");

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            pressEText.SetActive(false);
        }
    }
}