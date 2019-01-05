using UnityEngine;
using System;
using UnityEngine.UI;
[Serializable]
public class UI {
    public Animator notificationAnimator;
    public Animator characterFocusAnimator;
    public Animator quitPanelAnimator;
    public Text goldText;
    public Text btnCharacterSelectorText;
    public GameObject joyStickPanel;
   
}

public class MenuController : MonoBehaviour {
    public GameData data;
    public UI ui;
    public CharacterSelectionPlayer[] characterSelectPlayer;
 
    public int gold;
    public int selectedSkin;
    public Transform lastSelecterPlayer;
    public static MenuController instance;

    private Camera cam;
    private CharacterSelectionCamera CharSelectCamera;
    private bool isNotificationShown = true;
    private int currentCharacterFocus;
    private int[] skinPrices =  { 0, 300, 1000 };
    public bool isOnPanel;
    public bool isOnPc;
    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        data = DataController.instance.data;
        cam = Camera.main;
        CharSelectCamera = cam.GetComponent<CharacterSelectionCamera>();
        LoadData();
        ui.notificationAnimator.SetBool("ShowPanel", isNotificationShown);
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            isOnPc = true;
        else if (Application.platform == RuntimePlatform.Android) {
            isOnPc = false;
        }
           
    }

    private void Update() {
        if (isOnPc) {
            if (Input.GetMouseButtonDown(0)) {
                if (!isOnPanel) {
                    ChoosePlayer(Input.mousePosition);
                }
            }
        }

        else if (!isOnPc) {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                ChoosePlayer(Input.GetTouch(0).position);
            }

        }
       
      
        if (Input.GetButtonDown("Cancel")) {
            ShowQuitPanel();
        }
    }

    private void LoadData() {
        gold = data.gold;
        ui.goldText.text = "Total Gold: " + gold;
    }

    public void SaveData() {
        data.gold = gold;
        data.selectedSkin = selectedSkin;
        DataController.instance.SaveData(data);
    }

    public void ChoosePlayer(Vector3 pos) {
        Collider2D hitCollider = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(pos));
        /* if (isPC)
             hitCollider = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition));
         else {
             RaycastHit2D hit = Physics2D.Raycast(new Vector2(cam.ScreenToWorldPoint(pos).x, cam.ScreenToWorldPoint(pos).y), Vector2.zero, 0);
             hitCollider = hit.collider;
         }*/
        if (hitCollider != null && hitCollider.CompareTag("Player")) {
            if(hitCollider.name == "player_1") {
                currentCharacterFocus = 0;
                SwitchCharacter();
            }
            else if (hitCollider.name == "player_2") {
                currentCharacterFocus = 1;
                SwitchCharacter();
            }

            else if (hitCollider.name == "player_3") {
                currentCharacterFocus = 2;
                SwitchCharacter();
            }
   
            ui.characterFocusAnimator.SetTrigger("Show");
            if (isNotificationShown) {
                isNotificationShown = false;
                ui.notificationAnimator.SetBool("ShowPanel", isNotificationShown);
            }
            isOnPanel = true;
            if (!isOnPc)
                ui.joyStickPanel.SetActive(false);
        }
    }



    //CharacterSelection
    public void OnArrowClick(bool right) {
        if (right) {
            currentCharacterFocus++;
            //if no more character
            if (characterSelectPlayer.Length == currentCharacterFocus)
                currentCharacterFocus = 0;
        }
        else {
            currentCharacterFocus--;
            if (currentCharacterFocus == -1) {
                currentCharacterFocus = 2;
            }
        }
        SwitchCharacter();

    }
 
    public void SwitchCharacter(){
        if (currentCharacterFocus == 0) {
            ui.btnCharacterSelectorText.text = "Selected";
            selectedSkin = currentCharacterFocus;
            lastSelecterPlayer = characterSelectPlayer[currentCharacterFocus].transform;
            characterSelectPlayer[currentCharacterFocus].enabled = true;
            characterSelectPlayer[1].enabled = false;
            characterSelectPlayer[2].enabled = false;
            characterSelectPlayer[currentCharacterFocus].isOnPanel = true;
        }
        else if (currentCharacterFocus == 1) {
            if (data.skins[currentCharacterFocus]) {
                ui.btnCharacterSelectorText.text = "Selected";
                selectedSkin = currentCharacterFocus;
                lastSelecterPlayer = characterSelectPlayer[currentCharacterFocus].transform;
                characterSelectPlayer[currentCharacterFocus].enabled = true;
                characterSelectPlayer[0].enabled = false;
                characterSelectPlayer[2].enabled = false;
                characterSelectPlayer[currentCharacterFocus].isOnPanel = true;
            }
            else {
                ui.btnCharacterSelectorText.text = skinPrices[currentCharacterFocus].ToString();
            }
        }
        else if(currentCharacterFocus == 2) {
            if (data.skins[currentCharacterFocus]) {
                ui.btnCharacterSelectorText.text = "Selected";
                selectedSkin = currentCharacterFocus;
                lastSelecterPlayer = characterSelectPlayer[currentCharacterFocus].transform;
                characterSelectPlayer[currentCharacterFocus].enabled = true;
                characterSelectPlayer[0].enabled = false;
                characterSelectPlayer[1].enabled = false;
                characterSelectPlayer[currentCharacterFocus].isOnPanel = true;
            }
            else {
                ui.btnCharacterSelectorText.text = skinPrices[currentCharacterFocus].ToString();
            }
        }
        CharSelectCamera.OnFocusPlayer(characterSelectPlayer[currentCharacterFocus].transform);

    }

    public void UnlockOrSelectSkin() {
        if (TryUnlockSkin(currentCharacterFocus, skinPrices[currentCharacterFocus])) {
            ui.btnCharacterSelectorText.text = "Selected";
            ui.goldText.text = "Total Gold: " + gold;
        }
        else ui.btnCharacterSelectorText.text = skinPrices[currentCharacterFocus].ToString();

    }
    //Unlock Characters
    private bool TryUnlockSkin(int skinNumber, int skinPrice) {
        if (data.skins[skinNumber])
            return true;
        //do we have enough gold? if so upgrade and decrement the weaponprice from the gold
        if (gold >= skinPrice) {
            gold -= skinPrice;
            data.skins[skinNumber] = true;
            data.selectedSkin = skinNumber;
            return true;
        }
        return false;
    }

    public void GoBackFromCharacterSelectorMenu() {
        if (lastSelecterPlayer != null) {
            //player selected a skin  and we reposition the camera
            CharSelectCamera.OutFocusPlayer(lastSelecterPlayer);
        }
        else {
            CharSelectCamera.OutFocusPlayer(transform);
            isNotificationShown = true;
            ui.notificationAnimator.SetBool("ShowPanel", isNotificationShown);
        }
        ui.characterFocusAnimator.SetTrigger("Hide");
        isOnPanel = false;
        characterSelectPlayer[currentCharacterFocus].isOnPanel = false;
        if(!isOnPc)
            ui.joyStickPanel.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ShowQuitPanel() {
        if(!ui.quitPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("DeathMenu_showing"))
            ui.quitPanelAnimator.SetTrigger("Show");
    }
/*
    private void CheckForInput() {

       if (Application.platform == RuntimePlatform.Android) {
           if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) {
               RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y), Vector2.zero, 0);
               if (raycastHit && raycastHit.collider.CompareTag("Player")) {
                   Debug.Log("Player");
               }

           }

       }


       else if (Application.platform == RuntimePlatform.WindowsEditor) {
           if (Input.GetMouseButtonDown(0)) {
               Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
               if (hitCollider != null && hitCollider.CompareTag("Player")) {
                   Debug.Log("This is Player");
               }
           }
       }

   }*/

}
