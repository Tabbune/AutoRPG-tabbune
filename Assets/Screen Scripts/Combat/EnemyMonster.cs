using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TitleScreen;

public class EnemyMonster : EntityBase
{
    public EnemyClassData enemyClassData;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Creating enemy: " + enemyList[this.transform.GetSiblingIndex()]);
        this.enemyClassData = enemyDataDict[enemyList[this.transform.GetSiblingIndex()]];
        SetEntityName(enemyClassData.className);
        this.GetComponent<SpriteRenderer>().sprite = enemyClassData.icon;
        int thisUnitLevel = enemyClassData.level;
        this.LoadLevel(thisUnitLevel);
        //this.loadEXP(PC_Data.experience);

        this.SetMaxHP(Mathf.FloorToInt(enemyClassData.baseHP * (1 + (((float)enemyClassData.HPGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetAttack(Mathf.FloorToInt(enemyClassData.baseAttack * (1 + (((float)enemyClassData.attackGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetDefense(Mathf.FloorToInt(enemyClassData.baseDefense * (1 + (((float)enemyClassData.defenseGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetSpeed(Mathf.FloorToInt(enemyClassData.baseSpeed * (1 + (((float)enemyClassData.speedGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetMaxMP(enemyClassData.baseMaxMP);
        this.setCurrentMP(enemyClassData.baseStartingMP);
        this.setCritRate(enemyClassData.baseCritRate);
        this.setCritDamage(enemyClassData.baseCritDamage);

        StartBase();

        foreach (EnemyClassTactic tactic in enemyClassData.moveset)
        {
            tactics.Add(TacticFactory.GetTactic(tactic.TacticID, this, attackMap[tactic.AttackID], tactic.argument));
        }
        isEnemy = true;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
