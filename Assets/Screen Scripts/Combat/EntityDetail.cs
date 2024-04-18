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

    private EntityBase entity;

    public TMP_Text EntityName;
    public TMP_Text HPNumber;
    public Slider HPBar;
    public TMP_Text MPNumber;
    public Slider MPBar;

    public AttackListEntry attackListEntryPrefab;
    public StatusListEntry statusListEntryPrefab;

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
        this.entity = entity;
        //Debug.Log("New Character Detail: " + player.getEntityName());
        EntityName.SetText("Lv. " + this.entity.getLevel() + " " + this.entity.getEntityName());
        isEnemy = this.entity.isEnemy;

        if (!isEnemy)
        {
            GameObject attackList = this.transform.GetChild(4).transform.GetChild(0).GetChild(0).gameObject;
            foreach (IAttack attack in this.entity.GetAttackList())
            {
                AttackListEntry attackListEntry = Instantiate(attackListEntryPrefab, attackList.transform);
                attackListEntry.LoadData(attack);
            }
        }

        UpdateHPBar(this.entity);
    }

    public void AddStatus(EntityBase entity, IStatus status)
    {
        GameObject statusList;
        if (entity.isEnemy)
        {
            statusList = this.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        }
        else
        {
            statusList = this.transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
        }
        StatusListEntry statusListEntry = Instantiate(statusListEntryPrefab, statusList.transform);
        statusListEntry.LoadData(status);
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

}
