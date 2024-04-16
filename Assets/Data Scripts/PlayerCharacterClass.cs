using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerCharacterClass : ScriptableObject, IAddressable
{
    public string className;
    public bool isEnemy;
    public Sprite icon;
    public int baseHP;
    public int HPGrowth;
    public int baseAttack;
    public float attackGrowth;
    public int baseDefense;
    public float defenseGrowth;
    public int baseSpeed;
    public float speedGrowth;
    public int baseMaxMP;
    public int baseStartingMP;
    public int baseMPRegen;
    public int baseCritRate;
    public int baseCritDamage;
    public int passiveAggroGain;
    public List<Learnset> learnset;
    string IAddressable.className { get => className; set => this.className = value; }
    bool IAddressable.isEnemy { get => false; set => this.isEnemy = false; }
}

[Serializable]
public class Learnset
{
    public string attackID;
    public int learnLevel;
}
