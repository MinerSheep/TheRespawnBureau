using System;

[Serializable]
public class GlobalData
{
    public int unlockedLevel;
    public float volume;

    public GlobalData(int unlockedLevel, float volume)
    {
        this.unlockedLevel = unlockedLevel;
        this.volume = volume;
    }
}
