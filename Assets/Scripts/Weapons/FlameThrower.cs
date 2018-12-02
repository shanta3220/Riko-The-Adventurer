using UnityEngine;

public class FlameThrower : Weapon {

    public float bulletSpeed = 5;
 

	protected override void Start () {
        base.Start();
    }
    protected override void Update() {
        /*if (Input.GetMouseButton(0)) {
            if (Time.time - lastShoot > coolDown) {
                lastShoot = Time.time;
                Shoot();
            }
        }*/
    }


    public override void Shoot() {
        Quaternion bulletAngle = Quaternion.Euler(rotateAngle * Vector3.forward);
        Instantiate(Bullet, bulletSpawnPoint.position, bulletAngle);
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
       
       // Bullet.GetComponent<Bullet>().canMove = true;

    }

    private void Rotation() {

    }
}
