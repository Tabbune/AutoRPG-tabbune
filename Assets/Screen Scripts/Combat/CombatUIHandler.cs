using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatUIHandler : MonoBehaviour
{
    private List<string> displayedLines;
    private TMP_Text dialogBox;
    private TMP_Text AVTracker;

    private GameObject PartyDetail;
    private GameObject BossDetail;

    private Dictionary<string, EntityDetail> entityList;

    public EntityDetail characterDetailPrefab;
    public EntityDetail bossDetailPrefab;

    private bool isGamePaused;
    private GameObject pauseButton;
    public Sprite pauseIcon;
    public Sprite resumeIcon;

    void Awake()
    {
        displayedLines = new List<string>();
        entityList = new Dictionary<string, EntityDetail>();

        PartyDetail = GameObject.Find("PartyDetail");
        BossDetail = GameObject.Find("BossDetail");

        GameObject dialogBoxContainer = GameObject.Find("DialogText");
        dialogBox = dialogBoxContainer.GetComponent<TMP_Text>();

        GameObject AVTrackerContainer = GameObject.Find("AVTracker");
        AVTracker = AVTrackerContainer.GetComponent<TMP_Text>();

        pauseButton = GameObject.Find("PauseButton");
        isGamePaused = false;
    }

    public void CreateNewCharacterDetail(Player player)
    {
        //Debug.Log("New Character Detail: " + player.getEntityName());
        EntityDetail newCharacterDetail = Instantiate(characterDetailPrefab, PartyDetail.transform);
        newCharacterDetail.LoadCharacter(player);
        entityList.Add(player.getEntityName(), newCharacterDetail);
    }
    public void CreateNewBossDetail(EnemyMonster enemy)
    {
        //Debug.Log("New Character Detail: " + player.getEntityName());
        EntityDetail newCharacterDetail = Instantiate(bossDetailPrefab, BossDetail.transform);
        newCharacterDetail.LoadCharacter(enemy);
        entityList.Add(enemy.getEntityName(), newCharacterDetail);
    }

    public TMP_Text GetAVTracker()
    {
        return AVTracker;
    }
    public TMP_Text GetDialogBox()
    {
        return dialogBox;
    }
    public void DisplayNewLine(string line)
    {
        if(displayedLines.Count >= 2)
        {
            displayedLines.RemoveAt(0);
        }
        displayedLines.Add(line);
        string lines = "";
        foreach(string displayLine in displayedLines)
        {
            lines += displayLine + "\n";
        }
        Debug.Log(lines);
        dialogBox.SetText(lines);
    }
    public void ClearLines()
    {
        displayedLines.Clear();
    }

    public void UpdateHealthbar(EntityBase entity)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.UpdateHPBar(entity);
    }
    public void UpdateManabar(EntityBase entity)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.UpdateMPBar(entity);
    }
    public void UpdateAttack(EntityBase entity, IAttack attack)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.UpdateAttackIcon(attack);
    }
    public void AddStatus(EntityBase entity, IStatus status)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.AddStatus(entity, status);
    }
    public void UpdateStatus(EntityBase entity, IStatus status)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.UpdateStatusIcon(status);
    }

    public void DeleteStatus(EntityBase entity, IStatus status)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.DeleteStatusIcon(status);
    }
    public void ProcessDeath_UI(EntityBase entity)
    {
        EntityDetail entityDetail = entityList[entity.getEntityName()];
        entityDetail.ProcessDeath_UI();
    }

    public void TogglePause()
    {
        if (!isGamePaused)
        {
            PauseGame();
            return;
        }
        UnpauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseButton.transform.GetChild(0).GetComponent<Image>().sprite = resumeIcon;
        isGamePaused = true;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pauseButton.transform.GetChild(0).GetComponent<Image>().sprite = pauseIcon;
        isGamePaused = false;
    }

    public void ReturnToMenu()
    {
        UnpauseGame();
        SceneManager.LoadScene("Menu");
    }
}
