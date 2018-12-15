﻿using System;  // give access to the [Serializable] Attribute
using System.Collections.Generic;

/// <summary>
/// the GameData for your Model.
/// </summary>

[Serializable]// this attribute allows to save data inside this class

public class GameData{

    public int gold = 10000;
    public int experience;
    public List<int> notCollectedWeapons = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
    public List<int> collectedWeapons = new List<int>() {0};
    public int weaponSelected;
    public int selectedSkin;
    public bool[] skins = { true, false, false };
    public List<int> weaponLevel = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
   
   
}