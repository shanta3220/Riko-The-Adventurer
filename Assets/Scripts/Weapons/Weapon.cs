using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    //required sprites
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;
    public int weaponID;
    public List<int> weaponPrinces = new List<int>(){35,70,135,200,275,400};
    public int weaponLevel = 1;
    public GameObject Bullet;
    public GameObject emptyShells;
    public GameObject emptyShellsContainer;
    //damage States
    public int[] damagePoint = {1,2,3,4,5,6,7};
    public float[] pushForce = {2.0f,2.2f,2.6f,2.8f,3.0f,3.2f,3.4f};
    public float rotateAngle = 0;
    public Transform bulletSpawnPoint;
    public GunSpriteChanger gunSpriteChanger;
    public AudioSource audioS;
    public Vector2 target;
    protected int damageStartValue = 1;
    protected bool isOnPc;
    protected Joystick movementJoystick;
    protected virtual void Awake() {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            isOnPc = true;
        else if (Application.platform == RuntimePlatform.Android)
            isOnPc = false;
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    public void ChangeSprites() {
        if (GameManager.instance.data.weaponSelected != weaponID)
            return;
        audioS = GetComponent<AudioSource>();
        gunSpriteChanger.GunSide.sprite = GunSide;
        gunSpriteChanger.GunUp.sprite = GunUp;
        gunSpriteChanger.GunDown.sprite = GunDown;
        gunSpriteChanger.GunDiagUp.sprite = GunDiagUp;
        gunSpriteChanger.GunDiagDown.sprite = GunDiagDown;
        if (gunSpriteChanger.GetComponent<Player>().weapon != null && gunSpriteChanger.GetComponent<Player>().weapon != this)
            gunSpriteChanger.GetComponent<Player>().weapon.enabled = false;
        gunSpriteChanger.GetComponent<Player>().weapon = this;
        if (GameManager.instance != null) {
            GameManager.instance.weapon = this;
            GameManager.instance.weaponSprite = GunSide;
            movementJoystick = GameManager.instance.movementJoystick;
        }


    }

    public virtual void Shoot() {
    }
    public virtual void UpgradeWeapon() {
        weaponLevel++;
        GameManager.instance.data.weaponLevel[weaponID] = weaponLevel;

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

    public void PlayShootAudio() {
        audioS.Play();
    }

    protected Vector2 PCShootDirection(Vector2 myPos) {
        if (GameManager.instance.player.target == null) {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        return (target - myPos).normalized;
    }

    protected Vector2 MobileShootDirection(Vector2 myPos) {
        if (GameManager.instance.player.target == null) {
            return movementJoystick.Direction.normalized;
        }
        else {
            return (target - myPos).normalized;
        }
    }
}
