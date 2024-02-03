using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class EnemyBase : ScriptableObject
{
    public string charName;
    public Sprite img;
    public int rank;
    public string description;
    public List<Classes> traits = new List<Classes>();

    //details
    public int currentHealth;
    public int currentMana;
    public int currentLevel;

    public float upgradeIncrement = 1.5f;

    public Upgrades upgrade = new Upgrades();
}

public enum Classes
{
    Country,
    Disco,
    EDM
}

[System.Serializable]
public class Upgrades
{
    // only hp and ad is scalable after upgrade 
    // index 0 and 2 g
    public float[] basicStats = new float[10];
    // HP MANA 
    // AD AP
    // AR MR
    // CC CD
    // AS Range
}

[System.Serializable]
public class Equipment
{
    public int[] statIndex;
    public int[] statsAmount;
}