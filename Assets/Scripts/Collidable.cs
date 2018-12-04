using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collidable : MonoBehaviour {
    
    //with which player can collide with
    public ContactFilter2D filter;

    private BoxCollider2D boxCollider;
    //contains the data we hit in single frame [10] is big amount
    private Collider2D[] hits = new Collider2D[10];


    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {
        //collision work

        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if(hits[i] == null) {
                continue;
            }
            OnCollide(hits[i]);
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D col) {
        Debug.Log("OnCollide was not implemented in "+ this.name);
    }
}
