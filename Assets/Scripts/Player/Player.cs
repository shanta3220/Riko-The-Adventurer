using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour {

    public bool isPlayerAdvanced;

    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private Animator anim;
    private RaycastHit2D hit;

    public Transform target;

    public bool hasTarget;
    bool isWalking;
	private void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //isWalking = x != 0 || y != 0 ? true : false;
        
        //reset moveDelta
        moveDelta = new Vector3(x,y,0);
        if(!isPlayerAdvanced)
            PlayerMovement(moveDelta.x);
        else PlayerMovement(x,y);
       
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

   
    private void PlayerMovement(float x, float y) {
        //swap sprite direction whether moving left or right and animation
        if (x > 0) {
            transform.localScale = Vector3.one;
        }
  
        else if (x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
          
        }
        x = Mathf.Abs(x);
       // anim.SetBool("IsWalking", isWalking);
        anim.SetFloat("FaceX", x);
        anim.SetFloat("FaceY", y);

    }

  

    private void PlayerFaceEnemyAnim(Transform target) {
        float x, y;
        //Vector2 localMove = transform.InverseTransformDirection(target.position).normalized;
        Vector2 localMove = (target.position - transform.position).normalized;
        if (localMove.x > -0.5 && localMove.x < 0.5)
            x = 0;
        else x = Mathf.Sign(localMove.x);

        if (localMove.y > -0.5 && localMove.y < 0.5)
            y = 0;
        else y = Mathf.Sign(localMove.y);

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


}
