using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IStatus;
using static ITactic;
using static UnityEngine.EventSystems.EventTrigger;
using static TitleScreen;

public interface IStatus
{
    string statusID { get; set; }
    string statusName { get; set; }
    int argument { get; set; }
    int duration { get; set; }
    EntityBase entity { get; set; }
    bool isDispelable { get; set; }
    bool isBuff { get; set; }
    Sprite icon { get; set; }

    public string StatusName();
    public void AtEndOfTurn();
    public void ReduceDuration(int turns);
    public void StatChanges();
    public void updateStatus(int argument, int turns);
    public IStatus GetNewInstance(int argument, int duration, EntityBase entity);
        
}


public static class StatusFactory
{
    public static IStatus GetStatus(string status, EntityBase entity, int argument = 0, int duration = 0)
    {
        try
        {
            IStatus newStatus = statusMap[status].GetNewInstance(argument, duration, entity);
            return newStatus;
        }
        catch
        {
            Debug.Log("Status not found: ");
            return statusMap["StatusEmpty"].GetNewInstance(argument, duration, entity);
        }
    }
}