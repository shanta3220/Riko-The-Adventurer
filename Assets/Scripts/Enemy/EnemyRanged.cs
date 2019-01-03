﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangedType {
    horizontal, Vertical, Stationary
};

public class EnemyRanged : Mover {
    public float enemyTriggerLength = 4f;
    public float enemyAttackingLength = 5;
    
    public int xpValue = 1;
    public int goldValue = 20;
    public RangedType rangeType;
    public bool playerInRange;
    //delays to control how frequent enemy moves or attacks
    public Vector2 delayAttack = new Vector2(2,5); //x=mindelay, y= maxdelay
    public Vector2 delayChangePosition = new Vector2(1, 4); //x=mindelay, y= maxdelay
    public AudioClip enemyHurt;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private bool changedPosition;

    /// delay to change position
    private float delayForChangingPosition;
    private float lastChangedPosition;

    /// delay to change position
    private float delayForAttack;
    private float lastAttacked;
    //depending on the RangedType we use the variables
    private float minPos, maxPos;
    private Vector3 pos1, pos2;

    private Transform spawnPoint;
    private EnemyShoot enemyShoot;

    private float lastAudioPlay;

    protected override void Start() {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        delayForChangingPosition = Random.Range(delayChangePosition.x, delayChangePosition.y);
   
        if (rangeType == RangedType.horizontal) {
            minPos = transform.GetChild(0).transform.localPosition.x;
            maxPos = transform.GetChild(1).transform.localPosition.x;
        }

        else if (rangeType == RangedType.Vertical) {
            minPos = transform.GetChild(0).transform.localPosition.y;
            maxPos = transform.GetChild(1).transform.localPosition.y;
        }

        else {
            pos1 = transform.GetChild(0).transform.position;
            pos2 = transform.GetChild(1).transform.position;
            targetPosition = pos2;
        }
        spawnPoint = transform.GetChild(2).transform;
        enemyShoot = GetComponent<EnemyShoot>();
        SetLevelHealth(GameManager.instance.playerCurrentLevel);
    }

    private void FixedUpdate() {

        UpdateMotor(Move());
        if (hasEnemyTarget) {
            playerInRange = true;
        }
        else {
            playerInRange = false;
        }
    }

    private Vector3 Move() {
        if (rangeType == RangedType.horizontal) {
            if (!changedPosition) {
                float xPos = Random.Range(minPos, maxPos);
                targetPosition = startingPosition + Vector3.right * xPos;
                changedPosition = true;
            }
        }
        else if (rangeType == RangedType.Vertical) {
            if (!changedPosition) {
                float xPos = Random.Range(minPos, maxPos);
                targetPosition = startingPosition + Vector3.up * xPos;
                changedPosition = true;
            }
        }
        else {
            if (!changedPosition) {
                targetPosition = pos1 == targetPosition ? pos2 : pos1;
                changedPosition = true;
            }
        }
       
        if (transform.position == targetPosition) {
            if (Time.time - lastChangedPosition >= delayForChangingPosition) {
                lastChangedPosition = Time.time;
                changedPosition = false;
                delayForChangingPosition = Random.Range(delayChangePosition.x, delayChangePosition.y);
            }
        }
        return targetPosition;
    }

    protected override void UpdateMotor(Vector3 input) {
        transform.position = Vector3.MoveTowards(transform.position, input, Time.deltaTime * xSpeed);
        if (playerInRange) {
            Vector3 motion = (playerTransform.position - transform.position).normalized;
            Animate(motion.x);
            if (Time.time - lastAttacked >= delayForAttack) {
                lastAttacked = Time.time;
                anim.SetTrigger("Attack");
                Shoot();
                delayForAttack = Random.Range(delayAttack.x, delayAttack.y);
            }
        }
    }

    protected override void Animate(float x) {
        //swap sprite direction whether moving left or right and animation
        if (x > 0) {
            transform.localScale = Vector3.one * localScaleSize;
        }

        else if (x < 0) {
            transform.localScale = new Vector3(-localScaleSize, localScaleSize, localScaleSize);
        }
        moveDelta += pushDirection;
        //reduce pushForce everyframe, based on recoveryspeed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, PushRecoverySpeed);
      
    }

    void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.GetChild(0).transform.position, 0.02f);
        Gizmos.DrawSphere(transform.GetChild(1).transform.position, 0.02f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.GetChild(0).transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.GetChild(1).transform.position);
    }

    public void Shoot() {
        Vector2 direction = (playerTransform.position - spawnPoint.position).normalized;
        enemyShoot.Shoot(direction, spawnPoint.position);
    }

    protected override void ReceiveDamage(Damage dmg) {
        base.ReceiveDamage(dmg);
        Hurt();
    }

    protected override void Death() {
        AudioController.instance.PlaySound(SoundClip.enemyDeath);
        transform.parent.GetComponent<EnemyBatchHandler>().RemoveEnemy(transform);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
        GameManager.instance.gold += goldValue;
        GameManager.instance.ShowText("+" + goldValue + " gold!", 23, Color.yellow, transform.position, Vector3.up * 10, 1.5f);
        Destroy(gameObject);
    }

    private void Hurt() {
        sr.color = Color.magenta;
        Invoke("RestoreColor", 0.2f);
        if (audioS.isActiveAndEnabled && Time.time - lastAudioPlay > 0.5f) {
            lastAudioPlay = Time.time;
            audioS.Play();
        }
    }

    private void RestoreColor() {
        sr.color = Color.white;
    }

    private void PlayAudio() {
       
    }

    public void OnLevelUp() {
       
    }
    /*
        private void OldFixedUpdate() {

        UpdateMotor(Move());
        if (Vector3.Distance(playerTransform.position, startingPosition) < enemyAttackingLength) {
            if (Vector3.Distance(playerTransform.position, startingPosition) < enemyTriggerLength) {
                playerInRange = true;
            }
        }
        else {
            playerInRange = false;
        }
    */
}