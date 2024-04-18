using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatusEmpty")]
public class StatusEmpty : ScriptableObject, IStatus
{
    public string statusID;
    public string statusName;
    protected int argument;
    protected int duration;
    protected EntityBase entity;
    public bool isDispelable;
    public bool isBuff;
    public Sprite icon;

    string IStatus.statusID { get => statusID; set => this.statusID = value; }
    string IStatus.statusName { get => statusName; set => this.statusName = value; }
    int IStatus.argument { get => argument; set => this.argument = value; }
    int IStatus.duration { get => duration; set => this.duration = value; }
    EntityBase IStatus.entity { get => entity; set => this.entity = value; }
    bool IStatus.isDispelable { get => isDispelable; set => this.isDispelable = value; }
    bool IStatus.isBuff { get => isBuff; set => this.isBuff = value; }
    Sprite IStatus.icon { get => icon; set => this.icon = value; }

    public string StatusName()
    {
        return statusName;
    }

    public virtual void AtEndOfTurn()
    {
        this.ReduceDuration(1);
    }

    public virtual void ReduceDuration(int turns)
    {
        duration -= turns;
    }

    public virtual void StatChanges() { }

    public virtual void updateStatus(int argument, int turns)
    {
        this.argument = Mathf.Max(argument, this.argument);
        this.duration = Mathf.Max(turns, this.duration);
    }

    public virtual IStatus GetNewInstance(int argument, int duration, EntityBase entity)
    {
        IStatus status = CreateInstance<StatusEmpty>();
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
     
    
    //public StatusEmpty(int argument, int duration, EntityBase entity)
    //{
    //    this.statusID = "StatusEmpty";
    //    this.argument = argument;
    //    this.duration = duration;
    //    this.entity = entity;
    //}
}
