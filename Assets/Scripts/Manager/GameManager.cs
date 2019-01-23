using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    //Resources
    public List<Sprite> playerSprites;
    public Sprite weaponSprite;
    // public List<int> xpTable = new List<int>() { 8,17,25,35,45,55,65};
    [HideInInspector]
    public List<int> xpTable = new List<int>() { 8, 25, 40, 65, 90, 110, 135 };
    //only changing animators change the player skins
    public RuntimeAnimatorController[] playerSkins = new RuntimeAnimatorController[3];
    //References
    public Player player;
    public RectTransform healthBar;
    public Animator deathMenuAnimator;
    public Animator pauseMenuAnimator;
    public Animator toastMessageAnimator;
    public Text textToastMessage;
    public Text enemyKillText;
    [HideInInspector]
    public int gold;
    [HideInInspector]
    public int experience;
    [HideInInspector]
    public Weapon weapon;
    public GameData data;
    public List<Weapon> collectedWeapons;

    public bool isPaused;
   
    public static GameManager instance;
    public int playerCurrentLevel;
    public EnemyActivator enemyActivator;
    public ScenePortal scenePortal;
    public EnemyBatchHandler enemyBatchHandler;
    public Image switchWepImage;
    public bool isLevelCompleted; //to avoid Bug
    public Joystick movementJoystick;
    public int totalEnemies = 19;
    public SceneLoadingBarController loadLevel;
    private Transform weaponContainer;
    private int currentenemyKill = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        
        weaponContainer = player.weaponContainer.transform;
        data = DataController.instance.data;
        LoadData();
        UpdateEnemyKillText();
    }
    
    //floatingText;
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        if (!player.isOnPc)
            fontSize += 7;
        FloatingTextManager.instance.Show(msg, fontSize, color, position, motion, duration);

    }

    public void ShowToastMessage(string message, float duration) {
        textToastMessage.text = message;
        toastMessageAnimator.SetBool("ShowPanel", true);
        Invoke("HideToastMessage", duration);
    }

    private void HideToastMessage() {
        toastMessageAnimator.SetBool("ShowPanel", false);
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
        if (weapon.weaponPrices.Count == weapon.weaponLevel)
            return false;
        //do we have enough gold? if so upgrade and decrement the weaponprice from the gold
        else if(gold >= weapon.weaponPrices[weapon.weaponLevel]){
            gold -= weapon.weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            SaveData();
            return true;
           
        }
        return false;
    }
    //Weapon System
    public bool TryUpgradeWeapon(Weapon wep) {
        //is the weapon max level
        if (wep.weaponPrices.Count == wep.weaponLevel)
            return false;
        //do we have enough gold? if so upgrade and decrement the weaponprice from the gold
        else if (gold >= wep.weaponPrices[wep.weaponLevel]) {
            gold -= wep.weaponPrices[wep.weaponLevel];
            wep.UpgradeWeapon();
            SaveData();
            return true;

        }
        return false;
    }

    public bool UnlockWeapon() {
        if (data.notCollectedWeapons.Count == 0)
            return false;
        int dataID;
        dataID = data.notCollectedWeapons[Random.Range(0, data.notCollectedWeapons.Count)];
        data.collectedWeapons.Add(dataID);
        Weapon playerWep = CheckWeapons(dataID, collectedWeapons.Count);
        data.weaponSelected = collectedWeapons.Count;
        collectedWeapons.Add(playerWep);
        playerWep.enabled = true;
        playerWep.ChangeSprites();
        data.notCollectedWeapons.Remove(dataID);
        ChangeSwitchWeaponButtonImage();
        SaveData();
        return true;
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

    public void ChangeSwitchWeaponButtonImage() {
        switchWepImage.sprite = weaponSprite;
    }

    //if weapon is lastselected weapon before closing game enable else disable
    private bool IsSelectedWeapon(Weapon weapon) {
        if (weapon.weaponID == data.weaponSelected) {
            weapon.ChangeSprites();
            switchWepImage.sprite = weapon.GunSide;
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
        if(weapon.weaponLevel == weapon.weaponPrices.Count)
            return weapon.damagePoint[weapon.weaponLevel -1];
        return weapon.damagePoint[weapon.weaponLevel];
    }

    public float GetCurrentWeaponPushForce() {
        if (weapon.weaponLevel == weapon.weaponPrices.Count)
            return weapon.pushForce[weapon.weaponLevel-1];
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
        playerCurrentLevel += 1;
        player.SetLevelHealth(playerCurrentLevel);
        OnHealthChange();
    }
    
    //player health bar

    public void OnHealthChange() {
        float ratio = (float)player.health / player.maxHealth;
        healthBar.localScale = new Vector2(ratio, 1);
    }

    public void ShowPauseMenu() {
        pauseMenuAnimator.SetTrigger("Show");
        isPaused = true;
    }

    public void HidePauseMenu() {
        pauseMenuAnimator.SetTrigger("Hide");
        isPaused = false;
    }

    public void LevelComplete() {
        if (!isLevelCompleted) {
            isLevelCompleted = true;
            AudioController.instance.PlaySound(SoundClip.victory);
            //ShowText("Level Complete", 23, Color.blue, player.transform.position + (Vector3.up * 0.16f), Vector3.zero, 5f);
            ShowToastMessage("Level Complete!", 5f);
        }
       
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
        playerCurrentLevel = GetCurrentLevel() - 1;
        if (playerCurrentLevel > 1) {
            //player health based on experience, -1 because level 1 counts as well to increase health
            player.SetLevelHealth(playerCurrentLevel);
            //OnHealthChange(); 
        }
        ActivateEnemy();
    }

    public void UpdateEnemyKillText() {
        currentenemyKill += 1;
        if (currentenemyKill < 10)
            enemyKillText.text = "0" + currentenemyKill + "/" + totalEnemies;
        else enemyKillText.text = "" + currentenemyKill + "/" + totalEnemies;
    }

    public void ClearData() {
        SaveData();
    }

  /*  public void OnDestroy() {

        SaveData();
        PlayerPrefs.SetString("MySettingsEditor", "");
        PlayerPrefs.SetString("MySettings", "");
    }*/

   //enemy activation and opening doors if player completes the current level

    public void ActivateEnemy() {
        enemyActivator.ActivateFirstEnemyBatch();
    }

    public void OpenBarrier(int lockerID) {
        if(enemyActivator != null) {
            enemyActivator.OpenBarrier(lockerID);
        }
            
    }

    public void OpenDoor() {
        scenePortal.OpenDoor();
    }

    public void LoadLevel(string name) {
        SaveData();
        loadLevel.gameObject.SetActive(true);
        loadLevel.LoadLevel(name);
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
