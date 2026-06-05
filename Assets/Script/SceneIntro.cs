using UnityEngine;
using System.Collections;

public class SceneIntro : MonoBehaviour
{
    public ScreenTransition transition;

    IEnumerator Start()
    {
        yield return StartCoroutine(
            transition.OpenScreen()
        );
    }
}