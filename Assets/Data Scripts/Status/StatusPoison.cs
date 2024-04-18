using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName = "Status/StatusPoison")]
public class StatusPoison : StatusEmpty
{
    public override void AtEndOfTurn()
    {
        //Debug.Log(entity.getEntityName() + " is taking " + argument.ToString() + " poison damage");
        entity.TakePoisonDamage(argument);
        this.argument = argument / 2;
        this.ReduceDuration(1);
    }

    public override void ReduceDuration(int turns)
    {
        duration -= turns;
    }

    public override void StatChanges() { }

    public override void updateStatus(int argument, int turns)
    {
        this.argument = Mathf.Max(argument, this.argument);
        this.duration = Mathf.Max(turns, this.duration);
    }
    public override IStatus GetNewInstance(int argument, int duration, EntityBase entity)
    {
        IStatus status = CreateInstance<StatusPoison>();
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

    //public StatusPoison(int argument, int duration, EntityBase entity)
    //    : base(argument, duration, entity)
    //{
    //    this.statusID = "StatusPoison";
    //    this.argument = argument;
    //    this.duration = duration;
    //    this.entity = entity;
    //}
}
