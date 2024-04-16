using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatus;
using static ITactic;
using static TitleScreen;

public interface ITactic
{
    string tacticID { get; set; }
    string tacticName { get; set; }
    string tacticDesc { get; set; }
    int argument { get; set; }
    EntityBase source { get; set; }
    IAttack attack { get; set; }
    int tacticCost { get; set; }

    public bool invokeTactic(EntityBase source, EntityBase target = null);


    public class TacticDefault : ITactic
    {
        public string tacticID { get; set; }
        public string tacticName { get; set; }
        public string tacticDesc { get; set; }
        public int argument { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public IAttack attack { get; set; }
        public int tacticCost { get; set; }

        public TacticDefault(EntityBase source, IAttack attack, int argument = 0)
        {
            this.tacticID = "Default";
            this.tacticName = "Default";
            this.tacticDesc = "Always activates if the attack is ready";
            this.source = source;
            this.attack = attack;
            this.tacticCost = 0;
        }

        public bool invokeTactic(EntityBase source, EntityBase target)
        {
            if (attack.isReady(source))
            {
                attack.Execute(source, target);
                return true;
            }
            return false;
        }
    }
    public class TacticIsEnemyHPGreaterThan : ITactic
    {
        public string tacticID { get; set; }
        public string tacticName { get; set; }
        public string tacticDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int argument { get; set; }
        public IAttack attack { get; set; }
        public int tacticCost { get; set; }

        public TacticIsEnemyHPGreaterThan(EntityBase source, IAttack attack, int argument)
        {
            this.tacticID = "EnemyHPGreaterThan";
            this.tacticName = "Opener";
            this.tacticDesc = "Activates if the enemy's current HP percentage is X or higher";
            this.source = source;
            this.argument = argument;
            this.attack = attack;
            this.tacticCost = 10;
        }

        public bool invokeTactic(EntityBase source, EntityBase target)
        {
            if (!attack.isReady(source))
            {
                return false;
            }
            Debug.Log("Enemy HP ratio is: " + target.getCurrentHPRatio());
            if ((target.getCurrentHPRatio()) < argument)
            {
                return false;
            }
            attack.Execute(source, target);
            return true;
        }
    }

    public class TacticIsEnemyHPLowerThan : ITactic
    {
        public string tacticID { get; set; }
        public string tacticName { get; set; }
        public string tacticDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int argument { get; set; }
        public IAttack attack { get; set; }
        public int tacticCost { get; set; }

        public TacticIsEnemyHPLowerThan(EntityBase source, IAttack attack, int argument)
        {
            this.tacticID = "EnemyHPLowerThan";
            this.tacticName = "Closer";
            this.tacticDesc = "Activates if the enemy's current HP percentage is X or lower";
            this.source = source;
            this.argument = argument;
            this.attack = attack;
            this.tacticCost = 10;
        }

        public bool invokeTactic(EntityBase source, EntityBase target)
        {
            if (!attack.isReady(source))
            {
                return false;
            }
            if ((target.getCurrentHPRatio()) > argument)
            {
                return false;
            }
            attack.Execute(source, target);
            return true;
        }
    }

    public class TacticIsAllyLowHP : ITactic
    {
        public string tacticID { get; set; }
        public string tacticName { get; set; }
        public string tacticDesc { get; set; }
        public EntityBase source { get; set; }
        public EntityBase target { get; set; }
        public int argument { get; set; }
        public IAttack attack { get; set; }
        public int tacticCost { get; set; }

        public TacticIsAllyLowHP(EntityBase source, IAttack attack, int argument)
        {
            this.tacticID = "AllyHPLowerThan";
            this.tacticName = "Savior";
            this.tacticDesc = "Activates if an ally is below a certain HP";
            this.source = source;
            this.argument = argument;
            this.attack = attack;
            this.tacticCost = 10;
        }

        public bool invokeTactic(EntityBase source, EntityBase target)
        {
            if (!attack.isReady(source))
            {
                //Debug.Log("1");
                return false;
            }
            foreach(EntityBase entity in CombatLoop.entityList)
            {
                if(source.isEnemy == entity.isEnemy && entity.getCurrentHPRatio() <= argument)
                {
                    //Debug.Log("2");
                    attack.Execute(source, target);
                    return true;
                }
            }
            //Debug.Log("3");
            return false;
        }
    }
}

public static class TacticFactory
{
    public static ITactic GetTactic(string tactic, EntityBase source, IAttack attack, int argument = 0)
    {
        switch (tactic)
        {
            case "Default":
                return new TacticDefault(source, attack, argument);
            case "EnemyHPGreaterThan":
                return new TacticIsEnemyHPGreaterThan(source, attack, argument);
            case "EnemyHPLowerThan":
                return new TacticIsEnemyHPLowerThan(source, attack, argument);
            case "AllyHPLowerThan":
                return new TacticIsAllyLowHP(source, attack, argument);
        }
        return new TacticDefault(source, attackMap["BasicAttack"]);
    }
}
