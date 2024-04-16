using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    string attackID { get; set; }
    string attackName { get; set; }
    string attackDesc { get; set; }
    EntityBase source { get; set; }
    int cooldownTimer { get; set; }
    int cooldown { get; set; }
    bool isTargetEnemies { get; set; }
    EntityBase target { get; set; }

    void Execute(EntityBase source, EntityBase target = null);

    bool isReady(EntityBase source); 

    //public static IAttack NewAttack(string input)
    //{
    //    IAttack attack;
    //    if(input == "HealingChant")
    //    {
    //        attack = new AttackHealingChant();
    //        return attack;
    //    }
    //    else if(input == "AssassinStrike")
    //    {
    //        attack = new AttackAssassinStrike();
    //        return attack;
    //    }
    //    else if (input == "TangleStrike")
    //    {
    //        attack = new AttackTangleStrike();
    //        return attack;
    //    }
    //    else
    //    attack = new AttackBasicAttack();
    //    return attack;
    //}

    public class AttackBasicAttack : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }


        public AttackBasicAttack()
        {
            attackID = "BasicAttack";
            attackName = "Basic Attack";
            attackDesc = "Attacks a target with a slashing attack, dealing 100% of attack as damage. Good ol' reliable.";
            cooldown = 1;
            cooldownTimer = 1;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            source.DealDamage(target, 100f);
            target.GainAggro(-40);
            source.GainAggro(10);
        }

        public bool isReady(EntityBase source)
        {
            return true;
        }
    }

    public class AttackHealingChant : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }


        public AttackHealingChant()
        {
            attackID = "HealingChant";
            attackName = "Healing Chant";
            attackDesc = "Heals the entire party for 200% Attack. (MP cost: 30, CD: 3 turn)";
            cooldown = 3;
            cooldownTimer = 3;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            foreach(EntityBase entity in CombatLoop.entityList)
            {
                if(source.isEnemy == entity.isEnemy)
                {
                    entity.GetHealing(Mathf.RoundToInt(source.CurrentAttack() * 200 / 100));
                }
            }
            source.gainMP(-30);
            source.GainAggro(30);
            cooldownTimer = 0;
        }

        public bool isReady(EntityBase source)
        {
            //Debug.Log("checking cooldown");
            if (cooldownTimer < cooldown) { return false; }
            //Debug.Log("checking mana cost, current MP is: " + source.getCurrentMP());
            if (source.getCurrentMP() < 30) { return false; }
            return true;
        }
    }

    public class AttackAssassinStrike : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }

        public AttackAssassinStrike()
        {
            attackID = "AssassinStrike";
            attackName = "Assassin Strike";
            attackDesc = "Deals 50% of attack as damage, then an additional 150% if the enemy has half HP or less. (CD: 2 turn)";
            cooldown = 2;
            cooldownTimer = 2;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            source.DealDamage(target, 50f);
            if(target.getCurrentHPRatio() <= 50)
            {
                source.DealDamage(target, 150f);
            }
            source.GainAggro(-20);
            cooldownTimer = 0;
        }

        public bool isReady(EntityBase source)
        {
            if (cooldownTimer < cooldown) { return false; }
            return true;
        }
    }
    public class AttackTangleStrike : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }

        public AttackTangleStrike()
        {
            attackID = "TangleStrike";
            attackName = "Tangle Strike";
            attackDesc = "Deals 75% of attack as damage, then delays the enemy by 100%. (MP cost: 10, CD: 4 turn)";
            cooldown = 4;
            cooldownTimer = 4;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            source.DealDamage(target, 50f);
            target.AV = 2 * target.AV;
            source.gainMP(-30);
            source.GainAggro(30);
            cooldownTimer = 0;
        }

        public bool isReady(EntityBase source)
        {
            if (cooldownTimer < cooldown) { return false; }
            if (source.getCurrentMP() < 10) { return false; }
            return true;
        }
    }
    public class AttackPoisonStrike : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }

        public AttackPoisonStrike()
        {
            attackID = "PoisonStrike";
            attackName = "Poison Strike";
            attackDesc = "Deals 50% of attack as damage, then applies a 50% of attack as 4-turn poison. (MP cost: 10, CD: 4 turn)";
            cooldown = 4;
            cooldownTimer = 4;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            source.DealDamage(target, 50f);
            Debug.Log("Applying poison: " + Mathf.FloorToInt(source.CurrentAttack() * 0.5f).ToString());
            target.ReceiveStatus("StatusPoison", Mathf.FloorToInt(source.CurrentAttack() * 0.5f), 4);
            cooldownTimer = 0;
        }

        public bool isReady(EntityBase source)
        {
            if (cooldownTimer < cooldown) { return false; }
            if (source.getCurrentMP() < 10) { return false; }
            return true;
        }
    }
    public class AttackBlessingOfZeal : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }

        public AttackBlessingOfZeal()
        {
            attackID = "BlessingOfZeal";
            attackName = "Blessing of Zeal ";
            attackDesc = "Grants your party +40% attack for 3 turns (MP cost: 30, CD: 4 turn)";
            cooldown = 4;
            cooldownTimer = 4;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
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

        public bool isReady(EntityBase source)
        {
            if (cooldownTimer < cooldown) { return false; }
            if (source.getCurrentMP() < 30) { return false; }
            return true;
        }
    }
    public class AttackSwordsDance : IAttack
    {
        public string attackID { get; set; }
        public string attackName { get; set; }
        public string attackDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int cooldownTimer { get; set; }
        public int cooldown { get; set; }
        public bool isTargetEnemies { get; set; }

        public AttackSwordsDance()
        {
            attackID = "SwordsDance";
            attackName = "Swords Dance";
            attackDesc = "Gain +100% attack for 3 turns (MP cost: 30, CD: 4 turn)";
            cooldown = 4;
            cooldownTimer = 4;
            isTargetEnemies = true;
        }

        public void Execute(EntityBase source, EntityBase target)
        {
            source.DeclareAttack(source.getEntityName() + " uses " + this.attackName);
            //Debug.Log("Applying poison: " + Mathf.FloorToInt(source.currentAttack() * 0.5f).ToString());

            source.ReceiveStatus("StatusAttackUp", Mathf.FloorToInt(source.GetBaseAttack() * 1f), 4);

            source.gainMP(-30);
            source.GainAggro(50);
            cooldownTimer = 0;
        }

        public bool isReady(EntityBase source)
        {
            if (cooldownTimer < cooldown) { return false; }
            if (source.getCurrentMP() < 30) { return false; }
            return true;
        }
    }
}
