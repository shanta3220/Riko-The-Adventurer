using UnityEngine;

public class FlameThrower : Weapon {

    public float bulletSpeed = 2;

    protected override void Awake() {
        base.Awake();
        Presets();
    }


    public override void Shoot() {
        Vector2 direction;
        Vector2 myPos = new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y);
        if (isOnPc) {
            direction = PCShootDirection(myPos);
        }
        else {
            direction = MobileShootDirection(myPos);
        }
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject projectile = Instantiate(Bullet, myPos, rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        //GameObject emptyShell = Instantiate(emptyShells, myPos, rotation);
        //emptyShell.transform.parent = emptyShellsContainer.transform;
        //emptyShell.GetComponent<Rigidbody2D>().velocity = -direction * 1f;
    }

    private void Presets() {
        string folderName = "Weapons/flamthrower/";
        GunSide = Resources.Load<Sprite>(folderName + "side");
        GunUp = Resources.Load<Sprite>(folderName + "up");
        GunDown = Resources.Load<Sprite>(folderName + "down");
        GunDiagUp = Resources.Load<Sprite>(folderName + "diagup");
        GunDiagDown = Resources.Load<Sprite>(folderName + "diagdown");
        Bullet = Resources.Load<GameObject>("rocket_bullet");
    }


    /*
    private void OLD() {
        Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 myPos = new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y);
        Vector2 direction = target - myPos;
        direction.Normalize();
        Quaternion bulletAngle = Quaternion.Euler(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * Vector3.forward);
        Instantiate(Bullet, bulletSpawnPoint.position, bulletAngle);
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoint.up * bulletSpeed);
}*/
}
