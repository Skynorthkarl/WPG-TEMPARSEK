using UnityEngine;
using System.Collections;

public class BreakableFloor : MonoBehaviour
{
    private bool triggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered &&
            collision.gameObject.CompareTag("Player"))
        {
            triggered = true;

            StartCoroutine(BreakFloor());
        }
    }

    IEnumerator BreakFloor()
    {
        yield return new WaitForSeconds(0.25f);

        GetComponent<Rigidbody2D>().bodyType =
            RigidbodyType2D.Dynamic;
    }
}