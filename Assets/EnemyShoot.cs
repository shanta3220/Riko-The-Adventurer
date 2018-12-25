using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BulletType {
    Normal, Spread, SlowSpread
};

public class EnemyShoot : MonoBehaviour {

    public float numberofProjectiles = 0;
    public GameObject enemyProjectTile;
    public BulletType bulletType;
    public float enemyProjectileSpeed = 2;

    public void Shoot(Vector3 direction, Vector3 origin) {
        if(bulletType== BulletType.Spread) {
            ShootSpreadProjectiles(direction, origin, false);
        }
        else if(bulletType == BulletType.SlowSpread) {
            ShootSpreadProjectiles(direction, origin);
        }
        else if(bulletType == BulletType.Normal) {
            NormalShoot(direction, origin);
        }
    }

    public void ShootSpreadProjectiles (Vector3 direction, Vector3 origin, bool isProjectileSlowType = true) {
        float offset = 0.5f;
        for (int i = 0; i <= numberofProjectiles; i++) {
            direction.x += Random.Range(-offset, offset);
            direction.y += Random.Range(-offset, offset);
            //Vector3 offset = new Vector3(randomVal*Time.deltaTime, randomVal * Time.deltaTime, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(enemyProjectTile, origin, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * enemyProjectileSpeed / 5;
            projectile.GetComponent<Rigidbody2D>().isKinematic = true;
            projectile.GetComponent<EnemyProjectile>().isSlowType = isProjectileSlowType;
        }
    }

    public void NormalShoot(Vector3 direction, Vector3 origin) {
        for (int i = 0; i <= numberofProjectiles; i++) {
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(enemyProjectTile, origin, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * enemyProjectileSpeed;
        }
    }
}
