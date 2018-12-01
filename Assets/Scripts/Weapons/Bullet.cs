using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Collidable {
    //damageStructure
    public int damagePoint = 1;
    public float pushForce = 2.0f;
    public float zRot;
    //upgrade
    private Rigidbody rBody;
    public int weaponLevel = 0;
    public float bulletSpeed = 1;
    public bool canMove;
    public Transform bulletSpawnPoint;
    private SpriteRenderer spriteRenderer;

    public Vector2 direction;
    protected override void Start() {
        base.Start();
    }

    
    private void FixedUpdate() {
        if(canMove)
            rBody.AddForce(bulletSpawnPoint.up * bulletSpeed*Time.deltaTime);
    }

    public void SetSpawnPoint(Transform spawnPoint) {
        bulletSpawnPoint = spawnPoint;
    }

}
