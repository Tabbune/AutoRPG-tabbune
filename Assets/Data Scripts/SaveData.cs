using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public List<string> TeamList;
    public List<InventoryDataConsumable> inventoryConsumable;
    public List<InventoryDataEquipment> inventoryEquipment;
    public long Coins;
}

[System.Serializable]
public class InventoryDataConsumable
{
    public string ItemID;
    public string ItemOrder;
    public string type;
    public int Count;
}
[System.Serializable]
public class InventoryDataEquipment
{
    public string ItemID;
    public string ItemOrder;
    public string type;
    public int Count;
    public int Slot;
}