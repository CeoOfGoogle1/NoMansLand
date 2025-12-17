public enum AmmoType
{
    Intermediate,
    FullPower,
    PropelledGrenade,
    Rocket
}

[System.Serializable]
public class Magazine
{
    public AmmoType ammoType;
    public int capacity;
    public int currentAmmo;
    public float weight;
}

