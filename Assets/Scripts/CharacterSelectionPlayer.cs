using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionPlayer : MonoBehaviour{
   
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;
    private Vector3 moveDelta;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;
    private float localScaleSize;
    private SpriteRenderer sr;
    public Sprite defaultCharacterSprite;
    private bool isDisabled = true;
    public bool isOnPanel;
    public Joystick movementJoystick;
    public bool isOnPc;
    private void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        localScaleSize = transform.localScale.x;
    }

    private void OnEnable() {
        if(sr == null)
            sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        if (anim == null)
            anim = GetComponent<Animator>();
        anim.enabled = true;
        isDisabled = false;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            isOnPc = true;
        else if (Application.platform == RuntimePlatform.Android)
            isOnPc = false;
    }

    private void FixedUpdate() {
        if (isDisabled)
            return;
        if (isOnPc) {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            UpdateMotor(new Vector3(x, y, 0));
        }

        else {
            float x = movementJoystick.Horizontal;
            float y = movementJoystick.Vertical;
            UpdateMotor(new Vector3(x, y, 0));
        }
    }


    private void UpdateMotor(Vector3 input) {
        if (isOnPanel)
            return;
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);
        if (isOnPc) {
            TurningPC();
        }
        else {
            TurningMobile();
        }

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

    private void Animate(float x, float y) {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        if (x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (x < 0) {
            transform.localScale = new Vector3(-localScaleSize, localScaleSize, localScaleSize);
        }
        x = Mathf.Abs(x);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);
    }


    private void TurningPC() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        Animate(direction.x, direction.y);
    }

    public void TurningMobile() {
        Vector2 direction = movementJoystick.Direction.normalized;
        Animate(direction.x, direction.y);
    }

    private void OnDisable() {
        isDisabled = true;
        sr.sprite = defaultCharacterSprite;
        sr.color = Color.gray;
        anim.enabled = false;
    }



}
