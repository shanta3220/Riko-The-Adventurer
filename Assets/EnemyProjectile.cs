using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Collidable {
    public bool isSlowType;
    public int damagePoint = 2;
    public float pushForce = 2.0f;
    //references
    private Rigidbody2D rBody;
    private Animator anim;
    protected override void Start() {
        base.Start();
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate() {
        if (isSlowType)
            rBody.velocity = Vector2.Lerp (rBody.velocity, Vector2.zero, Random.value* Time.deltaTime);
    }

    protected override void OnCollide(Collider2D col) {
        if (col.tag == "Blocking") {
            rBody.velocity = Vector2.zero;
            anim.SetInteger("Anim", 1);
            Destroy(gameObject, 0.2f);
        }

        if (col.tag == "Player") {
            rBody.velocity = Vector2.zero;
            //create a new damage object then we will send it to the enemy we hit
            Damage dmg = new Damage {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };
            col.SendMessage("ReceiveDamage", dmg);
            anim.SetInteger("Anim", 1);
            Destroy(gameObject, 0.2f);
        }
    }
}
