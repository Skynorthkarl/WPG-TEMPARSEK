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
        // Lock player
        player.SetCanMove(false);

        // Stop camera follow
        cameraFollow.canFollow = false;

        // Kamera ke posisi intro
        cam.transform.position = new Vector3(
            introPoint.position.x,
            introPoint.position.y,
            -10f
        );

        // Zoom jauh
        cam.orthographicSize = startSize;

        // Tunggu layar hitam
        yield return new WaitForSeconds(openDelay);

        // Buka layar
        yield return StartCoroutine(
            transition.OpenScreen()
        );

        // Tampilkan ruangan
        yield return new WaitForSeconds(roomViewDelay);

        // Zoom perlahan
        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;

            cam.orthographicSize = Mathf.Lerp(
                startSize,
                endSize,
                timer / zoomDuration
            );

            yield return null;
        }

        // Geser ke player
        Vector3 startPos = cam.transform.position;

        Vector3 targetPos = new Vector3(
            playerTarget.position.x,
            playerTarget.position.y,
            -10f
        );

        timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            cam.transform.position = Vector3.Lerp(
                startPos,
                targetPos,
                timer / moveDuration
            );

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Aktifkan follow kamera
        cameraFollow.canFollow = true;

        // Unlock player
        player.SetCanMove(true);

        Debug.Log("INTRO SELESAI");
    }
}