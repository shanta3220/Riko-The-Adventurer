using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    //required sprites
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;
    public int weaponID;
    public bool isFromDataController;//to add the component
    public List<int> weaponPrinces = new List<int>(){35,70,135,200,275,400};
    public int weaponLevel = 1;
    public GameObject Bullet;
    //damage States
    public int[] damagePoint = {1,2,3,4,5,6,7};
    public float[] pushForce = {2.0f,2.2f,2.6f,2.8f,3.0f,3.2f,3.4f};
    public float rotateAngle = 0;
    protected float coolDown = 0.5f;
    protected float lastShoot;
    protected Transform bulletSpawnPoint;
    protected GunSpriteChanger gunSpriteChanger;
    protected int damageStartValue = 1;

    protected virtual void Awake() {
        if (isFromDataController)
            return;
        gunSpriteChanger = transform.parent.GetComponent<GunSpriteChanger>();
        bulletSpawnPoint = gunSpriteChanger.bulletSpawnPoint;
     
        //ChangeSprites();
    }

    protected virtual void Start() {
        /*if (!GameManager.instance.weaponSprites.Contains(GunSide))
            GameManager.instance.weaponSprites.Add(GunSide); */
        GameManager.instance.weaponSprite = GunSide;

    }

    public void ChangeSprites() {
        if (isFromDataController)
            return;
        if (DataController.instance.data.weaponSelected != weaponID)
            return;
        gunSpriteChanger.GunSide.sprite = GunSide;
        gunSpriteChanger.GunUp.sprite = GunUp;
        gunSpriteChanger.GunDown.sprite = GunDown;
        gunSpriteChanger.GunDiagUp.sprite = GunDiagUp;
        gunSpriteChanger.GunDiagDown.sprite = GunDiagDown;
        Debug.Log(DataController.instance.data.weaponSelected);
        if (transform.parent.GetComponent<Player>().weapon != null && transform.parent.GetComponent<Player>().weapon != this)
            transform.parent.GetComponent<Player>().weapon.enabled = false;
        transform.parent.GetComponent<Player>().weapon = this;
        if (GameManager.instance != null)
            GameManager.instance.weapon = this;

    }

    protected virtual void Update() {
        /*if(Input.GetMouseButton(0)) {
            if(Time.time - lastShoot > coolDown) {
                lastShoot = Time.time;
                Shoot();
            }
        }*/
        
    }

    public virtual void Shoot() { }

    public virtual void GetDirection(Vector3 dirFromMeToMouse) { }

    private void OnCollisionEnter2D(Collision2D other) { }


    public virtual void UpgradeWeapon() {
        weaponLevel++;
        DataController.instance.data.collectedWeapons[weaponID].weaponLevel = weaponLevel;
    }

    public void SetSettings(Weapon newWep) {
 
        newWep.GunSide = GunSide;
        newWep.GunUp = GunUp;
        newWep.GunDown= GunDown;
        newWep.GunDiagUp = GunDiagUp;
        newWep.GunDiagDown = GunDiagDown;
        newWep.Bullet = Bullet;
        newWep.weaponID = weaponID;
        newWep.weaponLevel = weaponLevel;


    }
}
