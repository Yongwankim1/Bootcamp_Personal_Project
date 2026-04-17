using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingController : MonoBehaviour
{
    [SerializeField] Light2D globalLighting;
    [SerializeField] float lightingIntensity;
    [SerializeField] float minIntensity;

    private void Update()
    {
        float t = TimeLightingController.Instance.TimeLightingValue
                  / TimeLightingController.Instance.CycleDuration;

        float pingpong = Mathf.PingPong(t * 2f, 1f);

        //if(pingpong <= 0.25f)
        //{
        //    pingpong = minIntensity;
        //}
        lightingIntensity = Mathf.Lerp(minIntensity, 1f, pingpong);

        globalLighting.intensity = lightingIntensity;
    }
}