using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable {

    public int healingAmount = 1;
    private float healCoolDown = 1.0f;
    private float lastHeal;

    protected override void OnCollide(Collider2D col) {
        if(col.tag == "Player") {
            if (Time.time - lastHeal > healCoolDown) {
                lastHeal = Time.time;
                GameManager.instance.player.Heal(healingAmount);

            }
        }
       
    }
}
