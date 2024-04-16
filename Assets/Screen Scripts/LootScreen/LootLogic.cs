using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TitleScreen;
using System.IO;
using UnityEditor;
using CsvHelper;
using System.Globalization;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using System.Linq;
using UnityEngine.Profiling;
using CsvHelper.Configuration;
using System.Text;
using UnityEngine.SceneManagement;

public class LootLogic : MonoBehaviour
{
    private int rollAttempts;
    public GameObject lootObtainedList;
    public LootEntry lootEntryPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        rollAttempts = 20;

        foreach(string enemyID in enemyList)
        {
            ProcessEnemyLootTable(enemyID);
        }

        //var csv = new CsvReader(new StreamReader("Actors.csv"));

    }

    void ProcessEnemyLootTable(string enemyID)
    {
        List<LootTableEntry> lootTable = LoadLootTable(enemyID);

        List<IndividualEntries> lootIndividualEntries = new List<IndividualEntries>();
        int lootLeftInTable = 0;
        foreach (LootTableEntry loot in lootTable)
        {
            lootLeftInTable += loot.Left;
            for (int i = 0; i < loot.Left; i++)
            {
                IndividualEntries individualEntry = new IndividualEntries(loot.ItemID, loot.Type);
                lootIndividualEntries.Add(individualEntry);
            }
        }

        List<IndividualEntries> lootGained = new List<IndividualEntries>();
        for (int i = 0; i < lootLeftInTable; i++)
        {
            float lootRoll = Random.Range(0f, 1f);
            if (lootRoll < ((float)rollAttempts / (float)(lootLeftInTable - i)))
            {
                IndividualEntries newLootName = lootIndividualEntries[i];
                lootGained.Add(newLootName);

                LootEntry newLoot = Instantiate(lootEntryPrefab, lootObtainedList.transform);
                if (newLootName.Type == "Consumable")
                {
                    AddToInventoryConsumable(newLootName.ItemID);
                    newLoot.LoadItemData(consumableItemMap[newLootName.ItemID]);
                }
                else if (newLootName.Type == "Equipment")
                {
                    AddToInventoryEquipment(newLootName.ItemID);
                    newLoot.LoadItemData(equipmentItemMap[newLootName.ItemID]);
                }


                lootTable.Find(x => x.ItemID.Equals(lootIndividualEntries[i].ItemID)).Left -= 1;
                rollAttempts -= 1;
            }
        }

        saveData.inventoryConsumable.Sort((x, y) => x.ItemOrder.CompareTo(y.ItemOrder));
        saveData.inventoryEquipment.Sort((x, y) => x.ItemOrder.CompareTo(y.ItemOrder));
        SaveCurrentData();

        Debug.Log("Loot obtained: " + lootGained.Count.ToString());
        UpdateLootTable(enemyID, lootTable);
    }

    public void AddToInventoryConsumable(string itemID)
    {
        if(saveData.inventoryConsumable == null) { saveData.inventoryConsumable = new List<InventoryDataConsumable>(); }
        InventoryDataConsumable inventoryData = saveData.inventoryConsumable.Find(x => x.ItemID.Equals(itemID));
        if (inventoryData != null)
        {
            inventoryData.Count += 1;
        }
        else
        {
            InventoryDataConsumable newItem = new InventoryDataConsumable();
            newItem.ItemID = itemID;
            newItem.ItemOrder = consumableItemMap[itemID].itemOrder;
            newItem.Count = 1;
            saveData.inventoryConsumable.Add(newItem);
        }
    }
    public void AddToInventoryEquipment(string itemID)
    {
        if (saveData.inventoryEquipment == null) { saveData.inventoryEquipment = new List<InventoryDataEquipment>(); }
        InventoryDataEquipment inventoryData = saveData.inventoryEquipment.Find(x => x.ItemID.Equals(itemID));
        if (inventoryData != null)
        {
            inventoryData.Count += 1;
        }
        else
        {
            InventoryDataEquipment newItem = new InventoryDataEquipment();
            newItem.ItemID = itemID;
            newItem.ItemOrder = equipmentItemMap[itemID].itemOrder;
            newItem.Count = 1;
            newItem.Slot = equipmentItemMap[itemID].slot;
            saveData.inventoryEquipment.Add(newItem);
        }
    }

    [ContextMenu("LoadLootTable")]
    List<LootTableEntry> LoadLootTable(string enemyID)
    {
        List<LootTableEntry> records;
        string file_path = Application.persistentDataPath + "/LootTables/Loot_" + enemyID;
        Debug.Log("Loot table: " + file_path + ", rolling " + rollAttempts.ToString() + " times");

        using (var reader = new StreamReader(file_path))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 }))
        {
            records = csv.GetRecords<LootTableEntry>().ToList();
        }
        foreach (LootTableEntry record in records)
        {
            Debug.Log("ItemID: " + record.ItemID + "(" + record.Type +") - " + record.Left.ToString() + "/" + record.Total.ToString());
        }
        return records;
    }

    void UpdateLootTable(string enemyID, List<LootTableEntry> lootTable)
    {
        string file_path = Application.persistentDataPath + "/LootTables/Loot_" + enemyID + ".csv";
        File.Delete(file_path);
        using (var writer = new StreamWriter(file_path))
        {
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 }))
            {
                csv.WriteRecords(lootTable);
            }
        }
    }

    public void MoveToScene()
    {
        SceneManager.LoadScene("Menu");
    }
}

public class LootTableEntry
{
    public string ItemID { get; set; }
    public string Type { get; set; }
    public int Left { get; set; }
    public int Total { get; set; }
}

public class IndividualEntries
{
    public string ItemID { get; set; }
    public string Type { get; set; }
    public IndividualEntries(string ItemID, string Type)
    {
        this.ItemID = ItemID;
        this.Type = Type;
    }
}