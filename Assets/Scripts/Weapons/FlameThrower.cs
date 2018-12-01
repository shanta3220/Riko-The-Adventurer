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
        Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        //Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
        Bullet.GetComponent<Bullet>().SetSpawnPoint(bulletSpawnPoint);
        Bullet.GetComponent<Bullet>().canMove = true;
        Bullet.transform.rotation = Quaternion.Euler(rotateAngle * Vector3.forward);
        Debug.Log("New One");

    }

    private void Rotation() {

    }
}
