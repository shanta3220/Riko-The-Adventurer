using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text field
    public Text levelText, hitpointText, goldText,upgradeCostText,xpText;
    //logic field
    public Image CharacterSelectionImage;
    public Image currentWeaponImage;
    public RectTransform xpBar;

    
    private int currentCharacterSelection = 0;

    //CharacterSelection
    public void OnArrowClick(bool right){
        if(right)
        {
            currentCharacterSelection++;
            //if no more character
            if(GameManager.instance.playerSprites.Count == currentCharacterSelection)
                currentCharacterSelection = 0;
            OnSelectionChange();
           
        }
        else{
             currentCharacterSelection--;
             if(currentCharacterSelection < 0){
                 currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
             }
            OnSelectionChange();
        }
    }

    private void OnSelectionChange() 
    {
        CharacterSelectionImage.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
    }

    //Ceapon upgrade
    public void OnUpgradeClick()
    {

    }

    //update character Information

    public void UpdateMenu()
    {
        //weapon
        currentWeaponImage.sprite = GameManager.instance.weaponSprites[0];
        upgradeCostText.text = "NOT IMPLEMENTED";
        //meta
        hitpointText.text = GameManager.instance.player.health.ToString();
        goldText.text = GameManager.instance.gold.ToString();
        levelText.text = "NOT IMPLEMENTED";

        //XP Bar
        xpText.text = "NOT IMPLEMENTED";
        xpBar.localScale = new Vector2(0.5f,1f);
    }


}
