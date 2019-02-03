using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is responsible to send new targets to the player 
/// and activates next enemy batch when this batch is cleared by the player
/// </summary>
public class EnemyBatchHandler : MonoBehaviour {

    public bool openBarrier;
    public int barrierID;
    public List<Transform> enemies;
    public GameObject nextEnemyBatch;
    private Player player;
    private Transform playerTransform;
    private Transform currentTarget;
    public bool playWithTarget;
    [Header("Fill up if enemytargetcontrollers need to be destroyed")]
    public GameObject enemyTargetEnable;
    public GameObject enemyTargetDisable;
    private bool isEnemeyTargetEnabled;
    public bool isSecondBatchOfSameArea;

    void Start () {
        player = GameManager.instance.player;
        playerTransform = player.transform;
        foreach(Transform trans in GetComponentInChildren<Transform>()) {
            if (trans != this)
                enemies.Add(trans);
        }
        GameManager.instance.enemyBatchHandler = this;
        currentTarget = enemies[0];
        if (isSecondBatchOfSameArea)
            EnableEnemyTarget();
    }
	
	private void Update () {
        if (GameManager.instance.isPaused)
            return;

        if (!isEnemeyTargetEnabled){
            if(player.target != null)
                player.target = null;
            return;
        }
        if (enemies.Count == 0) {
            if(enemyTargetEnable != null) {
                Destroy(enemyTargetEnable);
                Destroy(enemyTargetDisable);
                player.target = null;
            }
            if (nextEnemyBatch != null) {
                nextEnemyBatch.SetActive(true);
                if (openBarrier) {
                    GameManager.instance.OpenBarrier(barrierID);
                }
                   
            }
            else {
                GameManager.instance.LevelComplete();
                GameManager.instance.OpenDoor();
            }
            Destroy(gameObject,1f);
        }
        for (int i = 0; i < enemies.Count; i++) {
            if(enemies[i]== null) {
                RemoveEnemy(enemies[i]);
                continue;
            }

            if (currentTarget == null) {
                i = 0;
                currentTarget = enemies[i];
            }
            if (Vector3.Distance(currentTarget.transform.position, playerTransform.position) > Vector3.Distance(enemies[i].position, playerTransform.position)) {
                currentTarget = enemies[i];
            }

            player.target = currentTarget;
        }

    }

    public void RemoveEnemy(Transform enemy) {
        enemies.Remove(enemy);
    }

    public void EnableEnemyTarget() {
        if (isEnemeyTargetEnabled)
            return;
        foreach(Transform enemy in enemies) {
            enemy.GetComponent<Mover>().hasEnemyTarget = true;
        }
        isEnemeyTargetEnabled = true;
    }

    public void DisableEnemyTarget() {
        if (!isEnemeyTargetEnabled)
            return;
        foreach (Transform enemy in enemies) {
            enemy.GetComponent<Mover>().hasEnemyTarget = false;
        }
        isEnemeyTargetEnabled = false;

    }

}
