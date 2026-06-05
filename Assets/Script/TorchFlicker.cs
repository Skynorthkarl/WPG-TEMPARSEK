using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    private Light2D torchLight;

    void Start()
    {
        torchLight = GetComponent<Light2D>();
    }

    void Update()
    {
        torchLight.intensity =
            Random.Range(1.2f, 1.7f);
    }
}