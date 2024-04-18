using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static TitleScreen;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using Unity.VisualScripting;

public class CombatLoop : MonoBehaviour
{
    const int ActionGauge = 100000;
    public float turnLength; //prototype
    private float attackTimer; //prototype
    public static List<EntityBase> entityList; //prototype

    //private bool isResolvingTurn;
    private bool endTurnPause;

    public GameObject playerCharacterParent;
    public GameObject enemyMonsterParent;

    private CombatUIHandler CombatUI;

    public Player playerCharacterPrefab;
    public EnemyMonster enemyCharacterPrefab;

    public int actionBarDisplayCount;
    public int actionBarMaxAV;

    private List<string> dialogQueue;

    // Start is called before the first frame update
    void Start()
    {
        //prototype
        attackTimer = 1f;
        //isResolvingTurn = false;
        //prototype

        endTurnPause = false;

        CombatUI = GameObject.Find("CombatUI").GetComponent<CombatUIHandler>();

        dialogQueue = new List<string>();


        entityList = new List<EntityBase>();

        for(int i = 0; i < 4; i++)
        {
            //string teamMember = teamList[i];
            if(i >= teamList.Count) { break; }
            Player newPC = Instantiate(playerCharacterPrefab, playerCharacterParent.transform);
            newPC.transform.localPosition += Vector3.down * (i * 1.5f);
            entityList.Add(newPC);

            CombatUI.CreateNewCharacterDetail(newPC);

            foreach(BonusStatusEffects bonusStatus in newPC.bonusEffects)
            {
                newPC.ReceiveStatus(bonusStatus.statusID, bonusStatus.argument, bonusStatus.duration);
            }
            //Debug.Log("newPC speed: " + newPC.CurrentSpeed());
        }

        for(int i = 0; i < enemyList.Count; i++)
        {
            EnemyMonster newEnemy = Instantiate(enemyCharacterPrefab, enemyMonsterParent.transform);
            newEnemy.transform.localPosition += Vector3.down * i;
            entityList.Add(newEnemy);

            CombatUI.CreateNewBossDetail(newEnemy);
            //Debug.Log("newEnemy speed: " + newEnemy.CurrentSpeed());
        }

        foreach (EntityBase entity in entityList)
        {
            entity.AV = ActionGauge / entity.CurrentSpeed();
            //Debug.Log("Entity : " + entity.getEntityName() + " AV: " + entity.AV);
        }

        UpdateAVDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(attackTimer.ToString());
        if(attackTimer < turnLength)
        {
            attackTimer += Time.deltaTime;
        }
        else if(dialogQueue.Count > 0)
        {
            attackTimer = 0f;

            CombatUI.DisplayNewLine(dialogQueue[0]);
            dialogQueue.RemoveAt(0);

            if(dialogQueue.Count == 0 && !endTurnPause)
            {
                foreach(EntityBase entity in entityList)
                {
                    CombatUI.UpdateHealthbar(entity);
                }
                attackTimer -= 0.5f * turnLength;
                endTurnPause = true;
            }
        }
        else if(dialogQueue.Count == 0)
        {
            //isResolvingTurn = true;
            endTurnPause = false;
            attackTimer = 0f;

            CombatUI.ClearLines();
            ProcessAV();

            CombatUI.DisplayNewLine(dialogQueue[0]);
            dialogQueue.RemoveAt(0);
        }
    }

    void ProcessAV()
    {
        //Finds the smallest AV, all entities' AV will be reduced by this smallest amount
        int minAV = ActionGauge + 1;

        EntityBase enemyTarget = null;
        int highestEnemyAggro = -1;
        EntityBase playerTarget = null;
        int highestPlayerAggro = -1;

        foreach (EntityBase entity in entityList)
        {
            if (entity.AV < minAV)
            {
                minAV = entity.AV;
            }
            // each AV calculation, finds the entity with the highest aggro on each side,
            // this entity will be the target for entities taking their turn
            if (entity.isEnemy && entity.aggro > highestEnemyAggro)
            {
                highestEnemyAggro = entity.aggro;
                enemyTarget = entity;
            }
            if (!entity.isEnemy && entity.aggro > highestPlayerAggro)
            {
                highestPlayerAggro = entity.aggro;
                playerTarget = entity;
            }
        }

        //for each entity in the list, reduce their AV by minAV. Each entity with 0 AV takes their turn and resets their AV
        foreach (EntityBase entity in entityList)
        {
            entity.AV -= minAV;
            if (entity.AV <= 0)
            {
                //Debug.Log(CombatUI.dialogBox.ToString());

                if (entity.isEnemy){
                    //CombatUI.GetDialogBox().SetText(entity.getEntityName() + " is attacking " + playerTarget.getEntityName());
                    AddNewLine(entity.getEntityName() + " is attacking " + playerTarget.getEntityName());
                    entity.TakeTurn(playerTarget);
                }
                else if (!entity.isEnemy)
                {
                    //CombatUI.GetDialogBox().SetText(entity.getEntityName() + " is attacking " + enemyTarget.getEntityName());
                    AddNewLine(entity.getEntityName() + " is attacking " + enemyTarget.getEntityName());
                    entity.TakeTurn(enemyTarget);
                }
                entity.CooldownTick();
                entity.AV = ActionGauge / entity.CurrentSpeed();
            }
        }
        CheckForDeaths();
        UpdateAVDisplay();
    }

    public void AddNewLine(string line)
    {
        dialogQueue.Add(line);
    }

    void CheckForDeaths()
    {
        for (int i = entityList.Count - 1; i >= 0; i--)
        {
            if (entityList[i].getCurrentHPNumber() <= 0)
            {
                int characterIndex = entityList[i].transform.GetSiblingIndex();
                GameObject characterSprite = playerCharacterParent.transform.GetChild(characterIndex).gameObject;
                characterSprite.SetActive(false);
                CombatUI.ProcessDeath_UI(entityList[i]);
                entityList.RemoveAt(i);
            }
        }

        bool isPartyDead = true;
        bool isEnemiesDead = true;
        foreach(EntityBase entity in entityList)
        {
            if (entity.isEnemy)
            {
                isEnemiesDead = false;
            }
            if (!entity.isEnemy)
            {
                isPartyDead = false;
            }
            if(!isPartyDead && !isEnemiesDead)
            {
                break;
            }
        }
        if (isPartyDead)
        {
            didPartyWin = false;
            MoveToLootScreen();
        }
        if (isEnemiesDead)
        {
            didPartyWin = true;
            MoveToLootScreen();
        }
    }

    void MoveToLootScreen()
    {
        SceneManager.LoadScene("Loot Screen");
    }

    void UpdateAVDisplay()
    {
        string AVDisplayText = "";

        //v1
        //List<EntityBase> entitiesSortedByAV = entityList;
        //entitiesSortedByAV.Sort((x, y) => x.AV.CompareTo(y.AV));

        //foreach (EntityBase entity in entityList)
        //{
        //    AVDisplayText += entity.getEntityName() + " - "+ entity.AV.ToString() + "\n";
        //}

        //v2
        List<ActionBarEntry> actionBar = new List<ActionBarEntry>();
        foreach(EntityBase entity in entityList)
        {
            int currentAV = 0;
            while (currentAV < actionBarMaxAV)
            {
                actionBar.Add(new ActionBarEntry(entity.getEntityName(), entity.AV + currentAV));
                currentAV += ActionGauge / entity.CurrentSpeed();
            }
        }
        actionBar.Sort((x, y) => x.AV.CompareTo(y.AV));

        for(int i = 0; i < actionBarDisplayCount; i++)
        {
            AVDisplayText += actionBar[i].entityName + " - "+ actionBar[i].AV.ToString() + "\n";
        }

        //end
        CombatUI.GetAVTracker().SetText(AVDisplayText);

    }


    public class ActionBarEntry
    {
        public string entityName;
        public int AV;

        public ActionBarEntry(string entityName, int AV)
        {
            this.entityName = entityName;
            this.AV = AV;
        }
    }
}
