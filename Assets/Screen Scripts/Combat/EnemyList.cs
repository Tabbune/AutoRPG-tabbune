using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TitleScreen;

public class EnemyList : MonoBehaviour
{
    public EnemyThumbnail enemyThumbnailPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(string enemy in enemyDataDict.Keys) {
            //Debug.Log("Loading enemy: " + enemy);
            EnemyThumbnail newEnemy = Instantiate(enemyThumbnailPrefab, this.transform);
            newEnemy.LoadEnemy(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
