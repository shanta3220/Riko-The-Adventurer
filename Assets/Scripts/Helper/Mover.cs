using UnityEngine;

public abstract class Mover : Fighter {

    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected Animator anim;
    protected RaycastHit2D hit;
    protected bool isRunning;
    public int[] healthLength = { 5, 10, 15, 25, 30, 50, 70};
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;
    protected float localScaleSize;
    [HideInInspector]
    public bool hasEnemyTarget;
    protected AudioSource audioS;
    protected SpriteRenderer sr;
    protected bool flipx = false;

    private void Awake() {
        anim = GetComponent<Animator>();
        localScaleSize = transform.localScale.x;
        audioS = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        if (sr.flipX)
            flipx = true;
    }

    protected virtual void Start() {

        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input) {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed,0);
        isRunning = input.x != 0 || input.y != 0 ? true : false;
        if(moveDelta.x != 0)
            Animate(moveDelta.x);
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

    protected virtual void Animate(float x) {
        if (gameObject.tag == "Player")
            return;
        //swap sprite direction whether moving left or right and animation
        if (x > 0) {
            //transform.localScale = Vector3.one * localScaleSize;
            sr.flipX = flipx == false ? false : true;
        }

        else if (x < 0) {
            //transform.localScale = new Vector3(-localScaleSize, localScaleSize, localScaleSize);
            sr.flipX = flipx == true ? false : true;
        }
        moveDelta += pushDirection;
        //reduce pushForce everyframe, based on recoveryspeed
        pushDirection = Vector3.Lerp(pushDirection,Vector3.zero,PushRecoverySpeed);
        if(anim != null)
            anim.SetBool("IsRunning", isRunning);
    }

}
