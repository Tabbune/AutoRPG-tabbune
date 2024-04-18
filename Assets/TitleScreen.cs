using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static IAttack;
using static IItems;
using static ITactic;

public class TitleScreen : MonoBehaviour
{
    public static Dictionary<string, IAttack> attackMap;
    public static Dictionary<string, int> tacticMap;
    public static Dictionary<int, string> tacticMap2;
    public static List<string> teamList;
    public static Dictionary<string, PlayerCharacterClass> classDataDict;
    public static List<string> enemyList;
    public static Dictionary<string, EnemyClassData> enemyDataDict;
    public static Dictionary<string, ConsumableItem> consumableItemMap;
    public static Dictionary<string, EquipmentItem> equipmentItemMap;
    public static SaveData saveData;
    public static bool didPartyWin;

    // Start is called before the first frame update
    void Start()
    {
        saveData = LoadData();
        attackMap = new Dictionary<string, IAttack>();

        PopulateAttackList();
        //attackMap.Add("BasicAttack", new AttackBasicAttack());
        //attackMap.Add("HealingChant", new AttackHealingChant());
        //attackMap.Add("AssassinStrike", new AttackAssassinStrike());
        //attackMap.Add("TangleStrike", new AttackTangleStrike());
        //attackMap.Add("PoisonStrike", new AttackPoisonStrike());
        //attackMap.Add("SwordsDance", new AttackSwordsDance());
        //attackMap.Add("BlessingOfZeal", new AttackBlessingOfZeal());


        tacticMap = new Dictionary<string, int>();
        tacticMap.Add("Default", 0);
        tacticMap.Add("EnemyHPGreaterThan", 1);
        tacticMap.Add("EnemyHPLowerThan", 2);
        tacticMap.Add("AllyHPLowerThan", 3);

        tacticMap2 = new Dictionary<int, string>();
        tacticMap2.Add(0, "Default");
        tacticMap2.Add(1, "EnemyHPGreaterThan");
        tacticMap2.Add(2, "EnemyHPLowerThan");
        tacticMap2.Add(3, "AllyHPLowerThan");

        teamList = new List<string>();
        teamList = saveData.TeamList;

        //teamList.Add("Warrior");
        //teamList.Add("Mage");
        //teamList.Add("Paladin");
        //teamList.Add("Ranger");

        List<PlayerCharacterClass> tempClassData = PopulateClassList();
        classDataDict = new Dictionary<string, PlayerCharacterClass>();
        foreach (PlayerCharacterClass classLoad in tempClassData)
        {
            classDataDict.Add(classLoad.className, classLoad);
            //Debug.Log("Class " + classLoad.className + " has been loaded");
        }

        enemyList = new List<string>();
        List<EnemyClassData> tempEnemyData = PopulateEnemyList();

        enemyDataDict = new Dictionary<string, EnemyClassData>();
        foreach (EnemyClassData enemyLoad in tempEnemyData)
        {
            //Debug.Log("Adding to enemyList: " + enemyLoad.enemyID);
            if(enemyList.Count == 0) { enemyList.Add(enemyLoad.enemyID); }

            enemyDataDict.Add(enemyLoad.enemyID, enemyLoad);
            //Debug.Log("Enemy " + enemyLoad.className + " has been loaded");
        }

        consumableItemMap = new Dictionary<string, ConsumableItem>();
        equipmentItemMap = new Dictionary<string, EquipmentItem>();
        PopulateItemMap();

        CheckLootTables();

        didPartyWin = false;

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void moveToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void PopulateAttackList()
    {
        List<IAttack> tempLoadData = new List<IAttack>();
        IAttack[] assetNames = Resources.LoadAll<AttackBasicAttack>("Attacks");
        foreach (IAttack asset in assetNames)
        {
            attackMap.Add(asset.attackID, asset);
        }
    }

    public List<PlayerCharacterClass> PopulateClassList()
    {
        List<PlayerCharacterClass> tempLoadData = new List<PlayerCharacterClass>();
        PlayerCharacterClass[] assetNames = Resources.LoadAll<PlayerCharacterClass>("Class Data");
        foreach (PlayerCharacterClass asset in assetNames)
        {
            //var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            //PlayerCharacterClass character = AssetDatabase.LoadAssetAtPath<PlayerCharacterClass>(SOpath);
            //if(character == null) { continue; }
            //Debug.Log("Character Found: " + character.className);

            tempLoadData.Add(asset);
        }
        return tempLoadData;
    }
    public List<EnemyClassData> PopulateEnemyList()
    {
        List<EnemyClassData> tempLoadData = new List<EnemyClassData>();
        EnemyClassData[] assetNames = Resources.LoadAll<EnemyClassData>("Enemy Class Data");
        foreach (EnemyClassData asset in assetNames)
        {
            //var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            //EnemyClassData enemy = AssetDatabase.LoadAssetAtPath<EnemyClassData>(SOpath);
            //if (enemy == null) { continue; }
            tempLoadData.Add(asset);
        }
        return tempLoadData;
    }

    public void PopulateItemMap()
    {
        ConsumableItem[] assetNames = Resources.LoadAll<ConsumableItem>("Items/Consumables");
        foreach (ConsumableItem asset in assetNames)
        {
            consumableItemMap.Add(asset.itemID, asset);
        }

        EquipmentItem[] assetNamesE = Resources.LoadAll<EquipmentItem>("Items/Equipments");
        foreach (EquipmentItem asset in assetNamesE)
        {
            equipmentItemMap.Add(asset.itemID, asset);
        }
    }

    public SaveData LoadData()
    {
        SaveData save;

        string file_path = Application.persistentDataPath + "/playerdata.json";

        if (!File.Exists(file_path))
        {
            save = new();
            save.TeamList = new List<string>();
            save.TeamList.Add("Warrior");
            save.TeamList.Add("Mage");
            save.TeamList.Add("Paladin");
            save.TeamList.Add("Ranger");

            save.Coins = 0;

            save.inventoryConsumable = new List<InventoryDataConsumable>();
            save.inventoryEquipment = new List<InventoryDataEquipment>();

            string json = JsonUtility.ToJson(save);
            File.WriteAllText(file_path, json);
        }

        string loaded_data = File.ReadAllText(file_path);
        save = JsonUtility.FromJson<SaveData>(loaded_data);
        save.inventoryConsumable.Sort((x, y) => x.ItemOrder.CompareTo(y.ItemOrder));

        return save;
    }

    public static void SaveCurrentData()
    {
        string file_path = Application.persistentDataPath + "/playerdata.json";
        string json = JsonUtility.ToJson(saveData);
        Debug.Log(file_path + " " + json);
        File.WriteAllText(file_path, json);
    }

    public void CheckLootTables()
    {
        TextAsset[] assetNames = Resources.LoadAll<TextAsset>("Loot Tables");
        foreach (TextAsset lootTable in assetNames)
        {
            string file_path = Application.persistentDataPath + "/LootTables/" + lootTable.name;
            if (!File.Exists(file_path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file_path));
                File.WriteAllText(file_path, lootTable.text);
            }
            Debug.Log("lootTableData: " + lootTable.text + " file_path: " + file_path);
        }
    }

    [ContextMenu("ClearCurrentSaveData")]
    public void ClearCurrentSaveData()
    {
        string file_path = Application.persistentDataPath + "/playerdata.json";
        if (File.Exists(file_path))
        {
            File.Delete(file_path);
        }

        file_path = Application.persistentDataPath + "/LootTables/";
        string[] lootTableFiles = Directory.GetFiles(file_path);
        foreach(string lootTableFile in lootTableFiles)
        {
            if (File.Exists(lootTableFile))
            {
                File.Delete(lootTableFile);
            }
        }

    }
}
