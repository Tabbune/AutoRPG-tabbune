using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TitleScreen;
using Unity.VisualScripting;
using System.Xml.Linq;
using System.Reflection;

public class MenuScreen : MonoBehaviour
{
    private GameObject DetailScreen;
    private GameObject DetailScreenUnitEdit;
    private GameObject EditorPanel;
    private GameObject TeamPreview;
    private GameObject TacticContent;
    private int activeUnit;
    private PlayerCharacterEditorData activeUnitData;
    private GameObject TeambuilderContent;

    public PlayerCharacterThumbnail teambuilderIconPrefab;
    public GameObject teambuilderIconTemplate;

    public PlayerCharacterEditorData teamPreviewPrefab;
    public GameObject playerDataTacticTemplate;

    private List<InventoryDataEquipment>[] equipmentListBySlot;
    public InventoryConsumable inventoryConsumablePrefab;
    public InventoryEquipment inventoryEquipmentPrefab;
    public GameObject inventoryConsumableContent;
    public GameObject inventoryEquipmentContent;

    public EquippedItem equippedItemPrefab;
    public GameObject equipmentViewContent;

    private GameObject ItemDetailView;
    private IItems currentActiveItemPreview;
    private InventoryConsumable currentActiveConsumable;
    private InventoryEquipment currentActiveEquipment;

    private GameObject coinsDisplay;


    //Start is called before the first frame update
    void Start()
    {
        activeUnit = 0;
        //setActiveUnit(activeUnit);
        DetailScreen = GameObject.Find("DetailScreen");
        DetailScreenUnitEdit = GameObject.Find("DetailScreenUnitEdit");
        EditorPanel = GameObject.Find("EditorPanel");
        TeamPreview = GameObject.Find("TeamPreview");
        TacticContent = GameObject.Find("TacticContent");
        TeambuilderContent = GameObject.Find("TeambuilderContent");

        ItemDetailView = GameObject.Find("Item Detail View");
        currentActiveItemPreview = consumableItemMap["BronzeIngot"];

        coinsDisplay = GameObject.Find("CoinsDisplay");

        for (int i = 0; i < 4; i++)
        {
            //string teamMember = teamList[i];
            if (i >= teamList.Count) { break; }
            PlayerCharacterEditorData newPC = Instantiate(teamPreviewPrefab, TeamPreview.transform);
            newPC.LoadCharacter(teamList[i]);
            //Debug.Log("newPC name: " + newPC.getClassName());
        }


        foreach (string className in classDataDict.Keys)
        {
            PlayerCharacterThumbnail teambuilderIcon = Instantiate(teambuilderIconPrefab, TeambuilderContent.transform);
            teambuilderIcon.LoadCharacter(className);
        }

        foreach (InventoryDataConsumable item in saveData.inventoryConsumable)
        {
            InventoryConsumable inventoryItem = Instantiate(inventoryConsumablePrefab, inventoryConsumableContent.transform);
            inventoryItem.LoadItemData(item);
        }

        equipmentListBySlot = new List<InventoryDataEquipment>[6];
        for(int i = 0; i < 6; i++)
        {
            equipmentListBySlot[i] = new List<InventoryDataEquipment>();
        }

        bool defaultPreviewChange = false;
        foreach (InventoryDataEquipment item in saveData.inventoryEquipment)
        {
            InventoryEquipment inventoryItem = Instantiate(inventoryEquipmentPrefab, inventoryEquipmentContent.transform);
            inventoryItem.LoadItemData(item);
            equipmentListBySlot[item.Slot].Add(item);
            if (!defaultPreviewChange)
            {
                inventoryItem.changePreviewEquipment();
                defaultPreviewChange = true;
                currentActiveEquipment = inventoryItem;
            }
        }

        UpdateCoinsDisplay();

        TeamPreview.SetActive(false);
        DisableAllChild(DetailScreen);

        Debug.Log("Detail screen found: " + DetailScreen.name.ToString());
    }
    

    public void MoveToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OpenFightScreen(GameObject activeDetailScreen)
    {
        TeamPreview.SetActive(false);
        SetActiveUnit(-1);
        DisableAllChild(DetailScreen);
        activeDetailScreen.SetActive(true);
    }


    public void OpenTeambuildingScreen(GameObject activeDetailScreen)
    {
        TeamPreview.SetActive(true);


        DisableAllChild(DetailScreen);
        activeDetailScreen.SetActive(true);
        SetActiveUnit(-1);
        setSwapAvailable(false);
    }


    public void OpenUnitScreen(GameObject activeEditorPanel)
    {
        TeamPreview.SetActive(true);

        DisableAllChild(DetailScreen);
        DetailScreenUnitEdit.SetActive(true);

        DisableAllChild(EditorPanel);
        activeEditorPanel.SetActive(true);
        SetActiveUnit(activeUnit);
    }


    public void OpenItemsScreen(GameObject activeDetailScreen)
    {
        TeamPreview.SetActive(false);
        SetActiveUnit(-1);
        DisableAllChild(DetailScreen);
        activeDetailScreen.SetActive(true);
        OpenEquipments();
    }

    public void OpenConsumables()
    {
        inventoryConsumableContent.transform.parent.transform.parent.GameObject().SetActive(true);
        inventoryEquipmentContent.transform.parent.transform.parent.GameObject().SetActive(false);
    }
    public void OpenEquipments()
    {
        inventoryConsumableContent.transform.parent.transform.parent.GameObject().SetActive(false);
        inventoryEquipmentContent.transform.parent.transform.parent.GameObject().SetActive(true);
    }

    public void UpdateCoinsDisplay()
    {
        string coins = "Coins: " + saveData.Coins.ToString();
        coinsDisplay.transform.GetChild(0).GetComponent<TMP_Text>().SetText(coins);
    }

    private void DisableAllChild(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var child = parent.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
    }


    public void SetActiveUnit(int index)
    {
        if (index != -1) { activeUnit = index; }
        //Debug.Log("activeUnit = " + activeUnit.ToString());
        for (int i = 0; i < TeamPreview.transform.childCount; i++)
        {
            var child = TeamPreview.transform.GetChild(i).gameObject;
            if (child != null)
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            }
            if (i == index)
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                activeUnitData = child.GetComponent<PlayerCharacterEditorData>();
                if(activeUnitData == null){ Debug.Log("activeUnitData is null"); }

                RefreshProfilePage(activeUnitData);
                RefreshTacticPage(activeUnitData);
                RefreshEquipmentView(activeUnitData);
            }
            if (index == -1)
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
        }

        if (TeambuilderContent.activeSelf)
        {
            setSwapAvailable(true);
        }
    }

    public void setSwapAvailable(bool active)
    {
        for (int i = 0; i < TeambuilderContent.transform.childCount; i++)
        {
            var child = TeambuilderContent.transform.GetChild(i).gameObject;
            if (child == null) { break; }
            if (!active)
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                child.transform.GetChild(0).GetComponent<Button>().interactable = false;
            }
            else
            {
                child.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                child.transform.GetChild(0).GetComponent<Button>().interactable = true;
            }
        }
    }

    public void swapUnits(int newUnit)
    {
        PlayerCharacterThumbnail newTeamMember = TeambuilderContent.transform.GetChild(newUnit).GetComponent<PlayerCharacterThumbnail>();
        foreach(string character in teamList)
        {
            if(newTeamMember.getClassName() == character)
            {
                return;
            }
        }


        PlayerCharacterEditorData oldCharacter = TeamPreview.transform.GetChild(activeUnit).GetComponent<PlayerCharacterEditorData>();
        //Debug.Log("oldCharacter.name = " + oldCharacter.getClassName() + ", oldCharacterIndex: " + activeUnit.ToString());
        
        
        oldCharacter.LoadCharacter(newTeamMember.getClassName());

        teamList.RemoveRange(activeUnit, 1);
        teamList.Insert(activeUnit, newTeamMember.getClassName());

        saveData.TeamList = teamList;
        SaveCurrentData();
    }

    public void RefreshProfilePage(PlayerCharacterEditorData activeUnitData)
    {
        string profileText = activeUnitData.printProfile();
        string levelButtonText = "Level Up: " + activeUnitData.getLevelUpCost().ToString() + " coins";
        GameObject UnitProfileText = GameObject.Find("UnitProfileText");
        GameObject LevelUpButton = GameObject.Find("LevelUpButton");
        //Debug.Log(UnitProfileText.ToString());
        if (UnitProfileText != null)
        {
            UnitProfileText.GetComponentInChildren<TMP_Text>().SetText(profileText);
            LevelUpButton.transform.GetChild(0).GetComponent<TMP_Text>().SetText(levelButtonText);
        }
    }
    public void RefreshTacticPage(PlayerCharacterEditorData activeUnitData)
    {
        List<PlayerDataTactic> tactics = activeUnitData.getTactics();
        tactics.Sort((x, y) => x.order.CompareTo(y.order));

        while (TacticContent.transform.childCount > 0)
        {
            DestroyImmediate(TacticContent.transform.GetChild(0).gameObject);
        }
        //Debug.Log(tactics.ToString());

        foreach (PlayerDataTactic tactic in tactics)
        {
            NewTactic(tactic);
        }
    }

    public void RefreshEquipmentView(PlayerCharacterEditorData activeUnitData)
    {
        Debug.Log("equipmentViewContent.activeSelf: " + equipmentViewContent.activeInHierarchy.ToString());
        if (!equipmentViewContent.activeInHierarchy) { return; }
        while (equipmentViewContent.transform.childCount > 0)
        {
            Debug.Log("Clearing view content");
            DestroyImmediate(equipmentViewContent.transform.GetChild(0).gameObject);
        }

        for(int i = 0; i < 6; i++)
        {
            EquippedItem prefab = equippedItemPrefab;
            //Debug.Log(prefab.ToString(), equippedItemPrefab);
            Transform equipmentViewContentTransform = equipmentViewContent.transform;
            //Debug.Log(equipmentViewContentTransform.ToString(), equipmentViewContent);
            EquippedItem equip = Instantiate(prefab, equipmentViewContentTransform);
            //Debug.Log(equip.name, equip);

            equip.LoadEquipment(activeUnitData.getEquipmentAtSlot(i));

            foreach (InventoryDataEquipment equippable in equipmentListBySlot[i])
            {
                Debug.Log("Item " + equippable.ItemID + " can be equipped in slot: " + i.ToString());
            }
            equip.LoadDropdown(equipmentListBySlot[i]);
            //EquippedItem newEquipment = equip.GetComponent<EquippedItem>();
            //newEquipment.LoadEquipment(activeUnitData.getEquipmentAtSlot(i));
        }
    }

    public void ChangeEquipment(int slot, string equipment_ID, string previous_equipment_ID)
    {
        activeUnitData.ChangeEquipment(slot, equipment_ID);

        try
        {
            if (equipmentItemMap[previous_equipment_ID] != null)
            {
                equipmentListBySlot[slot].Find(x => x.ItemID.Equals(previous_equipment_ID)).Count += 1;
                saveData.inventoryEquipment.Find(x => x.ItemID.Equals(previous_equipment_ID)).Count += 1;
            }
        }
        catch{ }

        try
        {
            if (equipmentItemMap[equipment_ID] != null)
            {
                equipmentListBySlot[slot].Find(x => x.ItemID.Equals(equipment_ID)).Count -= 1;
                saveData.inventoryEquipment.Find(x => x.ItemID.Equals(equipment_ID)).Count -= 1;
            }
        }
        catch { }

        SaveEquipment();
        SaveCurrentData();
    }

    public void SaveEquipment()
    {

        PlayerCharacterData playerCharacterData;
        string className = activeUnitData.getClassName();
        playerCharacterData = PlayerCharacterEditorData.ExportData(className);

        for(int i = 0; i < playerCharacterData.equipments.Length; i++)
        {
            playerCharacterData.equipments[i] = activeUnitData.getEquipmentAtSlot(i);
        }

        string json = JsonUtility.ToJson(playerCharacterData);

        string file_path = Application.persistentDataPath + "/playerdata_" + className + ".json";
        Debug.Log(file_path + " " + json);
        File.WriteAllText(file_path, json);
        activeUnitData.LoadCharacter(className);
    }

    public void changeItemPreviewConsumable(InventoryConsumable inventoryConsumable)
    {
        currentActiveConsumable = inventoryConsumable;

        IItems item = inventoryConsumable.item;
        ItemDetailView.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
        ItemDetailView.transform.GetChild(1).GetComponent<TMP_Text>().SetText(item.itemName);
        ItemDetailView.transform.GetChild(2).GetComponent<TMP_Text>().SetText(item.itemDesc);

        currentActiveItemPreview = item;

        ItemDetailView.transform.GetChild(3).gameObject.SetActive(item.sellable);
        ItemDetailView.transform.GetChild(4).gameObject.SetActive(item.sellable);
    }


    public void changeItemPreviewEquipment(InventoryEquipment inventoryEquipment)
    {
        currentActiveEquipment = inventoryEquipment;

        IItems item = inventoryEquipment.item;
        ItemDetailView.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
        ItemDetailView.transform.GetChild(1).GetComponent<TMP_Text>().SetText(item.itemName);
        ItemDetailView.transform.GetChild(2).GetComponent<TMP_Text>().SetText(item.itemDesc);

        currentActiveItemPreview = item;

        ItemDetailView.transform.GetChild(3).gameObject.SetActive(item.sellable);
        ItemDetailView.transform.GetChild(4).gameObject.SetActive(item.sellable);
    }

    public void sellSingle()
    {
        Debug.Log("Selling item: " + currentActiveItemPreview.itemID);

        if (currentActiveItemPreview.isEquipment)
        {
            InventoryDataEquipment sellingItem = saveData.inventoryEquipment.Find(x => x.ItemID.Equals(currentActiveItemPreview.itemID));
            if(sellingItem.Count < 1) { return; }

            sellingItem.Count -= 1;
            saveData.Coins += currentActiveItemPreview.price;
            currentActiveEquipment.LoadItemData(sellingItem);
        }
        else
        {
            InventoryDataConsumable sellingItem = saveData.inventoryConsumable.Find(x => x.ItemID.Equals(currentActiveItemPreview.itemID));
            if (sellingItem.Count < 1) { return; }

            sellingItem.Count -= 1;
            saveData.Coins += currentActiveItemPreview.price;
            currentActiveConsumable.LoadItemData(sellingItem);
        }

        UpdateCoinsDisplay();
        SaveCurrentData();
    }
    public void sellBulk()
    {
        Debug.Log("Selling item: " + currentActiveItemPreview.itemID);

        if (currentActiveItemPreview.isEquipment)
        {
            InventoryDataEquipment sellingItem = saveData.inventoryEquipment.Find(x => x.ItemID.Equals(currentActiveItemPreview.itemID));
            if (sellingItem.Count < 10) { return; }

            sellingItem.Count -= 10;
            saveData.Coins += currentActiveItemPreview.price * 10;
            currentActiveEquipment.LoadItemData(sellingItem);
        }
        else
        {
            InventoryDataConsumable sellingItem = saveData.inventoryConsumable.Find(x => x.ItemID.Equals(currentActiveItemPreview.itemID));
            if (sellingItem.Count < 10) { return; }

            sellingItem.Count -= 10;    
            saveData.Coins += currentActiveItemPreview.price * 10;
            currentActiveConsumable.LoadItemData(sellingItem);
        }

        UpdateCoinsDisplay();
        SaveCurrentData();
    }

    public void LevelUp()
    {
        if(saveData.Coins < activeUnitData.getLevelUpCost())
        {
            return;
        }
        saveData.Coins -= activeUnitData.getLevelUpCost();
        SaveLevelUp();
        UpdateCoinsDisplay();
        SaveCurrentData();
        RefreshProfilePage(activeUnitData);
    }

    public void SaveLevelUp()
    {
        PlayerCharacterData playerCharacterData;
        string className = activeUnitData.getClassName();
        playerCharacterData = PlayerCharacterEditorData.ExportData(className);

        playerCharacterData.level += 1;

        string json = JsonUtility.ToJson(playerCharacterData);
        string file_path = Application.persistentDataPath + "/playerdata_" + className + ".json";
        Debug.Log(file_path + " " + json);
        File.WriteAllText(file_path, json);
        activeUnitData.LoadCharacter(className);
        RefreshTacticPage(activeUnitData);
    }

    public void AddNewTactic()
    {
        PlayerDataTactic tactic = new PlayerDataTactic();
        tactic.order = TacticContent.transform.childCount;
        tactic.TacticID = "Default";
        tactic.argument = 0;
        tactic.AttackID = "BasicAttack";
        NewTactic(tactic);
    }

    private void NewTactic(PlayerDataTactic tactic)
    {
        GameObject newTactic = Instantiate(playerDataTacticTemplate, TacticContent.transform);
        TMP_Dropdown tacticDropdown = newTactic.transform.Find("TacticType").GetComponentInChildren<TMP_Dropdown>();
        tacticDropdown.options.Clear();
        foreach (var tacticDropdownTactic in tacticMap)
        {
            tacticDropdown.options.Add(new TMP_Dropdown.OptionData(tacticDropdownTactic.Key));
            if (tacticDropdownTactic.Key == tactic.TacticID)
            {
                tacticDropdown.SetValueWithoutNotify(tacticMap[tactic.TacticID]);
            }
        }

        //Debug.Log("Text");
        TMP_Dropdown attackDropdown = newTactic.transform.Find("AttackSelector").GetComponentInChildren<TMP_Dropdown>();
        int index = 0;
        foreach (var attack in activeUnitData.getAttackPool())
        {
            if (activeUnitData.getLevel() >= attack.learnLevel)
            {
                attackDropdown.options.Add(new TMP_Dropdown.OptionData(attack.attackName));
                //Debug.Log("Class: " + activeUnitData.getClassName() + "AttackName: " + attack.attackName + " " + attack.learnLevel);

                //Debug.Log("attack.attackName: " + attack.attackName + " | tactic.AttackID: " + tactic.AttackID + " | index: " + index.ToString());
                if (attack.attackName.Equals(tactic.AttackID))
                {
                    //Debug.Log("text");
                    attackDropdown.value = index;
                }
                index++;
            }
        }
        newTactic.transform.Find("Argument").GetComponentInChildren<TMP_InputField>().text = tactic.argument.ToString();
        tacticDropdown.RefreshShownValue();
    }

    [ContextMenu ("GetEachTactic")]
    public void GetEachTactic()
    {
        
        for (int i = 0; i < TacticContent.transform.childCount; i++)
        {
            var child = TacticContent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                PlayerDataTactic tacticFromEditor = new PlayerDataTactic();
                tacticFromEditor.order = i;

                TMP_Dropdown tacticDropdown = child.transform.Find("TacticType").GetComponentInChildren<TMP_Dropdown>();
                tacticFromEditor.TacticID = tacticDropdown.options[tacticDropdown.value].text;

                TMP_Dropdown attackDropdown = child.transform.Find("AttackSelector").GetComponentInChildren<TMP_Dropdown>();
                tacticFromEditor.AttackID = attackDropdown.options[attackDropdown.value].text;

                TMP_InputField argumentInput = child.transform.Find("Argument").GetComponentInChildren<TMP_InputField>();
                tacticFromEditor.argument = Int32.Parse(argumentInput.text);

                Debug.Log("" + tacticFromEditor.TacticID + " " + tacticFromEditor.AttackID + " " + tacticFromEditor.argument.ToString()); ;
            }
        }
    }
    public void SaveTactics()
    {
        PlayerCharacterData playerCharacterData;
        string className = activeUnitData.getClassName();
        playerCharacterData = PlayerCharacterEditorData.ExportData(className);

        List<PlayerDataTactic> newTacticList = new List<PlayerDataTactic>();
        for (int i = 0; i < TacticContent.transform.childCount; i++)
        {
            var child = TacticContent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                PlayerDataTactic tacticFromEditor = new PlayerDataTactic();
                tacticFromEditor.order = i;

                TMP_Dropdown tacticDropdown = child.transform.Find("TacticType").GetComponentInChildren<TMP_Dropdown>();
                tacticFromEditor.TacticID = tacticDropdown.options[tacticDropdown.value].text;

                TMP_Dropdown attackDropdown = child.transform.Find("AttackSelector").GetComponentInChildren<TMP_Dropdown>();
                tacticFromEditor.AttackID = attackDropdown.options[attackDropdown.value].text;

                TMP_InputField argumentInput = child.transform.Find("Argument").GetComponentInChildren<TMP_InputField>();
                tacticFromEditor.argument = Int32.Parse(argumentInput.text);

                //Debug.Log("" + tacticFromEditor.TacticID + " " + tacticFromEditor.AttackID + " " + tacticFromEditor.argument.ToString()); ;
                newTacticList.Add(tacticFromEditor);
            }
        }

        playerCharacterData.tacticList = newTacticList;
        string json = JsonUtility.ToJson(playerCharacterData);

        string file_path = Application.persistentDataPath + "/playerdata_" + className + ".json";
        Debug.Log( file_path + " " + json);
        File.WriteAllText(file_path, json);
        activeUnitData.LoadCharacter(className);
        RefreshTacticPage(activeUnitData);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
