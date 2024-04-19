using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TitleScreen;
using UnityEngine.UI;
using TMPro;

public class StatusListEntry : MonoBehaviour
{
    public IStatus status;
    void Awake()
    {
        status = statusMap["StatusEmpty"];
    }


    public void LoadData(IStatus status)
    {
        this.status = status;
        this.transform.GetChild(0).GetComponent<Image>().sprite = this.status.icon;
        UpdateCooldown();
    }

    public void UpdateCooldown()
    {
        int duration = this.status.duration;
        this.transform.GetChild(1).GetComponent<TMP_Text>().SetText(duration.ToString());
    }
    //public void DeleteIcon()
    //{
    //    Debug.Log("Destroying this object", this);
    //    Destroy(this);
    //}
}
