using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TitleScreen;
using UnityEngine.UI;

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
        //UpdateCooldown();
    }
}
