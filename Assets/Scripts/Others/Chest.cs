using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chest : Collectable {


    public Sprite emptyChest;
    public int pesosAmount = 5;

    protected override void OnCollect() {
        if (!collected) {
            
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.gold += pesosAmount;
            //over a second text goes 50 pixel up
            int random = Random.Range(0, 4);
            if(random == 2 || random == 3) {
            
                if (GameManager.instance.UnlockWeapon()) {
                    //GameManager.instance.ShowText("new weapon!", 23, Color.green, transform.position, Vector3.up * 25, 1.5f);
                    GameManager.instance.ShowToastMessage("New Weapon!", 5f);
                    AudioController.instance.PlaySound(SoundClip.rewardWeapon);
                }
                //if all weapons are already unlocked we just give player gold
                else {
                    GameManager.instance.ShowText("+" + pesosAmount + " gold!", 23, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
                    AudioController.instance.PlaySound(SoundClip.rewardCoin);
                }
            }
            else {
                GameManager.instance.ShowText("+" + pesosAmount + " gold!", 23, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
                AudioController.instance.PlaySound(SoundClip.rewardCoin);
            }
            

        }
    }
}
