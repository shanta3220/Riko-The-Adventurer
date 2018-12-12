using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    //Resources
    public List<Sprite> playerSprites;
    // public List<Sprite> weaponSprites;
    public Sprite weaponSprite;
    //public List<int> weaponPrices;
    public List<int> xpTable;
    public static GameManager instance;

    //References

    public Player player;
    public Weapon weapon;

    public int gold;
    public int experience;
    [HideInInspector]


    private void Awake()
    {
        if (instance == null)
            instance = this;
        DataController.instance.RefreshData();

    }

    private void Start() {
       
        LoadData();
        //weapon = player.weapon;
    }

    //floatingText;
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.instance.Show(msg, fontSize, color, position, motion, duration);

    }

    //Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        weapon = player.weapon;
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

    public void SaveData() {

        DataController.instance.data.gold = gold;
        DataController.instance.data.experience = experience;
        DataController.instance.data.weaponSelected = weapon.weaponID;
        DataController.instance.SaveData();
    }

    public void LoadData() {
        GameData data = DataController.instance.data;
        gold = data.gold;
        experience = data.experience;
        int i = 0;
        //checking collected weapons
        foreach(Weapon wp in data.collectedWeapons) {
            wp.weaponID = i;
            Weapon wep = player.weaponContainer.AddComponent(wp.GetType()) as Weapon;
            CloneWeapon(wp, wep);
            i++;
        }
        //setting the default weapon
        if(data.weaponSelected == 0) {
            player.weaponContainer.GetComponent<FlameThrower>().enabled = true;
        }
    }

    public void UnlockWeapon() {
        GameData data = DataController.instance.data;
        Weapon wep = data.notCollectedWeapons[Random.Range(0, data.notCollectedWeapons.Count)];
        wep.enabled = true;
        data.collectedWeapons.Add(wep);
        data.notCollectedWeapons.Remove(wep);
        wep.weaponID = data.collectedWeapons.Count - 1;
        data.collectedWeapons[data.collectedWeapons.Count - 1].weaponID = data.collectedWeapons.Count - 1;
        //adding new weapon
        data.weaponSelected = wep.weaponID;
        Weapon playerWep = player.weaponContainer.AddComponent(wep.GetType()) as Weapon;
        CloneWeapon(wep, playerWep);
        SaveData();
    }

    public void SwitchWeapon() {
        GameData data = DataController.instance.data;
        if (weapon.weaponID == data.collectedWeapons.Count - 1) {
            data.weaponSelected = 0;
            Weapon wep = player.weaponContainer.GetComponent(data.collectedWeapons[0].GetType()) as Weapon;
            wep.enabled = true;
        }
        else {
            data.weaponSelected = weapon.weaponID + 1;
            Weapon wep = player.weaponContainer.GetComponent(data.collectedWeapons[weapon.weaponID + 1].GetType()) as Weapon;
            wep.enabled = true;
        }
    }
    public void OnDestroy() {
        SaveData();
    }

    private void CloneWeapon(Weapon originalWeapon, Weapon cloneWeapon) {
        GameData data = DataController.instance.data;
        originalWeapon.SetSettings(cloneWeapon);
        originalWeapon.enabled = false;
        if (cloneWeapon.weaponID == data.weaponSelected) {
            cloneWeapon.enabled = true;
            cloneWeapon.ChangeSprites();
        }
        else cloneWeapon.enabled = false;

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
