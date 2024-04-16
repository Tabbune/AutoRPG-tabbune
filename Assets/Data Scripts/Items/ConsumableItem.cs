using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data Consumable")]
public class ConsumableItem : ScriptableObject, IItems
{
    public string itemID ;
    public string itemOrder;
    public string itemName;
    public string itemDesc;
    public bool isEquipment;
    public bool sellable;
    public int price;
    public Sprite icon ;


    string IItems.itemID { get => itemID; set => this.itemID = value; }
    string IItems.itemOrder { get => itemOrder; set => this.itemOrder = value; }
    string IItems.itemName { get => itemName; set => this.itemName = value; }
    string IItems.itemDesc { get => itemDesc; set => this.itemDesc = value; }
    bool IItems.isEquipment { get => isEquipment; set => this.isEquipment = value; }
    bool IItems.sellable { get => sellable; set => this.sellable = value; }
    int IItems.price { get => price; set => this.price = value; }
    Sprite IItems.icon { get => icon; set => this.icon = value; }

}