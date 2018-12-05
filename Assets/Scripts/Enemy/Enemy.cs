using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover {
    //Experience
    public int xpValue = 1;

    //logic
    public float triggerLength  = 0.3f;
    public float chaseLength = 1;
    public ContactFilter2D filter;
    private bool isChasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    //hitbox - its weapon
    private BoxCollider2D hitBox;//additional boxCollider
    private Collider2D[] hits = new Collider2D[10];

    private Vector3 motion;

    protected override void Start(){
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }
    
    private void FixedUpdate() {

        //is the player in range?
        if(Vector3.Distance(playerTransform.position,startingPosition)< chaseLength){
            if(Vector3.Distance(playerTransform.position,startingPosition)<triggerLength){
                  isChasing = true;
            }
            if(isChasing){
                if(!collidingWithPlayer){
                    motion = (playerTransform.position - transform.position).normalized;
                    UpdateMotor(motion );
                }
            }
            else{
                //going back to where we were
                motion = startingPosition - transform.position;
                UpdateMotor(motion);
            }
            
        }
        else{
            //reseting
            motion = startingPosition - transform.position;
            UpdateMotor(motion);
            isChasing = false; 
        }
        if( Vector3.Distance(transform.position,startingPosition) <= 0.05f)
            UpdateMotor(Vector3.zero);
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
        
    }

    protected override void Death(){
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+"+ xpValue+" xp",30, Color.magenta,transform.position, Vector3.up * 40, 1.0f);
    }

}
