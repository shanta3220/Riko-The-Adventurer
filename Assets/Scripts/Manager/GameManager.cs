using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    public static GameManager instance;

    //References

    public Player player;
    //public Weapon weapon;

    public int gold;
    public int experience;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //sceneloaded event is fires which call savestate
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    //floatingText;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.instance.Show(msg, fontSize, color, position, motion, duration);

    }


    /// <summary>
    /// int preferedSkin,
    /// int pesosAmount,
    /// int experience
    /// int weaponLevel
    /// </summary>
    //save state
    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += gold.ToString() + "|";
        s += experience.ToString();
        s += "0";//weaponLevel

        PlayerPrefs.SetString("Setting", s);

        Debug.Log("Save State");
        //PlayerPrefs.DeleteAll();
    }


    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("Setting"))
            return;
        string[] data = PlayerPrefs.GetString("Setting").Split('|');

        //Change PlayerScreen
        // amount of pessos
        gold = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        //change weapon level
        Debug.Log("Load State");
    }

    //Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        //is the weapon max level
        return false;
    }
}
