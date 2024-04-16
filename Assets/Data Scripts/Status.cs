using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IStatus;
using static ITactic;
using static UnityEngine.EventSystems.EventTrigger;

public interface IStatus
{
    string statusName { get; set; }
    int argument { get; set; }
    int duration { get; set; }
    EntityBase entity { get; set; }

    public string StatusName();
    public void AtEndOfTurn();
    public void ReduceDuration(int turns);
    public void StatChanges();
    public void updateStatus(int argument, int turns);

    public class StatusPoison : IStatus
    {
        public string statusName { get; set; }
        public int argument { get; set; }
        public int duration { get; set; }
        public EntityBase entity { get; set; }

        public string StatusName()
        {
            return statusName;
        }

        public void AtEndOfTurn()
        {
            Debug.Log(entity.getEntityName() + " is taking " + argument.ToString() + " poison damage");
            entity.TakeTrueDamage(argument);
            this.argument = argument / 2;
            this.ReduceDuration(1);
        }

        public void ReduceDuration(int turns)
        {
            duration -= turns;
        }

        public void StatChanges() { }

        public void updateStatus(int argument, int turns)
        {
            this.argument = Mathf.Max(argument, this.argument);
            this.duration = Mathf.Max(turns, this.duration);
        }

        public StatusPoison(int argument, int duration, EntityBase entity)
        {
            this.statusName = "StatusPoison";
            this.argument = argument;
            this.duration = duration;
            this.entity = entity;
        }
    }

    public class StatusAttackUp : IStatus
    {
        public string statusName { get; set; }
        public int argument { get; set; }
        public int duration { get; set; }
        public EntityBase entity { get; set; }

        public string StatusName()
        {
            return statusName;
        }

        public void AtEndOfTurn()
        {
            this.ReduceDuration(1);
        }

        public void ReduceDuration(int turns)
        {
            duration -= turns;
            if (this.duration <= 0)
            {
                entity.SetAttackBonus(-this.argument);
            }
        }

        public void StatChanges() { }

        public void updateStatus(int argument, int turns)
        {
            if(argument > this.argument)
            {
                entity.SetAttackBonus(-this.argument);
                this.argument = argument;
                this.duration = turns;
                entity.SetAttackBonus(argument);
            }
        }

        public StatusAttackUp(int argument, int duration, EntityBase entity)
        {
            this.statusName = "StatusAttackUp";
            this.argument = argument;
            this.duration = duration;
            this.entity = entity;

            entity.SetAttackBonus(argument);
        }
    }
}


public static class StatusFactory
{
    public static IStatus GetStatus(string status, EntityBase entity, int argument = 0, int duration = 0)
    {
        switch (status)
        {
            case "StatusPoison":
                return new StatusPoison(argument, duration, entity);
            case "StatusAttackUp":
                return new StatusAttackUp(argument, duration, entity);
        }
        Debug.Log("Status not found: ");
        return new StatusPoison(0, 0, entity);
    }
}