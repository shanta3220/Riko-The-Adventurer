using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionCamera : MonoBehaviour {
    private bool onFocus;
    private bool isStartFocus;
    private bool isDoneAnimating;

    public bool hasTarget;
    public Transform lookAt;
    public float boundX = 0.3f, boundY = 0.15f;
    private Vector3 targetPosition;
    public float cameraSmoothing = 10f;
    private Transform permanentLookAt;
 
	private void Update () {
        if (!hasTarget)
            return;
        targetPosition = new Vector3(lookAt.position.x, lookAt.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothing * Time.deltaTime);
        if (!isDoneAnimating) {
            if (isStartFocus) {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 1f, cameraSmoothing/2 * Time.deltaTime);
                if (Camera.main.orthographicSize == 1f) {
                    isDoneAnimating = true;
                    isStartFocus = false;
                }
                   
            }
            if (onFocus) {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 0.3f, cameraSmoothing * Time.deltaTime);
                if (Camera.main.orthographicSize == 0.3f)
                    isDoneAnimating = true;

            }
            else {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 1f, cameraSmoothing * Time.deltaTime);
                if (Camera.main.orthographicSize == 0.3f)
                    isDoneAnimating = false;
            }
        }
        
    }

    public void OnFocusPlayer(Transform player) {
        lookAt = player;
        hasTarget = true;
        onFocus = true;
        isDoneAnimating = false;
    }

    public void OutFocusPlayer(Transform player) {
        if (permanentLookAt == null) {
            SetPermanentLookAt(player);
        }
        lookAt = player;
        hasTarget = true;
        onFocus = false;
        isDoneAnimating = false;
    }

    public void SetPermanentLookAt(Transform lookAt) {
        permanentLookAt = lookAt;
    }

    public void ResetLookAt() {
        lookAt = permanentLookAt;
    }

    public void StartFocusFromGameTitle() {
        lookAt = MenuController.instance.transform;
        hasTarget = true;
        isStartFocus = true;
    }
}
