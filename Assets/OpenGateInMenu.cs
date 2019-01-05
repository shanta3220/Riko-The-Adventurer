using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateInMenu : Collidable {
    public ScenePortal scenePortal;
    protected override void OnCollide(Collider2D col) {
        if (col.tag == "Player") {
            scenePortal.OpenDoor();
            Destroy(gameObject);
        }
    }
}
