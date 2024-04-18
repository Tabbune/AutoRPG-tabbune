using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack {
    string attackID { get; set; }
    string attackName { get; set; }
    string attackDesc { get; set; }
    int cooldownTimer { get; set; }
    int cooldown { get; set; }
    bool isTargetEnemies { get; set; }
    Sprite abilityIcon { get; set; }

    void Execute(EntityBase source, EntityBase target = null);

    bool isReady(EntityBase source);

    IAttack GetNewInstance();

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

   
}
