using UnityEngine;

public class SceneLoader : MonoBehaviour {

    public SceneLoadingBarController sceneLoadingBarController;

	private void Start () {
        sceneLoadingBarController.LoadLevel("MainMenu");
    }
	
	
}
