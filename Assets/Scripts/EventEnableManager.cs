using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// responsble to enable EventCaller if on Desktop it calls EventCallerPC or else EventCallerMobile
/// </summary>
public class EventEnableManager : MonoBehaviour {
    public GameObject HudMobile;
	void Start () {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            GetComponent<EventCallerPC>().enabled = true;
            HudMobile.SetActive(false);
        }
            
        else if (Application.platform == RuntimePlatform.Android) {
            GetComponent<EventCallerMobile>().enabled = true;
            HudMobile.SetActive(true);
        }
            
    }
	
}
