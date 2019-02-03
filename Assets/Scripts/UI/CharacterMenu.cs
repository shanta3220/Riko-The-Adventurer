using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour {
    //Text field
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;
    public Text characterSelectorText;
    public Text damagePointText, pushForceText;
    public Button characterSelectorButton;
    //logic field
    public Image CharacterSelectionImage;
    public Image currentWeaponImage;
    public RectTransform xpBar;

    public int currentCharacterSelection = 0;
    private int[] skinPrices = { 0, 300, 1000 };
    private Weapon wep;
    private int currentWeaponID = 0;

    private void Start() {
        Invoke("DeleyValue", 0.5f);
    }

    //CharacterSelection
    public void OnArrowClick(bool right) {
        if (right) {
            currentCharacterSelection++;
            //if no more character
            if (GameManager.instance.playerSprites.Count == currentCharacterSelection)
                currentCharacterSelection = 0;
            OnSelectionChange();
        
        }
        else {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0) {
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }
            OnSelectionChange();
        }
    }

    public void OnArrowWeaponClick(bool right) {
        if (right) {
            currentWeaponID++;
            //if no more character
            if (GameManager.instance.collectedWeapons.Count == currentWeaponID)
                currentWeaponID = 0;
        }
        else {
            currentWeaponID--;
            if (currentWeaponID < 0) {
                currentWeaponID = GameManager.instance.collectedWeapons.Count - 1;
            }
            
        }
        CheckWeapon(currentWeaponID);
    }

    private void OnSelectionChange() {
        CharacterSelectionImage.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        SkinSelector();
    }

    //Ceapon upgrade
    public void OnUpgradeClick() {
        if (GameManager.instance.TryUpgradeWeapon(wep))
            UpdateMenu();
    }

    //update character Information
    public void UpdateMenu() {
        //weapon

        CharacterSelectionImage.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        /*currentWeaponImage.sprite = GameManager.instance.weaponSprite;
        int weaponLevel = GameManager.instance.weapon.weaponLevel;
        if(GameManager.instance.weapon.weaponLevel == GameManager.instance.weapon.weaponPrices.Count) {
            upgradeCostText.text = "Max Level";
        }
           
        else upgradeCostText.text = GameManager.instance.weapon.weaponPrices[weaponLevel].ToString();*/
        CheckWeapon(currentWeaponID);
        //meta
        hitpointText.text = GameManager.instance.player.health.ToString();
        goldText.text = GameManager.instance.gold.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        /*damagePointText.text = GameManager.instance.GetCurrentWeaponDamage().ToString();
        pushForceText.text = GameManager.instance.GetCurrentWeaponPushForce().ToString();*/

        //XP Bar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        //if max level
        if (currentLevel == GameManager.instance.xpTable.Count) {
            xpText.text = GameManager.instance.experience.ToString() + " Total Experience Points";//displaying total xp
            xpBar.localScale = new Vector2(1f, 1f);
        }
        else {
            int previousLevelXp = GameManager.instance.GetXpFromLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXpFromLevel(currentLevel);
            int diff = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;
            float completationRatio = (float)currentXpIntoLevel / (float)diff;
            xpText.text = currentXpIntoLevel.ToString() + "/" + diff;
            xpBar.localScale = new Vector2(completationRatio, 1f);
        }
       
        
    }

    public void UnlockSkin() {
        if (GameManager.instance.TryUnlockSkin(currentCharacterSelection, skinPrices[currentCharacterSelection])) {
            characterSelectorText.text = "Selected";
            UpdateMenu();
        }
        else characterSelectorText.text = skinPrices[currentCharacterSelection].ToString();

    }

    public void SkinSelector() {
        //if is unlocked
        if (GameManager.instance.data.skins[currentCharacterSelection]) {
            characterSelectorText.text = "Selected";
            GameManager.instance.player.ChangeSkin(currentCharacterSelection);
            GameManager.instance.data.selectedSkin = currentCharacterSelection;
        }
        else {
            characterSelectorText.text = skinPrices[currentCharacterSelection].ToString();
        }
    }

    public void DeleyValue() {
        currentCharacterSelection = GameManager.instance.data.selectedSkin;
        currentWeaponID = GameManager.instance.weapon.weaponID;
        CheckWeapon(currentWeaponID);
    }

    public void CheckWeapon(int ID) {
        wep = GameManager.instance.collectedWeapons[ID] as Weapon;
        if (wep.weaponLevel == wep.weaponPrices.Count) {
            upgradeCostText.text = "Max Level";
        }
        else upgradeCostText.text = wep.weaponPrices[wep.weaponLevel].ToString();
        damagePointText.text = wep.damagePoint[wep.weaponLevel].ToString();
        pushForceText.text = wep.pushForce[wep.weaponLevel].ToString();
        currentWeaponImage.sprite = wep.GunSide;
    }

    //so that we reset value and methods based on the current weapon the player is holding 
    public void ResetCurrentWeaponID() {
        currentWeaponID = GameManager.instance.weapon.weaponID;
        wep = GameManager.instance.weapon;
    }
}
