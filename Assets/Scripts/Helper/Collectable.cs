using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : Collidable {
    //protected means private but your children class can have access over it
    protected bool collected;

    protected override void OnCollide(Collider2D col) {
        if(col.tag == "Player") {
            OnCollect();
        }
    }

    protected virtual void OnCollect() {
        collected = true;
    }
}
