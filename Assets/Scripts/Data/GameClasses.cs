using UnityEngine;

[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public Sprite icon;
    public UpgradeType upgradeType;
    public float value;
    public int maxLevel;
}

[System.Serializable]
public class WaveConfig
{
    public float startTime;
    public float endTime;
    public EnemySpawnData[] enemies;
}
