using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityDetail : MonoBehaviour
{
    private string className;
    private int currentHP;
    private int maxHP;
    
    private bool isEnemy;

    public TMP_Text EntityName;
    public TMP_Text HPNumber;
    public Slider HPBar;
    public TMP_Text MPNumber;
    public Slider MPBar;

    // Start is called before the first frame update
    void Awake()
    {
        EntityName = this.transform.GetChild(0).GetComponent<TMP_Text>();

        HPNumber = this.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        HPBar = this.transform.GetChild(1).GetComponent<Slider>();

        try
        {
            MPNumber = this.transform.GetChild(2).GetChild(2).GetComponent<TMP_Text>();
            MPBar = this.transform.GetChild(2).GetComponent<Slider>();
        }
        catch { 
            MPNumber = null; 
            MPBar = null; 
        };

        isEnemy = false;
    }

    public void LoadCharacter(EntityBase entity)
    {
        //Debug.Log("New Character Detail: " + player.getEntityName());
        EntityName.SetText("Lv. " + entity.getLevel() + " " + entity.getEntityName());
        isEnemy = entity.isEnemy;

        UpdateHPBar(entity);
    }
    public void UpdateHPBar(EntityBase entity)
    {
        if (entity.isEnemy)
        {
            HPNumber.SetText(entity.getCurrentHPRatio().ToString() + "%");
        }
        else
        {
            HPNumber.SetText(entity.getCurrentHPNumber().ToString() + " / " + entity.getMaxHPNumber().ToString());
        }
        HPBar.value = Mathf.Max(entity.getCurrentHPRatio(), 0f) / 100f;
    }
    public void UpdateMPBar(EntityBase entity)
    {
        if(MPBar == null) { return; }

        if (!entity.isEnemy)
        {
            MPNumber.SetText(entity.getCurrentMPRatio().ToString() + "%");
        }
        else
        {
            MPNumber.SetText(entity.getCurrentMP().ToString() + " / " + entity.getCurrentMaxMP().ToString());
        }
        MPBar.value = Mathf.Max(entity.getCurrentMPRatio(), 0f) / 100f;
    }

    public void ProcessDeath_UI()
    {
        //Color newColor = this.GetComponent<Image>().color;
        //newColor.a = 0;
        //this.GetComponent<Image>().color = newColor;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
