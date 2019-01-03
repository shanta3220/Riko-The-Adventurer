using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyShellMovement : MonoBehaviour {
    Color[] colors = { Color.red, Color.blue, Color.green, Color.magenta, Color.yellow, Color.blue, Color.cyan };
 
    void Start () {
        Invoke("StopMoving", 0.2f);
        Color color = colors[Random.Range(0, colors.Length)];
        GetComponent<SpriteRenderer>().color = new Color(color.r,color.g,color.b,0.5f);

    }
	
	private void StopMoving() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Destroy(this);
    }
}
