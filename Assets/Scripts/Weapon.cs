using UnityEngine;

public class Weapon : MonoBehaviour {
    //required sprites
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;

    public GameObject Bullet;
    //damage States
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    private float coolDown = 0.5f;
    private float lastShoot;
    protected Transform bulletSpawnPoint;
    protected GunSpriteChanger gunSpriteChanger;

    protected virtual void Start() {
        gunSpriteChanger = transform.parent.GetComponent<GunSpriteChanger>();
        bulletSpawnPoint = gunSpriteChanger.bulletSpawnPoint;
    }

    protected virtual void Shoot() {
        Debug.Log("Shoot");
    }

    protected virtual void ChangeSprites() {
        gunSpriteChanger.GunSide.sprite = GunSide;
        gunSpriteChanger.GunUp.sprite = GunUp;
        gunSpriteChanger.GunDown.sprite = GunDown;
        gunSpriteChanger.GunDiagUp.sprite = GunDiagUp;
        gunSpriteChanger.GunDiagDown.sprite = GunDiagDown;
    }

    protected virtual void Update() {
        if (Input.GetMouseButton(0)) {
            if(Time.time - lastShoot > coolDown) {
                lastShoot = Time.time;
                Shoot();
            }
        }
    }

}
