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

    [Header("Tutorial Script Reference")]
    public TutorialIntro tutorialIntro;

    [Header("Audio")]
    public AudioSource bgm;
    public AudioSource torchAudio;

    [Header("Zoom Settings")]
    public float startSize = 15f;
    public float endSize = 5f;

    [Header("Timing")]
    public float roomViewDelay = 1f;
    public float zoomDuration = 1.5f;
    public float moveDuration = 1f;

    void Start()
    {
        StartCoroutine(MainFlowSequence());
    }

    IEnumerator MainFlowSequence()
    {
        // 1. Kunci pergerakan player dan kamera di awal
        if (player != null) player.SetCanMove(false);
        if (cameraFollow != null) cameraFollow.canFollow = false;

        // Set posisi kamera awal ke titik intro
        if (cam != null && introPoint != null)
        {
            cam.transform.position = new Vector3(introPoint.position.x, introPoint.position.y, -10f);
            cam.orthographicSize = startSize;
        }

        // 2. Jalankan Teks Tutorial Terlebih Dahulu dan tunggu sampai SELESAI (Pemain pencet Enter di halaman terakhir)
        if (tutorialIntro != null)
        {
            yield return StartCoroutine(tutorialIntro.StartTutorialSequence());
        }

        // 3. Setelah teks selesai, baru buka Layar Transisi Hitamnya (Fade In Level)
        if (transition != null)
        {
            Debug.Log("OPEN SCREEN - LEVEL INTRO DIMULAI");
            yield return StartCoroutine(transition.OpenScreen());
        }

        yield return new WaitForSeconds(roomViewDelay);

        // 4. Efek Kamera Zoom Out ke Zoom In
        float timer = 0f;
        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(startSize, endSize, timer / zoomDuration);
            yield return null;
        }

        // 5. Efek Kamera Geser ke Player
        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = new Vector3(playerTarget.position.x, playerTarget.position.y, -10f);
        timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, timer / moveDuration);
            yield return null;
        }

        // 6. Selesai! Berikan kontrol ke player dan nyalakan musik
        if (cameraFollow != null) cameraFollow.canFollow = true;
        if (player != null) player.SetCanMove(true);

        if (bgm != null) bgm.Play();
        if (torchAudio != null) torchAudio.Play();

        Debug.Log("ALUR INTRO SELESAI - PLAYER BISA BERMAIN");
    }
}