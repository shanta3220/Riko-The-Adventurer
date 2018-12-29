using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matter : Weapon {
    public float bulletSpeed = 2;
    
    protected override void Awake() {
        base.Awake();
        Presets();
    }

    public override void Shoot() {
        if (GameManager.instance.player.target == null)
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPos = new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y);
        Vector2 direction = target - myPos;
        direction.Normalize();
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject projectile = Instantiate(Bullet, myPos, rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        GameObject emptyShell = Instantiate(emptyShells, myPos, rotation);
        emptyShell.transform.parent = emptyShellsContainer.transform;
        emptyShell.GetComponent<Rigidbody2D>().velocity = -direction * 1f;
    }

    private void Presets() {
        string folderName = "Weapons/matter/";
        GunSide = Resources.Load<Sprite>(folderName + "side");
        GunUp = Resources.Load<Sprite>(folderName + "up");
        GunDown = Resources.Load<Sprite>(folderName + "down");
        GunDiagUp = Resources.Load<Sprite>(folderName + "diagup");
        GunDiagDown = Resources.Load<Sprite>(folderName + "diagdown");
        Bullet = Resources.Load<GameObject>("green_bullet");
    }
}
