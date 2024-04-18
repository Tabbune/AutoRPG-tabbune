using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Attacks/SwordsDance")]
public class AttackSwordsDance : AttackBasicAttack
{
    public override void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        //Debug.Log("Applying poison: " + Mathf.FloorToInt(source.currentAttack() * 0.5f).ToString());

        source.ReceiveStatus("StatusAttackUp", Mathf.FloorToInt(source.GetBaseAttack() * 1f), 4);

        source.gainMP(-30);
        source.GainAggro(50);
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
        IAttack attack = CreateInstance<AttackSwordsDance>();
        //IAttack attack = new AttackSwordsDance();
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
