using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    public Light2D torchLight;

    void Update()
    {
        torchLight.intensity =
            Random.Range(1.2f, 1.7f);
    }
}