using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BlessingOfZeal")]
public class AttackBlessingOfZeal : AttackBasicAttack
{
    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        foreach (EntityBase entity in CombatLoop.entityList)
        {
            if (source.isEnemy == entity.isEnemy)
            {
                entity.ReceiveStatus("StatusAttackUp", Mathf.FloorToInt(entity.GetBaseAttack() * 0.4f), 4);
            }
        }
        //Debug.Log("Applying poison: " + Mathf.FloorToInt(source.currentAttack() * 0.5f).ToString());
        source.gainMP(-30);
        cooldownTimer = 0;
    }

    public override bool isReady(EntityBase source)
    {
        if (cooldownTimer < cooldown) { return false; }
        if (source.getCurrentMP() < 30) { return false; }
        return true;
    }
    public override IAttack GetNewInstance()
    {
        IAttack attack = CreateInstance<AttackBlessingOfZeal>();
        //IAttack attack = new AttackBlessingOfZeal();
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
