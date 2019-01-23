using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeechTracker : Collidable {

    public GameObject canvasDialogue;
    public Sprite CharacterAvatar;
    public Text characterText;
    public string Message;
    public Transform player;
    private Animator anim;
    private Image CharacterAvatarImage;
    private bool isCollided;
    private bool canvasOpened;

    protected override void Start() {
        base.Start();
        anim = canvasDialogue.GetComponent<Animator>();
        CharacterAvatarImage = canvasDialogue.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    protected override void Update() {
        base.Update();
        if (canvasOpened) {
            if (Vector3.Distance(player.transform.position, transform.position) > 1f) {
                HideDialoguePanel();
            }
               
        }
    }

    protected override void OnCollide(Collider2D col) {
        if(col.tag == "Player") {
            player = col.gameObject.transform;
            characterText.text = Message;
            CharacterAvatarImage.sprite = CharacterAvatar;
            canvasOpened = true;
            anim.SetBool("ShowPanel", canvasOpened);
        }

    }

    public void HideDialoguePanel() {
        canvasOpened = false;
        anim.SetBool("ShowPanel", canvasOpened);
        
    }
}
