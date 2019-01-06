using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCallerMobile : MonoBehaviour {
    public Player player;


    private void Start() {
    }

    public void Restart() {
        GameManager.instance.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SwitchWeapon() {
        GameManager.instance.SwitchWeapon();
        GameManager.instance.ChangeSwitchWeaponButtonImage();
        GameManager.instance.SaveData();
    }
    //Death Menu And Respawn

    public void Respawn() {
        GameManager.instance.SaveData();
        GameManager.instance.panelLoading.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }

    public void Shoot() {
        player.Shoot();
    }

}
