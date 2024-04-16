using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;

public class InventoryEquipment : MonoBehaviour
{
    public InventoryDataEquipment itemData;
    public EquipmentItem item;
    public string itemName;
    public string itemDesc;
    public int quantity;
    public Sprite icon;

    public MenuScreen menuScreen;

    // Start is called before the first frame update

    void Awake()
    {
        InventoryDataEquipment dummy = new InventoryDataEquipment();
        dummy.ItemID = "IronSword";
        dummy.Count = 0;
        LoadItemData(dummy);
        menuScreen = GameObject.Find("MenuScreen").GetComponent<MenuScreen>();
    }

    public void LoadItemData(InventoryDataEquipment itemData)
    {
        this.itemData = itemData;
        this.item = equipmentItemMap[itemData.ItemID];
        this.itemName = item.itemName;
        this.itemDesc = item.itemDesc;
        this.quantity = itemData.Count;
        this.icon = item.icon;

        this.transform.GetChild(0).GetComponent<Image>().sprite = this.icon;
        this.transform.GetChild(1).GetComponent<TMP_Text>().SetText(quantity.ToString());
    }
    public void changePreviewEquipment()
    {
        menuScreen.changeItemPreviewEquipment(this);
        //Debug.Log(ItemDetailView.ToString(), ItemDetailView);
    }

}
