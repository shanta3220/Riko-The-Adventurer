using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]


public class Player : Mover {

    //target to shoot
    public Transform target;
    public bool hasTarget;
    public GameObject weaponContainer;
    public Weapon weapon;
    private Animator shootAnim;

    private bool isAlive = true;
   
    //public Joystick movementJoystick, shootJoystick;
    protected float coolDown = 0.2f;
    protected float lastShoot;

    private CameraShake screenShake;

    protected override void Start() {
        base.Start();
        shootAnim = GetComponent<GunSpriteChanger>().bulletSpawnPoint.GetComponent<Animator>();
        screenShake = Camera.main.GetComponent<CameraShake>();
    }


    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }


    void Update() {

        if (Input.GetMouseButton(0) && isAlive) {
            if (Time.time - lastShoot > coolDown) {
                lastShoot = Time.time;
                shootAnim.SetTrigger("Shoot");
                screenShake.ShakeIt(coolDown);
                weapon.Shoot();
                weapon.PlayShootAudio();
            }
        }
    }


    protected override void UpdateMotor(Vector3 input) {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed,0);
        isRunning = input.x != 0 || input.y != 0 ? true : false;
        TurningPC();
        //making sure we can move the player upward or downward by casting a box there beforehand
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {
            //moving the player
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {
            //moving the player
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        
    }


    private void TurningPC() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        Animate(direction.x, direction.y);

    }

    private void Animate(float x, float y) {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        if (x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //add push vector, if any
        moveDelta += pushDirection;
        //reduce pushForce everyframe, based on recoveryspeed
        pushDirection = Vector3.Lerp(pushDirection,Vector3.zero,PushRecoverySpeed);
       /* if(weapon.enabled)
            weapon.rotateAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;*/
        // anim.SetBool("IsWalking", isWalking);
        x = Mathf.Abs(x);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }

  

    private void PlayerFaceEnemyAnim(Transform target) {
        float x, y;
        Vector2 localMove = (target.position - transform.position).normalized;
        x = Mathf.RoundToInt(localMove.x);
        y = Mathf.RoundToInt(localMove.y);

        if (x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        x = Mathf.Abs(x);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }

    protected override void ReceiveDamage(Damage dmg) {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHealthChange();
    }


    public void ChangeSkin(int skinId) {
        anim.runtimeAnimatorController = GameManager.instance.playerSkins[skinId];
    }
   
    public void OnLevelUp() {
        //increasing health
        maxHealth++;
        health = maxHealth;
    }

    public void SetLevelHealth(int level) {
        //increase the health based on your level
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void Heal(int healAmount) {
        if (health == maxHealth)
            return;
        health += healAmount;
        if (health > maxHealth)
            health = maxHealth;
        GameManager.instance.ShowText("+" + healAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 50, 1.0f);
        GameManager.instance.OnHealthChange();

    }

    protected override void Death() {
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
        isAlive = false;
    }

    private void Respawn() {
        pushDirection = Vector3.zero;
        lastImmune = Time.time;
        Heal(maxHealth);
        isAlive = true;

    }
    /*public void TurningMobile() {

        if (shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0) {
            isShooting = true;
            transform.forward = new Vector3(shootJoystick.Horizontal, 0f, shootJoystick.Vertical);
        }
        else if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0) {
            transform.forward = new Vector3(horizontal, 0f, vertical);
            isShooting = false;
        }
        else {
            isShooting = false;
        }
    }*/

    /* private void OldPlayerSwitch() {
         /*if (!isPlayerAdvanced)
           Animate(moveDelta.x);
       else {
           TurningPC();
          /* if (x != 0 || y != 0)
              Animate(x, y);
       }*/

    /*if (hasTarget) {
        PlayerFaceEnemyAnim(target);
    }

}*/
}
