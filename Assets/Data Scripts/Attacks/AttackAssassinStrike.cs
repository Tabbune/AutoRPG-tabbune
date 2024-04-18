using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Attacks/AssassinStrike")]
public class AttackAssassinStrike : AttackBasicAttack
{
    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        source.DealDamage(target, 50f);
        if (target.getCurrentHPRatio() <= 50)
        {
            source.DealDamage(target, 100f);
        }
        source.GainAggro(-20);
        cooldownTimer = 0;
    }

    public override bool isReady(EntityBase source)
    {
        if (cooldownTimer < cooldown) { return false; }
        return true;
    }


    public override IAttack GetNewInstance()
    {
        IAttack attack = CreateInstance<AttackAssassinStrike>();
        //IAttack attack = new AttackAssassinStrike();
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
