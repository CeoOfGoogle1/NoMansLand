using Unity.VisualScripting;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public TimeOfDay timeOfDay;
    public float defaultFogDensity;
    public float mistFogDensity;
    public float greenRainFogDensity;
    public float dustStormFogDensity;
    public float blackFogDensity;
    public Gradient defaultFogColor;
    public Gradient mistFogColor;
    public Gradient greenRainFogColor;
    public Gradient dustStormFogColor;
    public Gradient blackFogColor;
    public float transitionTime;
    float oldDensity;
    Color oldColor;
    float currentTimeOfDay;

    void Start()
    {
        RenderSettings.fogDensity = defaultFogDensity;
    }

    void Update()
    {
        currentTimeOfDay = timeOfDay.GetTimeOfDay();
        //RenderSettings.fogColor = newGradient.Evaluate(currentTimeOfDay);
    }

    public void SetFogColor(Gradient color)
    {
        oldColor = RenderSettings.fogColor;
        RenderSettings.fogColor = Color.Lerp(oldColor, color.Evaluate(currentTimeOfDay), Time.deltaTime / transitionTime);
    }

    public void SetFogDensity(float density)
    {
        oldDensity = RenderSettings.fogDensity;
        RenderSettings.fogDensity = Mathf.Lerp(oldDensity, density, Time.deltaTime / transitionTime);
    }
}
