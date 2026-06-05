using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public Transform spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = spawnPoint.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}