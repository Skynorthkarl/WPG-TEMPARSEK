using UnityEngine;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    public Camera cam;

    public CameraFollow cameraFollow;
    public PlayerMovement player;

    public Transform introPoint;
    public Transform playerTarget;

    public ScreenTransition transition;

    [Header("Zoom")]
    public float startSize = 15f;
    public float endSize = 5f;

    [Header("Timing")]
    public float openDelay = 1f;
    public float roomViewDelay = 2f;
    public float zoomDuration = 2f;
    public float moveDuration = 2f;

    IEnumerator Start()
    {
        // Kunci player
        player.canMove = false;
        Debug.Log("LOCK PLAYER");

        // Matikan camera follow
        Debug.Log("UNLOCK PLAYER");
        cameraFollow.canFollow = false;

        // Pindah kamera ke titik intro
        cam.transform.position =
            new Vector3(
                introPoint.position.x,
                introPoint.position.y,
                -10f
            );

        // Zoom jauh
        cam.orthographicSize = startSize;

        // Tunggu layar hitam sebentar
        yield return new WaitForSeconds(openDelay);

        // Animasi buka layar
        yield return StartCoroutine(
            transition.OpenScreen()
        );

        // Diam dulu menampilkan ruangan
        yield return new WaitForSeconds(roomViewDelay);

        // Zoom perlahan
        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;

            cam.orthographicSize =
                Mathf.Lerp(
                    startSize,
                    endSize,
                    timer / zoomDuration
                );

            yield return null;
        }

        // Geser kamera ke player
        Vector3 startPos =
            cam.transform.position;

        Vector3 targetPos =
            new Vector3(
                playerTarget.position.x,
                playerTarget.position.y,
                -10f
            );

        timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            cam.transform.position =
                Vector3.Lerp(
                    startPos,
                    targetPos,
                    timer / moveDuration
                );

            yield return null;
        }

        // Aktifkan camera follow
        cameraFollow.canFollow = true;

        // Baru player bisa bergerak
        player.canMove = true;
    }
}