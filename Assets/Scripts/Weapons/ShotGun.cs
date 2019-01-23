using UnityEngine;

public class ShotGun : Weapon {

    public float bulletSpeed = 2;
    public int numberofProjectiles = 2;
    protected override void Awake() {
        base.Awake();
        Presets();
        
    }


    public override void Shoot() {
        base.Shoot();
        if (GameManager.instance.player.numOfActiveBullets == 10) {
            GameManager.instance.player.numOfActiveBullets -= 1;
            return;
        }
           
        GameManager.instance.player.numOfActiveBullets += 2;
        Vector2 direction;
        Vector2 myPos = new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y);
        if (isOnPc) {
            direction = PCShootDirection(myPos);
        }
        else {
            direction = MobileShootDirection(myPos);
        }
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        /*GameObject emptyShell = Instantiate(emptyShells, myPos, rotation);
        emptyShell.transform.parent = emptyShellsContainer.transform;
        emptyShell.GetComponent<Rigidbody2D>().velocity = -direction * 1f;*/
        direction.Normalize();
        float offset = 0.25f;
        for (int i = 0; i <= numberofProjectiles; i++) {
            direction.x += Random.Range(-offset, offset);
            direction.y += Random.Range(-offset, offset);
            rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(Bullet, myPos, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            projectile.GetComponent<Bullet>().numberofbullets = (numberofProjectiles + 1);
  
        }
    }

    private void Presets() {
        string folderName = "Weapons/shotgun/";
        GunSide = Resources.Load<Sprite>(folderName + "side");
        GunUp = Resources.Load<Sprite>(folderName + "up");
        GunDown = Resources.Load<Sprite>(folderName + "down");
        GunDiagUp = Resources.Load<Sprite>(folderName + "diagup");
        GunDiagDown = Resources.Load<Sprite>(folderName + "diagdown");
        Bullet = Resources.Load<GameObject>("white_bullet");
    }

}
