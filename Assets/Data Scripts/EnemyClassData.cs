using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Class Data")]
public class EnemyClassData : ScriptableObject, IAddressable
{
    public string className;
    public string enemyID;
    public bool isEnemy;
    public Sprite icon;
    public int level;
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
    public int baseCritRate;
    public int baseCritDamage;
    public List<EnemyClassTactic> moveset;

    string IAddressable.className { get => className; set => this.className = value; }
    bool IAddressable.isEnemy { get => true; set => this.isEnemy = true; }
}

[Serializable]
public class EnemyClassTactic
{
    public int order;
    public string TacticID;
    public int argument;
    public string AttackID;
}
