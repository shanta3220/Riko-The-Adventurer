using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour {

    public static DataController instance = null;
    public GameData data;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else {
            Destroy(gameObject);
        }
        RefreshData();
    }

    public void RefreshData() {
        if (Application.isEditor) {
            string jsonData = PlayerPrefs.GetString("MySettingsEditor");
            JsonUtility.FromJsonOverwrite(jsonData, data);
        }
        else {
            string jsonData = PlayerPrefs.GetString("MySettings");
            JsonUtility.FromJsonOverwrite(jsonData, data);
        }
    }

    public void SaveData() {
        if (Application.isEditor) {
            //Convert to Json
            string jsonData = JsonUtility.ToJson(data);
            //Save Json string
            PlayerPrefs.SetString("MySettingsEditor", jsonData);
            PlayerPrefs.Save();
        }
        else {
            //Convert to Json
            string jsonData = JsonUtility.ToJson(data);
            //Save Json string
            PlayerPrefs.SetString("MySettings", jsonData);
            PlayerPrefs.Save();
        }
    }

    public void SaveData(GameData data) {
        if (Application.isEditor) {
            //Convert to Json
            string jsonData = JsonUtility.ToJson(data);
            //Save Json string
            PlayerPrefs.SetString("MySettingsEditor", jsonData);
            PlayerPrefs.Save();
        }
        else {
            //Convert to Json
            string jsonData = JsonUtility.ToJson(data);
            //Save Json string
            PlayerPrefs.SetString("MySettings", jsonData);
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationQuit() {
        //SaveData();
    }

    

}
