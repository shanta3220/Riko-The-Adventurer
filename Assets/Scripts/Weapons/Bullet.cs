using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Collidable {
    //damageStructure
    public int damagePoint = 1;
    public float pushForce = 2.0f;
   
    //upgrade
    private Rigidbody2D rBody;
    public int weaponLevel = 0;
    public float bulletSpeed = 5;
    public bool canMove;
    public float zRot;

    private SpriteRenderer spriteRenderer;
    private Animator anim;

    protected override void Start() {
        base.Start();
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate() {
        if (canMove)
            rBody.AddForce(transform.right * bulletSpeed);
    }

    protected override void OnCollide(Collider2D col) {
        if (col.tag == "Blocking"){
            rBody.velocity = Vector2.zero;
            anim.SetInteger("Anim", 1);
            Destroy(gameObject, 0.2f);
        }
            
        
        if (col.tag == "Enemy") {
            rBody.velocity = Vector2.zero;
            //create a new damage object then we will send it to the enemy we hit
            Damage dmg = new Damage {
                damageAmount = GameManager.instance.weapon.damagePoint[GameManager.instance.weapon.weaponLevel],
                origin = transform.position,
                pushForce = GameManager.instance.weapon.pushForce[GameManager.instance.weapon.weaponLevel]
            };
            col.SendMessage("ReceiveDamage", dmg);
            anim.SetInteger("Anim", 1);
            Destroy(gameObject, 0.2f);

        }
    }

  /*  void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            Debug.Log(col.gameObject.name);
        }
    }*/

}
