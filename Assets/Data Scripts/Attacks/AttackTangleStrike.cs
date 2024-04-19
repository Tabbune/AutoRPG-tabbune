using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/TangleStrike")]
public class AttackTangleStrike : AttackBasicAttack
{
    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        source.DealDamage(target, 50f);
        target.AV = Mathf.FloorToInt(target.AV + ((100f/100f) * target.AV));
        source.gainMP(-30);
        source.GainAggro(30);
        cooldownTimer = 0;
    }

    public override bool isReady(EntityBase source)
    {
        if (cooldownTimer < cooldown) { return false; }
        if (source.getCurrentMP() < 10) { return false; }
        return true;
    }


    public override IAttack GetNewInstance()
    {
        IAttack attack = CreateInstance<AttackTangleStrike>();
        //IAttack attack = new AttackTangleStrike();
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
