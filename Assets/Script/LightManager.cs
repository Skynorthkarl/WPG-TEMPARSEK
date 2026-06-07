using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public Light2D globalLight;

    public void MakeDark()
    {
        globalLight.intensity = 0.005f;
    }
}