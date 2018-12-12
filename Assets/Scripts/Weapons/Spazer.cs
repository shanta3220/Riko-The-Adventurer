using UnityEngine;

public class Spazer : Weapon {

    public float bulletSpeed = 5;

    protected override void Awake() {
        base.Awake();

    }

    private void OnEnable() {
        ChangeSprites();

    }
    public override void Shoot() {
        Quaternion bulletAngle = Quaternion.Euler(rotateAngle * Vector3.forward);
        Instantiate(Bullet, bulletSpawnPoint.position, bulletAngle);
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
    }
}
