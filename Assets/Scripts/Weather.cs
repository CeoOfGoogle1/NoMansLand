using UnityEngine;

public class Weather : MonoBehaviour
{
    public FogManager fogManager;
    public TimeOfDay timeOfDay;
    public GameObject player;
    public GameObject rain;
    public GameObject snow;
    public GameObject greenRain;
    public GameObject dustStorm;
    public GameObject embers;
    public float teleportDistance;

    public enum CurrentWeather
    {
        Clear,
        Misty,
        Rain,
        Snow,
        GreenRain,
        DustStorm,
        BlackFog
    }

    public CurrentWeather currentWeather = CurrentWeather.Clear;
    private CurrentWeather oldWeather = CurrentWeather.Clear;

    private AudioManager.LoopHandle rainLoop;
    private AudioManager.LoopHandle snowLoop;
    private AudioManager.LoopHandle dustStormLoop;
    private AudioManager.LoopHandle blackFogLoop;

    void Update()
    {
        Clear();
        Mist();
        Rain();
        Snow();
        GreenRain();
        DustStorm();
        BlackFog();
        PlayLoops();

        if (Vector3.Distance(player.transform.position, transform.position) > teleportDistance)
        transform.position = player.transform.position + new Vector3(0, 10, 0);
    }

    void Clear()
    {
        if (currentWeather == CurrentWeather.Clear)
        {
            fogManager.SetFogColor(fogManager.defaultFogColor);
            fogManager.SetFogDensity(fogManager.defaultFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topClear, timeOfDay.midClear, timeOfDay.bottomClear);
        }
    }

    void Mist()
    {
        if (currentWeather == CurrentWeather.Misty) 
        {
            fogManager.SetFogColor(fogManager.mistFogColor);
            fogManager.SetFogDensity(fogManager.mistFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topMist, timeOfDay.midMist, timeOfDay.bottomMist);
        }
    }

    void Rain()
    {
        if (currentWeather == CurrentWeather.Rain) 
        {
            rain.SetActive(true);
            fogManager.SetFogColor(fogManager.mistFogColor);
            fogManager.SetFogDensity(fogManager.mistFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topMist, timeOfDay.midMist, timeOfDay.bottomMist);
        }
        else
        {
            rain.SetActive(false);
        }
    }

    void Snow()
    {
        if (currentWeather == CurrentWeather.Snow) 
        {
            snow.SetActive(true);
            fogManager.SetFogColor(fogManager.mistFogColor);
            fogManager.SetFogDensity(fogManager.mistFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topMist, timeOfDay.midMist, timeOfDay.bottomMist);
        }
        else
        {
            snow.SetActive(false);
        }
    }

    void GreenRain()
    {
        if (currentWeather == CurrentWeather.GreenRain) 
        {
            greenRain.SetActive(true);
            fogManager.SetFogColor(fogManager.greenRainFogColor);
            fogManager.SetFogDensity(fogManager.greenRainFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topGreen, timeOfDay.midGreen, timeOfDay.bottomGreen);
        }
        else
        {
            greenRain.SetActive(false);
        }
    }

    void DustStorm()
    {
        if (currentWeather == CurrentWeather.DustStorm) 
        {
            dustStorm.SetActive(true);
            fogManager.SetFogColor(fogManager.dustStormFogColor);
            fogManager.SetFogDensity(fogManager.dustStormFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topDust, timeOfDay.midDust, timeOfDay.bottomDust);
        }
        else 
        {
            dustStorm.SetActive(false);
        }
    }

    void BlackFog()
    {
        if (currentWeather == CurrentWeather.BlackFog) 
        {
            embers.SetActive(true);
            fogManager.SetFogColor(fogManager.blackFogColor);
            fogManager.SetFogDensity(fogManager.blackFogDensity);
            timeOfDay.SetSkyColor(timeOfDay.topBlack, timeOfDay.midBlack, timeOfDay.bottomBlack);
        }
        else
        {
            embers.SetActive(false);
        }
    }

    void PlayLoops()
    {
        if (oldWeather != currentWeather)
        {
            oldWeather = currentWeather;

            rainLoop.Stop();
            snowLoop.Stop();
            dustStormLoop.Stop();
            blackFogLoop.Stop();

            if (currentWeather == CurrentWeather.Rain || currentWeather == CurrentWeather.GreenRain)
            {
                rainLoop = AudioManager.instance.PlayLoop("rain", transform);
            }
            if (currentWeather == CurrentWeather.Snow)
            {
                snowLoop = AudioManager.instance.PlayLoop("snow", transform);
            }
            if (currentWeather == CurrentWeather.DustStorm)
            {
                dustStormLoop = AudioManager.instance.PlayLoop("duststorm", transform);
            }
            if (currentWeather == CurrentWeather.BlackFog)
            {
                blackFogLoop = AudioManager.instance.PlayLoop("blackfog", transform);
            }
        }
    }
}
