using System;  // give access to the [Serializable] Attribute
using System.Collections.Generic;

/// <summary>
/// the GameData for your Model.
/// </summary>

[Serializable]// this attribute allows to save data inside this class

public class GameData{

    public int gold;
    public int experience;
    public List<Weapon> notCollectedWeapons;
    public List<Weapon> collectedWeapons;
    public int weaponSelected;
    public int selectedSkin;
    public bool[] skins = { true, false, false };
    public List<int> weaponLevel = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
   
   
}
