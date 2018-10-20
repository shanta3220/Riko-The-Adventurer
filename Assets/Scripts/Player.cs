using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour {

    public bool isPlayerAdvanced;

    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private Animator anim;
    private RaycastHit2D hit;

	private void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //reset moveDelta
        moveDelta = new Vector3(x,y,0);
        if(!isPlayerAdvanced)
            PlayerMovement(moveDelta.x);
        else PlayerMovement(moveDelta.x, moveDelta.y);

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

    private void PlayerMovement(float x,float y) {
        //swap sprite direction whether moving left or right and animation
        if (x > 0) {
            transform.localScale = Vector3.one;
            if (y > 0)
                //up side
                anim.SetInteger("Anim", 3);
            else if(y < 0)
                anim.SetInteger("Anim", 4);
            else if(y==0)
                anim.SetInteger("Anim", 0);

        }
        else if (x == 0) {
            if (y > 0)
                //up side
                anim.SetInteger("Anim", 1);
            else if (y < 0)
                anim.SetInteger("Anim", 2);
            else if (y == 0)
                anim.SetInteger("Anim", -1);
        }

        else if (x < 0 ) {
            transform.localScale = new Vector3(-1, 1, 1);
            if (y > 0)
                //up side
                anim.SetInteger("Anim", 3);
            else if (y < 0)
                anim.SetInteger("Anim", 4);
            else if (y == 0)
                anim.SetInteger("Anim", 0);
        }
        
      

    }
}
