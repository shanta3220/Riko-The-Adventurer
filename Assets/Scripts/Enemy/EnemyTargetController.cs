using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTargetType{
    enableEnemyTarget, disableEnemyTarget
};

public class EnemyTargetController : Collidable {

    public EnemyTargetType enemyTargetType;
    protected override void OnCollide(Collider2D col) {
        if(col.tag == "Player") {
            if(EnemyTargetType.enableEnemyTarget == enemyTargetType) {
                GameManager.instance.enemyBatchHandler.EnableEnemyTarget();
            }
            else {
                GameManager.instance.enemyBatchHandler.DisableEnemyTarget();
            }
        }
    }
}
