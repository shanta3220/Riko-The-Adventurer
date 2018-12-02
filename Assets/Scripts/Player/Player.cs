using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour {

    public bool isPlayerAdvanced;

    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private Animator anim;
    private Animator shootAnim;
    private RaycastHit2D hit;

   
    public Transform target;

    public bool hasTarget;
    bool isWalking;
    public Weapon weapon;
    //public Joystick movementJoystick, shootJoystick;

    protected float coolDown = 0.2f;
    protected float lastShoot;
    private void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        shootAnim = GetComponent<GunSpriteChanger>().bulletSpawnPoint.GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
       // isWalking = x != 0 || y != 0 ? true : false;
        //reset moveDelta
        moveDelta = new Vector3(x,y,0);
        TurningPC();
        /*if (!isPlayerAdvanced)
            PlayerMovement(moveDelta.x);
        else {
            TurningPC();
           /* if (x != 0 || y != 0)
               PlayerMovement(x, y);
        }*/
       
        /*if (hasTarget) {
            PlayerFaceEnemyAnim(target);
        }*/
        

        //making sure we can move the player upward or downward by casting a box there beforehand
        hit = Physics2D.BoxCast(transform.position,boxCollider.size,0,new Vector2(0,moveDelta.y),Mathf.Abs(moveDelta.y*Time.deltaTime),LayerMask.GetMask("Actor","Blocking"));
        if(hit.collider == null) {
            //moving the player
            transform.Translate(0,moveDelta.y * Time.deltaTime,0);
        }
        
        hit = Physics2D.BoxCast(transform.position,boxCollider.size,0,new Vector2(moveDelta.x,0),Mathf.Abs(moveDelta.x*Time.deltaTime),LayerMask.GetMask("Actor","Blocking"));
        if(hit.collider == null) {
            //moving the player
            transform.Translate(moveDelta.x * Time.deltaTime,0,0);
        }
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            if (Time.time - lastShoot > coolDown) {
                lastShoot = Time.time;
                shootAnim.SetTrigger("Shoot");
                weapon.Shoot();
            }
        }
    }

    //n3k's
    private void PlayerMovement(float x) {
        //swap sprite direction whether moving left or right and animation
        if (x > 0) {
            transform.localScale = Vector3.one;
            anim.SetInteger("Anim", 1);
        }

        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetInteger("Anim", 1);
        }
        else anim.SetInteger("Anim", 0);

    }


    private void TurningPC() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        //transform.up = direction;
        FaceDirecion(direction.x, direction.y);

    }

    private void FaceDirecion(float x, float y) {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);
       
        if (x > 0) {
            transform.localScale = Vector3.one;
        }

        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        weapon.rotateAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        // anim.SetBool("IsWalking", isWalking);
        x = Mathf.Abs(x);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }

  

    private void PlayerFaceEnemyAnim(Transform target) {
        float x, y;
        //Vector2 localMove = transform.InverseTransformDirection(target.position).normalized;
        Vector2 localMove = (target.position - transform.position).normalized;
        x = Mathf.RoundToInt(localMove.x);
        y = Mathf.RoundToInt(localMove.y);
       /* if (localMove.x > -0.5 && localMove.x < 0.5)
            x = 0;
        else x = Mathf.Sign(localMove.x);

        if (localMove.y > -0.5 && localMove.y < 0.5)
            y = 0;
        else y = Mathf.Sign(localMove.y);*/

        if (x > 0) {
            transform.localScale = Vector3.one;
        }

        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
       // anim.SetBool("IsWalking", isWalking);
        x = Mathf.Abs(x);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
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



}
