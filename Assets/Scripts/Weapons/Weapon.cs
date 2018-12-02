using UnityEngine;

public class Weapon : MonoBehaviour {
    //required sprites
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;

    public GameObject Bullet;
    //damage States
    public int damagePoint = 1;
    public float pushForce = 2.0f;
    public float rotateAngle = 0;
    protected float coolDown = 0.5f;
    protected float lastShoot;
    protected Transform bulletSpawnPoint;
    protected GunSpriteChanger gunSpriteChanger;

    protected virtual void Start() {
        gunSpriteChanger = transform.parent.GetComponent<GunSpriteChanger>();
        bulletSpawnPoint = gunSpriteChanger.bulletSpawnPoint;
        //ChangeSprites();
    }

    public virtual void Shoot() {
        // Debug.Log("Shoot");
        
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

   public virtual void GetDirection(Vector3 dirFromMeToMouse) {

    }

}
