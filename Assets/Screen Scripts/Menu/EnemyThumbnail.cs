using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TitleScreen;
using UnityEngine.SceneManagement;

public class EnemyThumbnail : MonoBehaviour
{
    private string enemyName;
    private string enemyCallID;
    private int level;
    private Sprite icon;

    // Start is called before the first frame update
    private void Awake()
    {
        LoadEnemy("WhiteSnake10");
    }

    public void LoadEnemy(string enemyID)
    {
        EnemyClassData enemyClassData = enemyDataDict[enemyID];

        enemyName = enemyClassData.className;
        this.enemyCallID = enemyClassData.enemyID;
        level = enemyClassData.level;
        icon = enemyClassData.icon;

        this.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        string enemyTitle = "Lv." + level.ToString() + " " + enemyName;
        this.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = enemyTitle;
    }

    public void MoveToCombatScene()
    {
        enemyList.Clear();
        enemyList.Add(this.enemyCallID);
        SceneManager.LoadScene("Combat Screen");
    }
}
