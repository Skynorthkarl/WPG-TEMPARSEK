using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour
{
    public RectTransform topPanel;
    public RectTransform bottomPanel;
    public RectTransform leftPanel;
    public RectTransform rightPanel;

    public float duration = 1f;

    public IEnumerator CloseScreen()
    {
        float timer = 0f;

        float targetHeight = Screen.height / 2f;
        float targetWidth = Screen.width / 2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            topPanel.sizeDelta =
                new Vector2(topPanel.sizeDelta.x,
                Mathf.Lerp(0, targetHeight, t));

            bottomPanel.sizeDelta =
                new Vector2(bottomPanel.sizeDelta.x,
                Mathf.Lerp(0, targetHeight, t));

            leftPanel.sizeDelta =
                new Vector2(
                Mathf.Lerp(0, targetWidth, t),
                leftPanel.sizeDelta.y);

            rightPanel.sizeDelta =
                new Vector2(
                Mathf.Lerp(0, targetWidth, t),
                rightPanel.sizeDelta.y);

            yield return null;
        }
    }

    public IEnumerator OpenScreen() {
    float timer = 0f;
    float startHeight = Screen.height / 2f;
    float startWidth = Screen.width / 2f;

    while (timer < duration) {
        timer += Time.deltaTime;
        float t = timer / duration;

        topPanel.sizeDelta =
            new Vector2(
                topPanel.sizeDelta.x,
                Mathf.Lerp(startHeight, 0, t));

        bottomPanel.sizeDelta =
            new Vector2(
                bottomPanel.sizeDelta.x,
                Mathf.Lerp(startHeight, 0, t));

        leftPanel.sizeDelta =
            new Vector2(
                Mathf.Lerp(startWidth, 0, t),
                leftPanel.sizeDelta.y);

        rightPanel.sizeDelta =
            new Vector2(
                Mathf.Lerp(startWidth, 0, t),
                rightPanel.sizeDelta.y);

        yield return null;
    }
    }
}