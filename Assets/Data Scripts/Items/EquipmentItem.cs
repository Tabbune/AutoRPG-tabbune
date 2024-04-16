using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[CreateAssetMenu(menuName = "Item Data Equipment")]
public class EquipmentItem : ScriptableObject, IItems
{
    public string itemID ;
    public string itemOrder;
    public string itemName;
    public bool sellable;
    public bool isEquipment;
    public int price;
    public Sprite icon ;
    public string itemDesc;
    public int healthBoost ;
    public int attackBoost ;
    public int defenseBoost ;
    public int speedBoost ;
    public int slot;
    public string itemSpecialText;
    public List<BonusStatusEffects> bonusEffects;

    string IItems.itemID { get => itemID; set => this.itemID = value; }
    string IItems.itemOrder { get => itemOrder; set => this.itemOrder = value; }
    string IItems.itemName { get => itemName; set => this.itemName = value; }
    string IItems.itemDesc { get => itemDesc; set => this.itemDesc = value; }
    bool IItems.isEquipment { get => isEquipment; set => this.isEquipment = value; }
    bool IItems.sellable { get => sellable; set => this.sellable = value; }
    int IItems.price { get => price; set => this.price = value; }
    Sprite IItems.icon { get => icon; set => this.icon = value; }

}


[System.Serializable]
public class BonusStatusEffects
{
    public string statusID;
    public int argument;
    public int duration;
}
