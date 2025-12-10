using System.ComponentModel;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    public float cycleLengthInMinutes = 0.5f;
    public float DayNumber = 0;
    public Transform sunPosition;
    public Light sun;
    public float sunBaseIntensity = 1f;
    public float sunVariation = 1.5f;
    public Gradient sunColor;
    public Gradient fogColor;
    //public AnimationCurve fogDensity;
    [Range(0f, 1f)]
    private float timeOfDay;
    private float timeScale = 100f;
    private float intensity;

    void UpdateTimeScale()
    {
        timeScale = 24 / (cycleLengthInMinutes / 60);
    }

    void UpdateTime()
    {
        timeOfDay += Time.deltaTime * timeScale / 86400;
        if (timeOfDay > 1) { timeOfDay -= 1f; DayNumber++; }
    }

    void RotateSun()
    {
        float sunAngle = timeOfDay * 360;
        sunPosition.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));
    }

    void SunIntensity()
    {
        intensity = Vector3.Dot(sunPosition.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sun.intensity = intensity * sunVariation + sunBaseIntensity;
    }

    void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeScale();
        UpdateTime();
        RotateSun();
        SunIntensity();
        //AdjustSunColor();

        RenderSettings.fogColor = fogColor.Evaluate(timeOfDay);
        //RenderSettings.fogDensity = fogDensity.Evaluate(timeOfDay);
    }
}
