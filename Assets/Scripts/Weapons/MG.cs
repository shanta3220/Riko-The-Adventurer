using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG : Weapon {
    public float bulletSpeed = 5;

    protected override void Awake() {
        base.Awake();
        Presets();
    }

    public override void Shoot() {
        Quaternion bulletAngle = Quaternion.Euler(rotateAngle * Vector3.forward);
        Instantiate(Bullet, bulletSpawnPoint.position, bulletAngle);
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
    }

    private void Presets() {
        string folderName = "Weapons/mg/";
        GunSide = Resources.Load<Sprite>(folderName + "side");
        GunUp = Resources.Load<Sprite>(folderName + "up");
        GunDown = Resources.Load<Sprite>(folderName + "down");
        GunDiagUp = Resources.Load<Sprite>(folderName + "diagup");
        GunDiagDown = Resources.Load<Sprite>(folderName + "diagdown");
        Bullet = Resources.Load<GameObject>("flamethrower_bullet");
    }
}
