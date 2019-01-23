using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    //required sprites
    public Sprite GunSide, GunUp, GunDown, GunDiagUp, GunDiagDown;
    public int weaponID;
    public List<int> weaponPrices = new List<int>(){35,70,135,200,275,400};
    public int weaponLevel = 1;
    public GameObject Bullet;
    public GameObject emptyShells;
    public GameObject emptyShellsContainer;
    //damage States
    public int[] damagePoint = {1,2,3,4,5,6,7};
    public float[] pushForce = {2.0f,2.2f,2.6f,2.8f,3.0f,3.2f,3.4f};
    public float shootCoolDown = 0.2f;
    public Transform bulletSpawnPoint;
    public GunSpriteChanger gunSpriteChanger;
    public Vector2 target;

    protected AudioSource audioS;
    protected int damageStartValue = 1;
    protected bool isOnPc;
    protected Joystick movementJoystick;

    private Player player;

    protected virtual void Awake() {
        audioS = GetComponent<AudioSource>();
    }

    protected virtual void Start() {
       
    }

    protected virtual void Update() { }

    public void ChangeSprites() {
        if (GameManager.instance.data.weaponSelected != weaponID)
            return;
        if(player == null)
         player = gunSpriteChanger.GetComponent<Player>();
        isOnPc = player.isOnPc;
        gunSpriteChanger.GunSide.sprite = GunSide;
        gunSpriteChanger.GunUp.sprite = GunUp;
        gunSpriteChanger.GunDown.sprite = GunDown;
        gunSpriteChanger.GunDiagUp.sprite = GunDiagUp;
        gunSpriteChanger.GunDiagDown.sprite = GunDiagDown;
        if (player.weapon != null && player.weapon != this)
            player.weapon.enabled = false;
        player.weapon = this;
        if (GameManager.instance != null) {
            GameManager.instance.weapon = this;
            GameManager.instance.weaponSprite = GunSide;
            movementJoystick = GameManager.instance.movementJoystick;
            player.coolDown = shootCoolDown;
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
            Vector2 direction = movementJoystick.Direction.normalized;
            if (direction != Vector2.zero)
                 return direction;
            return -Vector2.up;
        }

        else {
            return (target - myPos).normalized;
        }
    }
}
