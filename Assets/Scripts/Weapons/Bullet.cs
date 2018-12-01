using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Collidable {
    //damageStructure
    public int damagePoint = 1;
    public float pushForce = 2.0f;
    public float zRot;
    //upgrade

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update() {
        base.Update();

    }

}
