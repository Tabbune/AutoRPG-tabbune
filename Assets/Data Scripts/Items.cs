using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IItems;

public interface IItems
{
    string itemID { get; set; }
    string itemOrder { get; set; }
    string itemName { get; set; }
    string itemDesc { get; set; }
    bool isEquipment { get; set; }
    bool sellable { get; set; }
    int price { get; set; }
    Sprite icon { get; set; }

}
