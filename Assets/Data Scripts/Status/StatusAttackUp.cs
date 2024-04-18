using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatusAttackUp")]
public class StatusAttackUp : StatusEmpty
{
    public override void AtEndOfTurn()
    {
        this.ReduceDuration(1);
    }

    public override void ReduceDuration(int turns)
    {
        duration -= turns;
        if (this.duration <= 0)
        {
            entity.SetAttackBonus(-this.argument);
        }
    }

    public override void StatChanges() { }

    public override void updateStatus(int argument, int turns)
    {
        if (argument > this.argument)
        {
            entity.SetAttackBonus(-this.argument);
            this.argument = argument;
            this.duration = turns;
            entity.SetAttackBonus(argument);
        }
    }

    public override IStatus GetNewInstance(int argument, int duration, EntityBase entity)
    {
        IStatus status = CreateInstance<StatusAttackUp>();
        status.statusID = this.statusID;
        status.statusName = this.statusName;
        status.argument = argument;
        status.duration = duration;
        status.entity = entity;
        status.isDispelable = this.isDispelable;
        status.isBuff = this.isBuff;
        status.icon = this.icon;
        return status;
    }

    //public StatusAttackUp(int argument, int duration, EntityBase entity)
    //    : base(argument, duration, entity)
    //{
    //    this.statusID = "StatusAttackUp";
    //    this.argument = argument;
    //    this.duration = duration;
    //    this.entity = entity;

    //    entity.SetAttackBonus(argument);
    //}
}
