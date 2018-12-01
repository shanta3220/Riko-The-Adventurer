using UnityEngine;

public class Weapon : MonoBehaviour {

    public Sprite bullet;
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;
    protected Transform bulletSpawnPoint;

    protected GunSpriteChanger gunSpriteChanger;
    protected virtual void Start() {
        gunSpriteChanger = transform.parent.GetComponent<GunSpriteChanger>();
        bulletSpawnPoint = gunSpriteChanger.bulletSpawnPoint;
    }

    protected virtual void Shoot() {

    }

    protected virtual void ChangeSprites() {
        gunSpriteChanger.GunSide.sprite = GunSide;
        gunSpriteChanger.GunUp.sprite = GunUp;
        gunSpriteChanger.GunDown.sprite = GunDown;
        gunSpriteChanger.GunDiagUp.sprite = GunDiagUp;
        gunSpriteChanger.GunDiagDown.sprite = GunDiagDown;
    }

}
