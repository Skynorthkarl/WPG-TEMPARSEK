using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    private Light2D torchLight;
    private AudioSource torchAudio;

    void Start()
    {
        torchLight = GetComponent<Light2D>();
        torchAudio = GetComponent<AudioSource>();

        if (torchAudio != null)
            torchAudio.Stop();
    }

    public void StartTorch()
    {
        if (torchAudio != null)
            torchAudio.Play();
    }

    void Update()
    {
        torchLight.intensity =
            Random.Range(1.2f, 1.7f);
    }
}