using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneLoadingBarController : MonoBehaviour {

    public Slider progressBar;
    public Text progressText;

   private IEnumerator LoadAsynchronously(string sceneName) {
        gameObject.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            progressBar.value = progress;
            progressText.text = (int)(progress * 100 )+ "%";
            yield return null;
        }
    }


    public void LoadLevel(string sceneName) {
        StartCoroutine(LoadAsynchronously(sceneName));
    }


}
