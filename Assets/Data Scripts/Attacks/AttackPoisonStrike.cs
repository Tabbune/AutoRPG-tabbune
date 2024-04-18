using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Attacks/PoisonStrike")]
public class AttackPoisonStrike : AttackBasicAttack
{
    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        source.DealDamage(target, 50f);
        Debug.Log("Applying poison: " + Mathf.FloorToInt(source.CurrentAttack() * 0.5f).ToString());
        target.ReceiveStatus("StatusPoison", Mathf.FloorToInt(source.CurrentAttack() * 0.5f), 4);
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
        IAttack attack = CreateInstance<AttackPoisonStrike>();
        //IAttack attack = new AttackPoisonStrike();
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
