using UnityEngine;

public class ScenePortal : Collidable {

    public string[] sceneNames;
    protected override void OnCollide(Collider2D col) {
        if (col.tag =="Player") {
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
