using System;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public int level;
    public int stat;
    public int cost;

    public UpgradeData(int level, int stat, int cost)
    {
        this.level = level;
        this.stat = stat;
        this.cost = cost;
    }
}

public enum UpgradeType
{
    MaxStamina,
    Health,
    StaminaDrain,
    ScoreOrbValue,
    CoinValue,
    SmallMagnet
}