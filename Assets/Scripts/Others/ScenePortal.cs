using UnityEngine;

public class ScenePortal : Collidable {

    
    public string[] sceneNames;
    public Sprite doorLeafOpen;
    private bool canChangeScene;
    private delegate void LoadLevel(string sceneName);
    LoadLevel loadLevel;

    protected override void Start() {
        base.Start();
        if (GameManager.instance != null)
            loadLevel = GameManager.instance.LoadLevel;
        else loadLevel = MenuController.instance.LoadLevel;

    }
    public void OpenDoor() {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = doorLeafOpen;
        AudioController.instance.PlaySound(SoundClip.gateOpen);
        canChangeScene = true;
    }

    protected override void OnCollide(Collider2D col) {
        if (!canChangeScene)
            return;
        if (col.tag =="Player") {
        
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            if (DataController.instance.data.experience < 8)
                sceneName = "Dungeon_1";
            canChangeScene = false;
            loadLevel(sceneName);
        }
    }
}
