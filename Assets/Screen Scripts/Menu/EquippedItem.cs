using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;

public class EquippedItem : MonoBehaviour
{
    // Start is called before the first frame update
    private EquipmentItem equippedItem;
    public int expandedHeight;
    public int shrinkedHeight;
    public GameObject descriptionPanel;
    private bool isExpand;
    private int slot;
    private MenuScreen menuScreen;

    public GameObject HPBonus;
    public GameObject AttackBonus;
    public GameObject DefenseBonus;
    public GameObject SpeedBonus;
    public GameObject Image;

    public TMP_Dropdown equipmentOptions;
    public List<string> equipmentIDKey;

    void Awake()
    {
        menuScreen = GameObject.Find("MenuScreen").GetComponent<MenuScreen>();
        slot = this.transform.GetSiblingIndex();
        isExpand = false;
        setExpand(isExpand);
    }

    public void toggleExpand()
    {
        //Debug.Log("test");
        setExpand(!isExpand);
    }

    private void setExpand(bool expand)
    {
        if (expand)
        {
            isExpand = expand;
            this.descriptionPanel.SetActive(true);
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            size.y = expandedHeight;
            this.GetComponent<RectTransform>().sizeDelta = size;
            //Debug.Log("equipped item expand done");
        }
        else
        {
            isExpand = expand;
            this.descriptionPanel.SetActive(false);
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            size.y = shrinkedHeight;
            this.GetComponent<RectTransform>().sizeDelta = size;
            //Debug.Log("equipped item shrink done");
        }
    }

    public void LoadEquipment (string item)
    {
        if (item.Equals("null")) {
            this.equippedItem = null;
            this.HPBonus.GetComponent<TMP_Text>().SetText("HP: +0");
            this.AttackBonus.GetComponent<TMP_Text>().SetText("Attack: +0");
            this.DefenseBonus.GetComponent<TMP_Text>().SetText("Defense: +0");
            this.SpeedBonus.GetComponent<TMP_Text>().SetText("Speed: +0");
            this.Image.GetComponent<Image>().sprite = null;
            return;
        }
        Debug.Log("Equipping: " + item);
        this.equippedItem = equipmentItemMap[item];
        Debug.Log("Loading equipment: " + item + " - " + equippedItem.itemName);
        Debug.Log("Current text: " + this.AttackBonus.GetComponent<TMP_Text>().text);
        Debug.Log("Attack: " + equippedItem.attackBoost.ToString());
        this.HPBonus.GetComponent<TMP_Text>().SetText("HP: +" + equippedItem.healthBoost.ToString());
        this.AttackBonus.GetComponent<TMP_Text>().SetText("Attack: +" + equippedItem.attackBoost.ToString());
        this.DefenseBonus.GetComponent<TMP_Text>().SetText("Defense: +" + equippedItem.defenseBoost.ToString());
        this.SpeedBonus.GetComponent<TMP_Text>().SetText("Speed: +" + equippedItem.speedBoost.ToString());
        this.Image.GetComponent<Image>().sprite = equippedItem.icon;
    }

    public void LoadDropdown(List<InventoryDataEquipment> equippables)
    {
        equipmentOptions.options.Clear();
        equipmentIDKey = new List<string>();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        if(equippedItem != null)
        {
            TMP_Dropdown.OptionData optionCurrent = new TMP_Dropdown.OptionData();
            optionCurrent.text = this.equippedItem.itemName;
            optionCurrent.image = this.equippedItem.icon;
            options.Add(optionCurrent);
            equipmentIDKey.Add("item_current");
        }

        TMP_Dropdown.OptionData optionUnequip = new TMP_Dropdown.OptionData();
        optionUnequip.text = "(Unequip)";
        optionUnequip.image = null;
        options.Add(optionUnequip);
        equipmentIDKey.Add("null");

        foreach (InventoryDataEquipment equippable in equippables)
        {
            if(equippable.Count <= 0) { continue; }
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = equipmentItemMap[equippable.ItemID].itemName;
            option.image = equipmentItemMap[equippable.ItemID].icon;
            options.Add(option);
            equipmentIDKey.Add(equippable.ItemID);
        }

        equipmentOptions.AddOptions(options);

        int dropdown_value_temp = 0;
        if(equippedItem != null)
        {
            dropdown_value_temp = equipmentIDKey.FindIndex(x => x.Equals(equippedItem.itemID));
        }
        equipmentOptions.SetValueWithoutNotify(dropdown_value_temp);
    }

    public void ReloadEquipment()
    {
        int currentEquip = equipmentOptions.value;
        //Debug.Log("ReloadEquipment - currentEquip: " + currentEquip);
        //for (int i = 0; i < equipmentIDKey.Count; i++)
        //{
        //    Debug.Log("ReloadEquipment - EquipmentIDKey[" + i + "]: " + equipmentIDKey[i]);
        //}
        string equippedID = equipmentIDKey[currentEquip];
        if (equippedID.Equals("item_current")) { return; }
        if (this.equippedItem == null)
        {
            menuScreen.ChangeEquipment(transform.GetSiblingIndex(), equippedID, "null");
            LoadEquipment(equippedID);
            return;
        }
        menuScreen.ChangeEquipment(transform.GetSiblingIndex(), equippedID, this.equippedItem.itemID);
        LoadEquipment(equippedID);
    }

}
