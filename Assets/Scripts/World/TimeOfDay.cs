using UnityEngine;
using UnityEngine.UIElements;

public class TimeOfDay : MonoBehaviour
{
    public float cycleLengthInMinutes = 1.0f;
    public float sunBaseIntensity = 1f;
    public float sunVariation = 1.0f;
    public float DayNumber = 0;
    public float transitionTime;
    public Transform sunPosition;
    public Light sun;
    public Material sky;
    public Material clouds;

    [Header("Clear")]
    public Gradient topClear;
    public Gradient midClear;
    public Gradient bottomClear;
    public Gradient cloudClear;
    public float cloudAlphaClear;

    [Header("Mist")]
    public Gradient topMist;
    public Gradient midMist;
    public Gradient bottomMist;
    public Gradient cloudMist;
    public float cloudAlphaMist;

    [Header("Green")]
    public Gradient topGreen;
    public Gradient midGreen;
    public Gradient bottomGreen;
    public Gradient cloudGreen;
    public float cloudAlphaGreen;

    [Header("Dust")]
    public Gradient topDust;
    public Gradient midDust;
    public Gradient bottomDust;
    public Gradient cloudDust;
    public float cloudAlphaDust;

    [Header("Black")]
    public Gradient topBlack;
    public Gradient midBlack;
    public Gradient bottomBlack;
    public Gradient cloudBlack;
    public float cloudAlphaBlack;

    private Color oldTopColor;
    private Color oldMidColor;
    private Color oldBottomColor;
    private Color oldCloudColor;
    private float oldCloudAlpha;
    [Range(0f, 1f)]
    private float timeOfDay;
    [Range(0f, 1f)]
    private float cycleOfDay;
    private float timeScale = 100f;
    private float intensity;

    public void SetSkyColor(Gradient topColor, Gradient midColor, Gradient bottomColor)
    {
        oldTopColor = sky.GetColor("_UpColor");
        sky.SetColor("_UpColor", Color.Lerp(oldTopColor, 
        topColor.Evaluate(cycleOfDay), 
        Time.deltaTime / transitionTime));

        oldMidColor = sky.GetColor("_MidColor");
        sky.SetColor("_MidColor", Color.Lerp(oldMidColor, 
        midColor.Evaluate(cycleOfDay), 
        Time.deltaTime / transitionTime));

        oldBottomColor = sky.GetColor("_DownColor");
        sky.SetColor("_DownColor", Color.Lerp(oldBottomColor, 
        bottomColor.Evaluate(cycleOfDay), 
        Time.deltaTime / transitionTime));
    }

    public void SetClouds(Gradient color, float alpha)
    {
        oldCloudColor = clouds.GetColor("_Color");
        clouds.SetColor("_Color", Color.Lerp(oldCloudColor, 
        color.Evaluate(cycleOfDay), 
        Time.deltaTime / transitionTime));

        oldCloudAlpha = clouds.GetFloat("_Alpha");
        clouds.SetFloat("_Alpha", Mathf.Lerp(oldCloudAlpha, alpha, 
        Time.deltaTime / transitionTime));
    }

    void UpdateTimeScale()
    {
        timeScale = 24 / (cycleLengthInMinutes / 60);
    }

    void UpdateTime()
    {
        timeOfDay += Time.deltaTime * timeScale / 86400;
        if (timeOfDay > 1) { timeOfDay -= 1f; DayNumber++; }

        float shifted = Mathf.Repeat(timeOfDay - 0.25f, 1f); // shifted timeOfDay
        cycleOfDay = 1f - Mathf.Abs(shifted * 2f - 1f); // triangle wave
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

    void Update()
    {
        UpdateTimeScale();
        UpdateTime();
        RotateSun();
        SunIntensity();;
    }

    public float GetTimeOfDay()
    {
        return cycleOfDay;
    }
}
