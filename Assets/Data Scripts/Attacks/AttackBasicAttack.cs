using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Attacks/BasicAttack")]
public class AttackBasicAttack : ScriptableObject, IAttack
{
    public string attackID;
    public string attackName;
    public string attackDesc;
    public int cooldownTimer;
    public int cooldown;
    public bool isTargetEnemies;
    public Sprite abilityIcon;

    string IAttack.attackID { get => attackID; set => this.attackID = value; }
    string IAttack.attackName { get => attackName; set => this.attackName = value; }
    string IAttack.attackDesc { get => attackDesc; set => this.attackDesc = value; }
    int IAttack.cooldownTimer { get => cooldownTimer; set => this.cooldownTimer = value; }
    int IAttack.cooldown { get => cooldown; set => this.cooldown = value; }
    bool IAttack.isTargetEnemies { get => isTargetEnemies; set => this.isTargetEnemies = value; }
    Sprite IAttack.abilityIcon { get => abilityIcon; set => this.abilityIcon = value; }

    //public AttackBasicAttack()
    //{
    //    attackID = "BasicAttack";
    //    attackName = "Basic Attack";
    //    attackDesc = "Attacks a target with a slashing attack, dealing 100% of attack as damage. Good ol' reliable.";
    //    cooldown = 1;
    //    cooldownTimer = 1;
    //    isTargetEnemies = true;
    //}

    public virtual void Execute(EntityBase source, EntityBase target)
    {
        source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
        source.DealDamage(target, 100f);
        target.GainAggro(-40);
        source.GainAggro(10);
    }

    public virtual bool isReady(EntityBase source)
    {
        return true;
    }

    public virtual IAttack GetNewInstance()
    {
        IAttack attack = CreateInstance<AttackBasicAttack>();
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
