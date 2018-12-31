﻿using System.Collections;
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
            GameManager.instance.ShowText("+" + pesosAmount + " gold!", 23, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
            AudioController.instance.PlaySound(SoundClip.rewardCoin);

        }
    }
}
