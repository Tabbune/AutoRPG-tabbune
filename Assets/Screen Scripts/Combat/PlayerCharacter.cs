using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static TitleScreen;

public class Player : EntityBase
{
    // Start is called before the first frame update

    public PlayerCharacterClass classData;
    public List<BonusStatusEffects> bonusEffects;

    void Awake()
    {
        //if(this.transform.GetSiblingIndex() > teamList.Count)
        //{
        //    Destroy(this);
        //}
        this.classData = classDataDict[teamList[this.transform.GetSiblingIndex()]];
        SetEntityName(classData.className);

        Sprite icon = classData.icon;
        this.GetComponent<SpriteRenderer>().sprite = icon;

        PlayerCharacterData PC_Data;
        string class_Data_Loc = Application.persistentDataPath + "/playerdata_" + this.getEntityName() + ".json";
        if (File.Exists(class_Data_Loc))
        {
            string loaded_data = File.ReadAllText(class_Data_Loc);
            PC_Data = JsonUtility.FromJson<PlayerCharacterData>(loaded_data);
        }
        else
        {
            PC_Data = new PlayerCharacterData();
            PC_Data.baseCharacter = this.getEntityName();
            PC_Data.level = 1;
            PC_Data.experience = 0;
            PC_Data.tacticList = new List<PlayerDataTactic>();

            PlayerDataTactic tactic = new PlayerDataTactic();
            tactic.order = 1;
            tactic.TacticID = "Default";
            tactic.argument = 0;
            tactic.AttackID = "BasicAttack";

            PC_Data.tacticList.Add(tactic);
        }

        int thisUnitLevel = PC_Data.level;
        this.LoadLevel(thisUnitLevel);
        //this.loadEXP(PC_Data.experience);

        int finalBaseHP = classData.baseHP;
        int finalBaseAtt = classData.baseAttack;
        int finalBaseDef = classData.baseDefense;
        int finalBaseSpe = classData.baseSpeed;
        for (int i = 0; i < 6; i++)
        {
            if (PC_Data.equipments[i] != "null")
            {
                EquipmentItem equip = equipmentItemMap[PC_Data.equipments[i]];
                finalBaseHP += equip.healthBoost;
                finalBaseAtt += equip.attackBoost;
                finalBaseDef += equip.defenseBoost;
                finalBaseSpe += equip.speedBoost;
            }
        }

        this.SetMaxHP(Mathf.FloorToInt(finalBaseHP * (1 + (((float)classData.HPGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetAttack(Mathf.FloorToInt(finalBaseAtt * (1 + (((float)classData.attackGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetDefense(Mathf.FloorToInt(finalBaseDef * (1 + (((float)classData.defenseGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetSpeed(Mathf.FloorToInt(finalBaseSpe * (1 + (((float)classData.speedGrowth / 100f) * (thisUnitLevel - 1)))));
        this.SetMaxMP(classData.baseMaxMP);
        this.setCurrentMP(classData.baseStartingMP);
        this.setCritRate(classData.baseCritRate);
        this.setCritDamage(classData.baseCritDamage);
        this.setAggroGain(classData.passiveAggroGain);

        StartBase();

        for (int i = 0; i < 6; i++)
        {
            if (PC_Data.equipments[i] != "null")
            {
                EquipmentItem equip = equipmentItemMap[PC_Data.equipments[i]];
                foreach (BonusStatusEffects bonusStatus in equip.bonusEffects)
                {
                    this.bonusEffects.Add(bonusStatus);
                }
            }
        }

        foreach (Learnset item in classData.learnset)
        {
            IAttack attack = attackMap[item.attackID].GetNewInstance();
            this.attackList.Add(attack);
        }

        foreach (PlayerDataTactic tactic in PC_Data.tacticList)
        {
            IAttack attack = attackList.Find(x => x.attackID.Equals(tactic.AttackID));
            tactics.Add(TacticFactory.GetTactic(tactic.TacticID, this, attack, tactic.argument));
        }

        //tactics.Add(new ITactic.TacticIsAllyLowHP(this, attackMap["HealingChant"], 99));
        //tactics.Add(new ITactic.TacticDefault(this, attackMap["BasicAttack"]));
        isEnemy = false;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

}
