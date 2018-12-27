using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

    //public fields
    public int health = 10;
    public int maxHealth =  10;
    public float PushRecoverySpeed = 0.2f;

    //immunity -so that we can spams hits

    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //push
    protected Vector3 pushDirection;

    //all fighters can receive damage/die

    protected virtual void ReceiveDamage(Damage dmg) {
        if(Time.time - lastImmune > immuneTime) {
            lastImmune = Time.time;
            health -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 26, Color.red, transform.position, Vector3.up * 25, 0.5f);
            if(health <= 0) {
                health = 0;
                Death();
            }
        }
    }

    protected virtual void Death() { }
	
}
