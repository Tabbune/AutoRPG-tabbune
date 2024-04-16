using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;

public class LootEntry : MonoBehaviour
{
    public IItems itemData;
    public string itemName;
    public Sprite icon;

    // Start is called before the first frame update

    void Awake()
    {
        LoadItemData(consumableItemMap["BronzeIngot"]);
    }

    public void LoadItemData(IItems itemData)
    {
        this.itemData = itemData;
        this.itemName = itemData.itemName;
        this.icon = itemData.icon;

        this.transform.GetChild(0).GetComponent<Image>().sprite = this.icon;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
