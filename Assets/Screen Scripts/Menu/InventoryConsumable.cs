using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;

public class InventoryConsumable : MonoBehaviour
{
    public InventoryDataConsumable itemData;
    public ConsumableItem item;
    public string itemName;
    public string itemDesc;
    public int quantity;
    public Sprite icon;

    public MenuScreen menuScreen;

    // Start is called before the first frame update

    void Awake()
    {
        InventoryDataConsumable dummy = new InventoryDataConsumable();
        dummy.ItemID = "BronzeIngot";
        dummy.Count = 0;
        LoadItemData(dummy);
        menuScreen = GameObject.Find("MenuScreen").GetComponent<MenuScreen>();
    }

    public void LoadItemData(InventoryDataConsumable itemData)
    {
        this.itemData = itemData;
        this.item = consumableItemMap[itemData.ItemID];
        this.itemName = item.itemName;
        this.itemDesc = item.itemDesc;
        this.quantity = itemData.Count;
        this.icon = item.icon;

        this.transform.GetChild(0).GetComponent<Image>().sprite = this.icon;
        this.transform.GetChild(1).GetComponent<TMP_Text>().SetText(quantity.ToString());
    }

    public void changePreviewConsumable()
    {
        menuScreen.changeItemPreviewConsumable(this);
        //Debug.Log(ItemDetailView.ToString(), ItemDetailView);
    }
}
