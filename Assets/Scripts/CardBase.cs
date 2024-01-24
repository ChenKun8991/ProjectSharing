using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class EnemyBase : ScriptableObject
{
    public Sprite img;
    public int rank;
    public string description;
    public List<Classes> traits = new List<Classes>();

    //details
    public int currentHealth;
    public int currentMana;
    public int currentLevel;

    public Upgrades[] upgrades = new Upgrades[3];
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
    public string[] basicStats = new string[10];
}

[System.Serializable]
public class Equipment
{
    public int[] statIndex;
    public int[] statsAmount;
}