using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeechTracker : Collidable {

    public GameObject canvasDialogue;
    public Sprite CharacterAvatar;
    public Text characterText;
    public string Message;

    private Animator anim;
    private Image CharacterAvatarImage;
    private bool isCollided;
    protected override void Start() {
        base.Start();
        anim = canvasDialogue.GetComponent<Animator>();
        CharacterAvatarImage = canvasDialogue.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    protected override void OnCollide(Collider2D col) {
        if(col.tag == "Player") {
            characterText.text = Message;
            CharacterAvatarImage.sprite = CharacterAvatar;
            anim.SetBool("ShowPanel", true);
        }

    }

    public void HideDialoguePanel() {
        anim.SetBool("ShowPanel", false);
    }
}
