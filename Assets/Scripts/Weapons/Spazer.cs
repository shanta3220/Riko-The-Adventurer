using UnityEngine;

public class Spazer : Weapon {

    public float bulletSpeed = 5;
    string folderName = "Weapons/spazer/";

    protected override void Awake() {
        base.Awake();
        GunSide = Resources.Load<Sprite>(folderName + "side");
        GunUp = Resources.Load<Sprite>(folderName + "up");
        GunDown = Resources.Load<Sprite>(folderName + "down");
        GunDiagUp = Resources.Load<Sprite>(folderName + "diagup");
        GunDiagDown = Resources.Load<Sprite>(folderName + "diagdown");
        Bullet = Resources.Load<GameObject>("canon_bullet");
    }

    private void OnEnable() {
        //ChangeSprites();

    }
    public override void Shoot() {
        Quaternion bulletAngle = Quaternion.Euler(rotateAngle * Vector3.forward);
        Instantiate(Bullet, bulletSpawnPoint.position, bulletAngle);
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
    }
}
