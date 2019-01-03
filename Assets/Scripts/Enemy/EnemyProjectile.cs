using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Collidable {
    public bool isSlowType;
    public int damagePoint = 2;
    public int[] damagePoints = { 1, 2, 3, 4, 5, 6, 7, 8 };
    public float pushForce = 2.0f;
    public bool isRotatedProjectile;
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
        if (isRotatedProjectile)
            transform.Rotate(Vector3.forward * 60);
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
                damageAmount = damagePoints[GameManager.instance.GetCurrentLevel()],
                origin = transform.position,
                pushForce = pushForce
            };
            col.SendMessage("ReceiveDamage", dmg);
            anim.SetInteger("Anim", 1);
            Destroy(gameObject, 0.2f);
        }
    }
}
