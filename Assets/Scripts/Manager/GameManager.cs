using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Resources
    public List<Sprite> playerSprites;
    public Sprite weaponSprite;
    public List<int> xpTable;
    //only changing animators change the player skins
    public RuntimeAnimatorController[] playerSkins = new RuntimeAnimatorController[3];

    //References
    public Player player;
    public RectTransform healthBar;
    public Animator deathMenuAnimator;
    public int gold;
    public int experience;
    [HideInInspector]
    public Weapon weapon;
    public GameData data;
    public List<Weapon> collectedWeapons;
    private Transform weaponContainer;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        
        weaponContainer = player.weaponContainer.transform;
        data = DataController.instance.data;
        LoadData();
    }
    
    //floatingText;
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.instance.Show(msg, fontSize, color, position, motion, duration);

    }

    //Unlock Characters
    public bool TryUnlockSkin(int skinNumber, int skinPrice) {
        if (data.skins[skinNumber])
            return true;
        //do we have enough gold? if so upgrade and decrement the weaponprice from the gold
        if (gold >= skinPrice) {
            gold -= skinPrice;
            data.skins[skinNumber] = true;
            data.selectedSkin = skinNumber;
            player.ChangeSkin(skinNumber);
            return true;
        }
        return false;
    }

    //Weapon System
    public bool TryUpgradeWeapon()
    {
        //is the weapon max level
        if (weapon.weaponPrinces.Count == weapon.weaponLevel)
            return false;
        //do we have enough gold? if so upgrade and decrement the weaponprice from the gold
        else if(gold >= weapon.weaponPrinces[weapon.weaponLevel]){
            gold -= weapon.weaponPrinces[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
        
    }
    public void UnlockWeapon() {
        int dataID = data.notCollectedWeapons[Random.Range(0, data.notCollectedWeapons.Count)];
        data.collectedWeapons.Add(dataID);
        data.notCollectedWeapons.Remove(dataID);
        Weapon playerWep = CheckWeapons(dataID, collectedWeapons.Count);
        data.weaponSelected = collectedWeapons.Count;
        collectedWeapons.Add(playerWep);
        playerWep.enabled = true;
        playerWep.ChangeSprites();
    }
    public void SwitchWeapon() {
 
        if (weapon.weaponID == collectedWeapons.Count - 1) {
            data.weaponSelected = 0;
            Weapon wep = collectedWeapons[0];
            wep.enabled = true;
            weaponSprite = wep.GunSide;
            wep.ChangeSprites();
        }
        else {
            data.weaponSelected = weapon.weaponID + 1;
            Weapon wep = collectedWeapons[weapon.weaponID + 1] as Weapon;
            wep.enabled = true;
            weaponSprite = wep.GunSide;
            wep.ChangeSprites();
        }
    }
    //if weapon is lastselected weapon before closing game enable else disable
    private bool IsSelectedWeapon(Weapon weapon) {
        if (weapon.weaponID == data.weaponSelected) {
            weapon.ChangeSprites();
            return true;
        }
        return false;
    }
    //checking collected weapons, if weapon is collected then give it a ID
    public Weapon CheckWeapons(int dataWepID, int newID) {
        Weapon temp = weaponContainer.GetChild(dataWepID).GetComponent<Weapon>();
        temp.weaponID = newID;
        temp.weaponLevel = data.weaponLevel[newID];
        temp.enabled = IsSelectedWeapon(temp);
        return temp;
    }
    public int GetCurrentWeaponDamage() {
        return weapon.damagePoint[weapon.weaponLevel];
    }
    public float GetCurrentWeaponPushForce() {
        return weapon.pushForce[weapon.weaponLevel];
    }

    //experience system
    public int GetCurrentLevel() {
        int r = 0;
        int add = 0;
        while (experience >= add) {
            add += xpTable[r];
            r++;
            if (r == xpTable.Count)// max level
                return r;
        }
        return r;
    }
    public int GetXpFromLevel(int level) {
        int r = 0;
        int xp = 0;

        while (r < level) {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp) {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp() {
        ShowText("Level Up!", 25, Color.green, player.transform.position, Vector3.up * 50, 1.0f);
        player.OnLevelUp();
        OnHealthChange();
    }
    
    //player health bar

    public void OnHealthChange() {
        float ratio = (float)player.health / player.maxHealth;
        healthBar.localScale = new Vector2(ratio, 1);
    }

    //Death Menu And Respawn
    public void Respawn() {
        SceneManager.LoadScene("Main");
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Save  Data
    public void SaveData() {
        data.gold = gold;
        data.experience = experience;
        data.weaponSelected = weapon.weaponID;
        data.weaponLevel[weapon.weaponID] = weapon.weaponLevel;
        DataController.instance.SaveData(data);
    }

    // Load data
    public void LoadData() {
        player.ChangeSkin(data.selectedSkin);
        gold = data.gold;
        experience = data.experience;
        int i = 0;
        //checking collected weapons, i is the sequential id of collected weapons
        //dataWepID is the child number of the wepcontainer that has weapon script
        foreach (int dataWepID in data.collectedWeapons) {
            Weapon wep = CheckWeapons(dataWepID, i);
            collectedWeapons.Add(wep);
            wep.enabled = IsSelectedWeapon(wep);
            i++;
        }
        int getCurrentLevel = GetCurrentLevel();
        if (getCurrentLevel > 1) {
            //player health based on experience, -1 because level 1 counts as well to increase health
            player.SetLevelHealth(getCurrentLevel - 1);
        }
       
    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void ClearData() {
        SaveData();
    }

    public void OnDestroy() {

        SaveData();
        //PlayerPrefs.SetString("MySettingsEditor", "");
        //PlayerPrefs.SetString("MySettings", "");
    }



    /*/// <summary>
/// int preferedSkin,
/// int pesosAmount,
/// int experience
/// int weaponLevel
/// </summary>
//save state
public void SaveState() {
    string s = "";
    s += "0" + "|";
    s += gold.ToString() + "|";
    s += experience.ToString();
    s += weapon.weaponLevel.ToString();

    PlayerPrefs.SetString("Setting", s);

    //PlayerPrefs.DeleteAll();
}


public void LoadState(Scene scene, LoadSceneMode mode) {
    if (!PlayerPrefs.HasKey("Setting"))
        return;
    string[] data = PlayerPrefs.GetString("Setting").Split('|');

    //Change PlayerScreen
    // amount of pessos
    gold = int.Parse(data[1]);
    experience = int.Parse(data[2]);
    //change weapon level
    weapon.weaponLevel = int.Parse(data[3]);
}*/

}
