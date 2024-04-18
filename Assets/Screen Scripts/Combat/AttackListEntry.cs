using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;

public class AttackListEntry : MonoBehaviour
{
    // Start is called before the first frame update

    public IAttack attack;

    void Awake()
    {
        attack = attackMap["BasicAttack"];
    }

    public void LoadData(IAttack attack)
    {
        this.attack = attack;
        this.transform.GetChild(0).GetComponent<Image>().sprite = this.attack.abilityIcon;
        //UpdateCooldown();
    }

    public void UpdateCooldown()
    {
        int cooldown = attack.cooldown - attack.cooldownTimer;
        this.transform.GetChild(1).GetComponent<TMP_Text>().SetText(cooldown.ToString());
    }
}
