using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITactic;

[System.Serializable]
public class PlayerCharacterData 
{
    public string baseCharacter;
    public int level;
    public int experience;
    public List<PlayerDataTactic> tacticList;
    public string[] equipments;
}
[System.Serializable]
public class PlayerDataTactic
{
    public int order;
    public string TacticID;
    public int argument;
    public string AttackID;
}
