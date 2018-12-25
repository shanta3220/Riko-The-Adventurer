using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangedType {
    horizontal, Vertical, Stationary
};
public enum BulletType {
    Stationary, Slow
};

public class EnemyRanged : Mover {

    
    public float enemyTriggerLength = 4f;
    public float enemyAttackingLength = 5;
    public RangedType rangeType;
    public bool playerInRange;
    //delays to control how frequent enemy moves or attacks
    public Vector2 delayAttack = new Vector2(2,5); //x=mindelay, y= maxdelay
    public Vector2 delayChangePosition = new Vector2(1, 4); //x=mindelay, y= maxdelay
    public GameObject enemyProjectTile;
    public BulletType bulletType;

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
    private float enemyProjectileSpeed = 2;

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
    }

    private void FixedUpdate() {

        UpdateMotor(Move());
        if (Vector3.Distance(playerTransform.position, startingPosition) < enemyAttackingLength) {
            if (Vector3.Distance(playerTransform.position, startingPosition) < enemyTriggerLength) {
                playerInRange = true;
            }
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
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        
        
        if(bulletType == BulletType.Slow) {
            for(int i = 0; i<5; i++) {
                float randomVal = i * Random.Range(i,5);
                Vector3 offset = new Vector3(randomVal*Time.deltaTime, randomVal * Time.deltaTime, 0);
                GameObject projectile = Instantiate(enemyProjectTile, spawnPoint.position + offset, rotation);
                projectile.GetComponent<Rigidbody2D>().velocity = direction * enemyProjectileSpeed / 2;
                projectile.GetComponent<EnemyProjectile>().isSlowType = true;
            }
        }
      
        else {
            GameObject projectile = Instantiate(enemyProjectTile, spawnPoint.position, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * enemyProjectileSpeed;
        }
    }
}
