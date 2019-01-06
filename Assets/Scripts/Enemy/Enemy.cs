using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover {
    //Experience
    public int xpValue = 1;

    public int goldValue = 15;
    public ContactFilter2D filter;
    private bool collidingWithPlayer;//for animating
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Collider2D[] hits = new Collider2D[10];
    private Vector3 motion;
   
    private float chaseSpeed = 0.8f; //the less the value the slower it is
    private bool startAnim = true;// just to show animespawn  anim;

    private float lastAudioPlay;
    private bool isDeath;
    protected override void Start(){
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        sr.color = Color.black;
        SetLevelHealth(GameManager.instance.playerCurrentLevel);
    }
    
    private void FixedUpdate() {
        //is the player in range?
        if (isDeath)
            return;
        if (startAnim) {
            sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 20);
            if (sr.color == Color.white)
                startAnim = false;
            return;
        }
        if (hasEnemyTarget) {
            
            if (!collidingWithPlayer){
                motion = (playerTransform.position - transform.position).normalized * chaseSpeed;
            }
            else {
                //going back to where we were
                motion = startingPosition - transform.position;
               
            }
         
        }
        else{
            //reseting
            motion = startingPosition - transform.position;
            
        }
        //overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if(hits[i] == null) {
                continue;
            }
            if(hits[i].tag == "Player") {
                collidingWithPlayer = true;
            }
            hits[i] = null;
        }
        if (collidingWithPlayer)
            UpdateMotor(Vector3.zero);
        else UpdateMotor(motion);
    }

    protected override void ReceiveDamage(Damage dmg) {
        if (isDeath)
            return;
        base.ReceiveDamage(dmg);
        Hurt();
    }

    protected override void Death(){
        isDeath = true;
        anim.SetTrigger("EnemyDeath");
        AudioController.instance.PlaySound(SoundClip.enemyDeath);
        transform.parent.GetComponent<EnemyBatchHandler>().RemoveEnemy(transform);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+"+ xpValue+" xp",30, Color.magenta,transform.position, Vector3.up * 40, 1.0f);
        GameManager.instance.gold += goldValue;
        GameManager.instance.ShowText("+" + goldValue + " gold!", 23, Color.yellow, transform.position, Vector3.up * 10, 1.5f);
        Destroy(gameObject, 0.7f);
        
    }

    private void Hurt() {
        if(sr != null)
            sr.color = Color.magenta;
        Invoke("RestoreColor", 0.5f);
        if (audioS.isActiveAndEnabled && Time.time - lastAudioPlay > 0.5f) {
            lastAudioPlay = Time.time;
            audioS.Play();
        }
           
    }

    private void RestoreColor() {
        sr.color = Color.white;
    }

    /*
            //logic
        public float triggerLength  = 0.3f;
        public float chaseLength = 1;
        private bool isChasing;
        private void OldMovement() {
            float distance = Vector3.Distance(transform.position, startingPosition);
            if (distance <chaseLength) {
                if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength) {
                    isChasing = true;
                }
                if (isChasing) {
                    if (!collidingWithPlayer) {
                        motion = (playerTransform.position - transform.position).normalized;
                        UpdateMotor(motion);
                    }
                }
                else {
                    //going back to where we were
                    motion = startingPosition - transform.position;
                    UpdateMotor(motion);
                }

            }
            else {
                //reseting
                motion = startingPosition - transform.position;
                UpdateMotor(motion);
                isChasing = false;
            }
        }*/

}
