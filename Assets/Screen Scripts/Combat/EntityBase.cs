using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    private CombatUIHandler CombatUI;
    private CombatLoop CombatLoop;
    public bool isEnemy;
    public int aggro;
    public int AV;

    private string entityName { get; set; }
    private int level { get; set; }
    private int maxHP { get; set; }
    private int currentHP { get; set; }
    private int baseAttack { get; set; }
    private int attackBonus { get; set; }
    private int baseDefense { get; set; }
    private int defenseBonus { get; set; }
    private int baseSpeed { get; set; }
    private int speedBonus { get; set; }
    private int maxMP { get; set; }
    private int currentMP { get; set; }
    private int MPRegen { get; set; }
    public int passiveAggroGain { get; set; }

    public float baseDamageBonus { get; set; }
    public float critRate { get; set; }
    public float critDamage { get; set; }

    protected List<ITactic> tactics { get; set; }
    protected List<IAttack> attackList { get; set; }

    private List<IStatus> statusList { get; set; }

    // Start is called before the first frame update
    public void StartBase()
    {
        CombatUI = GameObject.Find("CombatUI").GetComponent<CombatUIHandler>();
        CombatLoop = GameObject.Find("LogicManager").GetComponent<CombatLoop>();
        currentHP = maxHP;
        attackBonus = 0;
        defenseBonus = 0;
        speedBonus = 0;
        //Debug.Log(entityName + " HP: " + currentHP.ToString() + "/" + maxHP.ToString());

        attackList = new List<IAttack>();
        tactics = new List<ITactic>();

        statusList = new List<IStatus>();
    }


    public int getLevel()
    {
        return level;
    }
    public string getEntityName()
    {
        return entityName;
    }
    public int getCurrentHPNumber()
    {
        return currentHP;
    }
    public int getMaxHPNumber()
    {
        return maxHP;
    }
    public int getCurrentHPRatio()
    {
        return Mathf.FloorToInt(currentHP * 100 / maxHP);
    }
    public int getCurrentMP()
    {
        return currentMP;
    }
    public int getCurrentMPRatio()
    {
        return Mathf.FloorToInt(currentMP * 100 / maxMP);
    }
    public int getCurrentMaxMP()
    {
        return maxMP;
    }

    public int GetBaseAttack()
    {
        return baseAttack;
    }

    public int CurrentAttack()
    {
        return Mathf.Max(0, baseAttack + attackBonus);
    }
    public int CurrentDefense()
    {
        return Mathf.Max(0, baseDefense + defenseBonus);
    }

    public int CurrentSpeed()
    {
        return Mathf.Max(1, baseSpeed + speedBonus);
    }
    public int GetMPRegen()
    {
        return MPRegen;
    }
    public List<IAttack> GetAttackList()
    {
        return attackList;
    }

    public void SetEntityName(string name)
    {
        this.entityName = name;
    }
    public void LoadLevel(int level)
    {
        this.level = level;
    }
    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
    }
    public void SetAttack(int attack)
    {
        this.baseAttack = attack;
    }
    public void SetAttackBonus(int attackBonus)
    {
        this.attackBonus += attackBonus;
    }
    public void SetDefense(int defense)
    {
        this.baseDefense = defense;
    }
    public void SetSpeed(int speed)
    {
        this.baseSpeed = speed;
    }
    public void SetMaxMP(int maxMP)
    {
        this.maxMP = maxMP;
    }
    public void setCurrentMP(int currentMP)
    {
        this.currentMP = currentMP;
    }
    public void setCritRate(int critRate)
    {
        this.critRate = critRate;
    }
    public void setCritDamage(int critDamage)
    {
        this.critDamage = critDamage;
    }
    public void setAggroGain(int passiveAggroGain)
    {
        this.passiveAggroGain = passiveAggroGain;
    }
    public void gainMP(int MP)
    {
        this.currentMP += MP;
        CombatUI.UpdateManabar(this);
    }

    public void DeclareAttack(string line)
    {
        CombatLoop.AddNewLine(line);
    }

    public void TakeTurn(EntityBase target)
    {
        bool success = false;
        foreach (ITactic tactic in tactics)
        {
            //Debug.Log("Entity " + this.entityName + " is using tactic: " + tactic.tacticName + " with attack " + tactic.attack.attackID);
            //CombatLoop.AddNewLine("Entity " + this.entityName + " is using tactic: " + tactic.tacticName + " with attack " + tactic.attack.attackID);
            success = tactic.invokeTactic(this, target);
            if (success) {
                CombatUI.UpdateAttack(this, tactic.attack);
                break; 
            }
        }
        if (!success) { 
            //Debug.Log("No applicable tactic");
            CombatLoop.AddNewLine("No applicable tactic");
        }

        EndOfTurnStatusCheck();
        GainAggro(passiveAggroGain);
        GainMP(GetMPRegen());
    }

    public void ReceiveStatus(string statusID, int argument, int duration)
    {
        foreach(IStatus status in statusList)
        {
            if(status.statusID == statusID)
            {
                status.updateStatus(argument, duration);
                CombatUI.UpdateStatus(this, status);
                return;
            }
        }
        IStatus newStatus = StatusFactory.GetStatus(statusID, this, argument, duration);
        CombatUI.AddStatus(this, newStatus);
        statusList.Add(newStatus);
    }

    public void CooldownTick()
    {
        foreach(IAttack attack in attackList)
        {
            if(attack.cooldownTimer < attack.cooldown) { 
                attack.cooldownTimer++;
                CombatUI.UpdateAttack(this, attack);
            }
        }
    }

    public void GainAggro(int aggroGain)
    {
        aggro = Mathf.Max(0, aggro + aggroGain);
    }
    public void GainMP(int MPGain)
    {
        currentMP = Mathf.Min(Mathf.Max(0, currentMP + MPGain), maxMP);
    }

    public void EndOfTurnStatusCheck()
    {
        for(int i = statusList.Count - 1; i >= 0; i--)
        {
            IStatus status = statusList[i];
            status.AtEndOfTurn();
            CombatUI.UpdateStatus(this, status);
            if(status.duration <= 0)
            {
                CombatUI.DeleteStatus(this, status);
                statusList.RemoveAt(i);
            }
        }
    }

    public void GetHealing(int healAmount)
    {
        this.currentHP = Mathf.Min(maxHP, currentHP + healAmount);
        //Debug.Log(entityName + " got healed for " + healAmount.ToString() + " current HP is " + currentHP.ToString());
        CombatLoop.AddNewLine(entityName + " got healed for " + healAmount.ToString() + " current HP is " + currentHP.ToString());
        //CombatUI.UpdateHealthbar(this);
    }

    public void DealDamage(EntityBase target, float motionValuePercent, int extraAttackMod = 0, float extraDamageBonus = 0, float extraCritRate = 0, float extraCritDamage = 0 )
    {
        //Base damage dealt is attack * motionValue * damage bonus modifier * crit modifier

        int attack = this.CurrentAttack() + extraAttackMod;
        float motionValue = motionValuePercent / 100f;

        float damageDealt = attack * motionValue * (1 + this.baseDamageBonus + extraDamageBonus);

        float critRoll = Random.Range(0f, 100f);
        if (critRoll < (this.critRate + extraCritRate))
        {
            Debug.Log("Crit! " + this.entityName + " is attacking for " + (damageDealt * (1 + (this.critDamage / 100f) + (extraCritDamage / 100f))).ToString());
            //CombatLoop.AddNewLine("Crit! " + this.entityName + " is attacking for " + (damageDealt * (1 + (this.critDamage / 100f) + (extraCritDamage / 100f))).ToString());
            CombatLoop.AddNewLine("Critical Hit!");
            target.TakeDamage(damageDealt * (1 + (this.critDamage / 100f) + (extraCritDamage / 100f)));
        }
        else
        {
            Debug.Log(this.entityName + " is attacking for " + damageDealt.ToString());
            //CombatLoop.AddNewLine(this.entityName + " is attacking for " + damageDealt.ToString());
            target.TakeDamage(damageDealt);
        }
    }

    public void TakeDamage(float baseDamage)
    {
        //Damage formula is:
        //(base damage * 10000)/(defense+90) ^ 2
        //At 0 Defense, the target takes x1.56 damage
        //At 10 Defense, the target takes x1 damage
        //At 50 Defense, the target takes x0.51 damage
        int defense = this.CurrentDefense();
        //Debug.Log("Defense = " + defense.ToString());
        float defenseModifier = 10000f / ((defense + 90) * (defense + 90));
        //Debug.Log("Defense multiplier = " + defenseModifier.ToString());
        int damageTaken = Mathf.FloorToInt( baseDamage * defenseModifier);

        this.currentHP -= damageTaken;
        //Debug.Log("Taking " + damageTaken.ToString() + " damage, HP remaning: " + currentHP.ToString());
        //CombatLoop.AddNewLine(this.entityName + " takes " + damageTaken.ToString() + " damage, HP remaning: " + currentHP.ToString());
        CombatLoop.AddNewLine(this.entityName + " takes " + damageTaken.ToString() + " damage");
        //CombatUI.UpdateHealthbar(this);
    }

    public void TakeTrueDamage(float baseDamage)
    {
        this.currentHP -= Mathf.FloorToInt(baseDamage);
        //Debug.Log("Taking " + Mathf.FloorToInt(baseDamage).ToString() + " damage, HP remaning: " + currentHP.ToString());
        //CombatLoop.AddNewLine(this.entityName + " takes " + Mathf.FloorToInt(baseDamage).ToString() + " damage, HP remaning: " + currentHP.ToString());
        CombatLoop.AddNewLine(this.entityName + " takes " + Mathf.FloorToInt(baseDamage).ToString() + " damage");
        //CombatUI.UpdateHealthbar(this);
    }

    public void TakePoisonDamage(float baseDamage)
    {
        this.currentHP -= Mathf.FloorToInt(baseDamage);
        //Debug.Log("Taking " + Mathf.FloorToInt(baseDamage).ToString() + " damage, HP remaning: " + currentHP.ToString());
        //CombatLoop.AddNewLine(this.entityName + " takes " + Mathf.FloorToInt(baseDamage).ToString() + " damage, HP remaning: " + currentHP.ToString());
        CombatLoop.AddNewLine(this.entityName + " takes " + Mathf.FloorToInt(baseDamage).ToString() + " poison damage");
        //CombatUI.UpdateHealthbar(this);
    }

}
