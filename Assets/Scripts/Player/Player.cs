using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]


public class Player : Mover {

    //target to shoot
    public Transform target;
    public bool hasTarget;
    public GameObject weaponContainer;
    public Weapon weapon;
    public AudioClip gameOverClip;
    private Animator shootAnim;
    public int maxNumberOfActiveBullets = 10;
    public int numOfActiveBullets=0;

    //public Joystick movementJoystick, shootJoystick;
    public float coolDown = 0.2f;
    protected float lastShoot;

    [HideInInspector]
    public Joystick movementJoystick;
    public bool isOnPc;
    private bool isAlive = true;
    private CameraShake screenShake;

    protected override void Start() {
        base.Start();
        shootAnim = GetComponent<GunSpriteChanger>().bulletSpawnPoint.GetComponent<Animator>();
        screenShake = Camera.main.GetComponent<CameraShake>();
    }


    private void FixedUpdate() {
        if (GameManager.instance.isPaused||!isAlive)
            return;
        float x, y;
        if (isOnPc) {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
        else {
            x = Mathf.RoundToInt(movementJoystick.Horizontal);
            y = Mathf.RoundToInt(movementJoystick.Vertical);
           
        }
        UpdateMotor(new Vector3(x, y, 0));
    }


   public void Shoot() {
        if (isAlive && numOfActiveBullets<maxNumberOfActiveBullets) {
            if (Time.time - lastShoot > coolDown) {
                numOfActiveBullets++;
                lastShoot = Time.time;
                shootAnim.SetTrigger("Shoot");
                screenShake.ShakeIt(coolDown);
                weapon.Shoot();
                weapon.PlayShootAudio();
                
            }
        }
    }

    protected override void UpdateMotor(Vector3 input) {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);
        isRunning = input.x != 0 || input.y != 0 ? true : false;
        if (target == null) {
            if (isOnPc) {
                TurningPC();
            }
            else {
                TurningMobile();
            }
           
        }
        else {
            PlayerFaceEnemyAnim(target);
        }
        //making sure we can move the player upward or downward by casting a box there beforehand
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {
            //moving the player
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        else if (hit.collider != null && pushDirection != Vector3.zero) {
            pushDirection = Vector3.zero;
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {
            //moving the player
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        else if (hit.collider != null && pushDirection != Vector3.zero) {
            pushDirection = Vector3.zero;
        }
    }


    private void TurningPC() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        Vector2 localMove = (target.position - transform.position).normalized;
        Animate(Mathf.RoundToInt(localMove.x), Mathf.RoundToInt(localMove.y));
        weapon.target = target.transform.position;
    }

    protected override void ReceiveDamage(Damage dmg) {
        if (!isAlive || GameManager.instance.isPaused)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHealthChange();
        Hurt();
    }


    public void ChangeSkin(int skinId) {
        anim.runtimeAnimatorController = GameManager.instance.playerSkins[skinId];
    }
   
    /*public void OnLevelUp() {
        //increasing health
        maxHealth++;
        health = maxHealth;
    }

    public void SetLevelHealth(int level) {
        //increase the health based on your level
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }*/

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
        audioS.clip = gameOverClip;
        audioS.Play();
    }

    private void Respawn() {
        pushDirection = Vector3.zero;
        lastImmune = Time.time;
        Heal(maxHealth);
        isAlive = true;
     

    }
    private void Hurt() {
        sr.color = Color.red;
        Invoke("RestoreColor", 0.5f);
        if (audioS != null && !audioS.isPlaying)
            audioS.Play();
    }

    private void RestoreColor() {
        sr.color = Color.white;
    }

    public void TurningMobile() {
        Vector2 direction =  movementJoystick.Direction.normalized;
        Animate(direction.x, direction.y);
    }

    public void SetLevelHealth(int currentPlayerLevel) {
        //increasing health
        maxHealth = healthLength[currentPlayerLevel];
        health = maxHealth;
    }


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
