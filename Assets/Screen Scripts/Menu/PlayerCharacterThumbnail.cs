using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITactic;
using static TitleScreen;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine.UI;

public class PlayerCharacterThumbnail : MonoBehaviour
{
    public PlayerCharacterClass classData;
    private string className;
    private Sprite icon;
    private int Level;
    private int XP;

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

    public void gainXP(int xp)
    {
        this.XP += xp;
        checkLevelUp();
    }
    public void checkLevelUp()
    {
        while(this.XP >= 100 + (Level * 5) + (Level * Level * 5))
        {
            this.XP -= 100 + (Level * 5) + (Level * Level * 5);
            this.Level += 1;
        }
    }
    public int getLevel()
    {
        return this.Level;
    }
    public string getClassName()
    {
        return this.className;
    }

    private void Awake()
    {
        LoadCharacter("Warrior");
    }

    public void LoadCharacter(string className)
    {
        this.classData = classDataDict[className];
        if(this.classData == null)
        {
            return;
        }
        this.className = classData.className;

        icon = classData.icon;
        transform.gameObject.GetComponent<Image>().sprite = icon;

        PlayerCharacterData PC_Data;
        string class_Data_Loc = Application.persistentDataPath + "/playerdata_" + className + ".json";
        if (File.Exists(class_Data_Loc))
        {
            string loaded_data = File.ReadAllText(class_Data_Loc);
            PC_Data = JsonUtility.FromJson<PlayerCharacterData>(loaded_data);
        }
        else
        {
            PC_Data = new PlayerCharacterData();
            PC_Data.baseCharacter = className;
            PC_Data.level = 1;
            PC_Data.experience = 0;
        }
        Level = PC_Data.level;
        XP = PC_Data.experience;
    }

    public void setThisAsActiveUnit()
    {
        MenuScreen menuScreen = FindFirstObjectByType<MenuScreen>();
        menuScreen.swapUnits(this.transform.GetSiblingIndex());
    }

}
