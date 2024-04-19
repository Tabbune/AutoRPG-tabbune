using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITactic;
using static TitleScreen;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine.UI;
using System.Xml.Linq;

public class PlayerCharacterEditorData : MonoBehaviour
{
    public PlayerCharacterClass classData;
    public string className;
    private Sprite icon;
    private int Level;
    private int HP;
    private int Attack;
    private int Defense;
    private int Speed;
    private int MaxMP;
    private int StartingMP;
    private int CritRate;
    private int CritDamage;
    private List<PlayerDataTactic> tactics;
    private List<AttackPool> attackPool;
    private string[] equipments;

    public class AttackPool
    {
        public string attackName;
        public int learnLevel;

        public AttackPool(string attackName, int learnLevel)
        {
            this.attackName = attackName;
            this.learnLevel = learnLevel;
        }
    }

    public int getLevelUpCost()
    {
        return 1000 + (Level * 50) + (Level * Level * 50);
    }


    public int getLevel()
    {
        return this.Level;
    }
    public string getClassName()
    {
        return this.className;
    }
    public string getEquipmentAtSlot(int index)
    {
        return this.equipments[index];
    }

    private void Awake()
    {
        LoadCharacter("Warrior");
    }

    public void LoadCharacter(string className)
    {
        this.classData = classDataDict[className];
        //Debug.Log("Classname: " + className + ", classData.className: " + classData.className);
        //className = classData.className;
        this.className = classData.className;

        icon = classData.icon;
        transform.gameObject.GetComponent<Image>().sprite = icon;

        PlayerCharacterData PC_Data;
        PC_Data = ExportData(className);
        Level = PC_Data.level;


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

        HP = Mathf.FloorToInt(finalBaseHP * (1 + (((float)classData.HPGrowth/100f) * (Level - 1))));
        Attack = Mathf.FloorToInt(finalBaseAtt * (1 + (((float)classData.attackGrowth / 100f) * (Level - 1))));
        Defense = Mathf.FloorToInt(finalBaseDef * (1 + (((float)classData.defenseGrowth / 100f) * (Level - 1))));
        Speed = Mathf.FloorToInt(finalBaseSpe * (1 + (((float)classData.speedGrowth / 100f) * (Level - 1))));
        StartingMP = classData.baseStartingMP;
        MaxMP = classData.baseMaxMP;
        CritRate = classData.baseCritRate;
        CritDamage = classData.baseCritDamage;
        tactics = new List<PlayerDataTactic>();
        attackPool = new List<AttackPool>();

        foreach (PlayerDataTactic tactic in PC_Data.tacticList)
        {
            tactics.Add(tactic);
        }

        foreach (var item in classData.learnset)
        {
            this.attackPool.Add(new AttackPool(item.attackID, item.learnLevel));
        }
        equipments = PC_Data.equipments;
    }

    public static PlayerCharacterData ExportData(string className)
    {
        PlayerCharacterData PC_Data = new PlayerCharacterData();
        string class_Data_Loc = Application.persistentDataPath + "/playerdata_" + className + ".json";
        if (File.Exists(class_Data_Loc))
        {
            string loaded_data = File.ReadAllText(class_Data_Loc);
            PC_Data = JsonUtility.FromJson<PlayerCharacterData>(loaded_data);
        }
        else
        {
            PC_Data.baseCharacter = className;
            PC_Data.level = 1;
            PC_Data.experience = 0;
            PC_Data.tacticList = new List<PlayerDataTactic>();

            PlayerDataTactic tactic = new PlayerDataTactic();
            tactic.order = 1;
            tactic.TacticID = "Default";
            tactic.argument = 0;
            tactic.AttackID = "BasicAttack";

            PC_Data.tacticList.Add(tactic);

            PC_Data.equipments = new string[6];
            for(int i = 0; i < 6; i++)
            {
                PC_Data.equipments[i] = "null";
            }

            string file_path = Application.persistentDataPath + "/playerdata_" + className + ".json";
            string json = JsonUtility.ToJson(PC_Data);
            File.WriteAllText(file_path, json);
        }
        return PC_Data;
    }

    public string printProfile()
    {
        string profile = "";
        profile += getClassName() + "\r\n";
        profile += "Level: " + getLevel().ToString() + "\r\n";

        profile += "HP: " + HP.ToString() + "\r\n";
        profile += "Attack: " + Attack.ToString() + "\r\n";
        profile += "Defense: " + Defense.ToString() + "\r\n";
        profile += "Speed: " + Speed.ToString() + "\r\n";
        profile += "MP: " + StartingMP.ToString() + "/" + MaxMP.ToString() + "\r\n";
        profile += "Crit: " + CritRate.ToString() + "/" + CritDamage.ToString() + "\r\n";


        return profile;
    }

    public void setThisAsActiveUnit()
    {
        MenuScreen menuScreen = FindFirstObjectByType<MenuScreen>();
        menuScreen.SetActiveUnit(transform.GetSiblingIndex());
    }

    public List<PlayerDataTactic> getTactics()
    {
        return tactics;
    }
    public List<AttackPool> getAttackPool()
    {
        return attackPool;
    }

    public void ChangeEquipment(int slot, string equip)
    {
        this.equipments[slot] = equip;
    }
}
