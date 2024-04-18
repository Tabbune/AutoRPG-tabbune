using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Attacks/HealingChant")]
public class AttackHealingChant : AttackBasicAttack
{

    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        foreach (EntityBase entity in CombatLoop.entityList)
        {
            if (source.isEnemy == entity.isEnemy)
            {
                entity.GetHealing(Mathf.RoundToInt(source.CurrentAttack() * 200 / 100));
            }
        }
        source.gainMP(-30);
        source.GainAggro(30);
        cooldownTimer = 0;
    }

    public override bool isReady(EntityBase source)
    {
        //Debug.Log("checking cooldown");
        if (cooldownTimer < cooldown) { return false; }
        //Debug.Log("checking mana cost, current MP is: " + source.getCurrentMP());
        if (source.getCurrentMP() < 30) { return false; }
        return true;
    }

    public override IAttack GetNewInstance()
    {
        IAttack attack = CreateInstance<AttackHealingChant>();
        //IAttack attack = new AttackHealingChant();
        attack.attackID = this.attackID;
        attack.attackName = this.attackName;
        attack.attackDesc = this.attackDesc;
        attack.cooldownTimer = this.cooldownTimer;
        attack.cooldown = this.cooldown;
        attack.isTargetEnemies = this.isTargetEnemies;
        attack.abilityIcon = this.abilityIcon;

        return attack;
    }
}