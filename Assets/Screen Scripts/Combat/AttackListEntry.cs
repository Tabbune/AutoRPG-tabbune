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
        UpdateCooldown();
    }

    public void UpdateCooldown()
    {
        int cooldown = attack.cooldown - attack.cooldownTimer;
        TMP_Text text = this.transform.GetChild(1).GetComponent<TMP_Text>();
        Image icon = this.transform.GetChild(0).GetComponent<Image>();
        Color color = icon.color;
        if (cooldown > 0)
        {
            color.a = 0.5f;
            text.alpha = 1f;
        }
        else
        {
            color.a = 1f;
            text.alpha = 0f;
        }
        icon.color = color;
        text.SetText(cooldown.ToString());
    }
}
