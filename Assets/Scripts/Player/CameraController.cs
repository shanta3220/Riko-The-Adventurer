using UnityEngine;

public class CameraController : MonoBehaviour {

  
    public Transform lookAt;
    public float boundX=0.3f, boundY = 0.15f;
    public float cameraSmoothing = 0.05f;
    Vector3 targetPosition;
   // private Vector3 velocity = Vector3.zero;
    //public float dampTime  = 15;

    private void FixedUpdate() {
        Vector3 delta = Vector3.zero;
        //this is to check if we are inside the bounds on the x axis
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX) {
            if (transform.position.x < lookAt.position.x) {
                //moving right
                delta.x = deltaX - boundX;
            }
            else {
                //moving left
                delta.x = deltaX + boundX;
            }
        }
        //this is to check if we are inside the bounds on the y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY) {
            if (transform.position.y < lookAt.position.y) {
                delta.y = deltaY - boundY;
            }
            else {
                delta.y = deltaY + boundY;
            }
        }

        Vector3 newPosition = new Vector3(delta.x, delta.y, 0f);
       
        transform.position += newPosition;
        //targetPosition = transform.position + newPosition;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothing * Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime * Time.deltaTime);

    }


    /*public float pixelPerUnit = 40f;

    public Vector3 PixelPerfectClamp(Vector3 moveVector) {
        Vector3 vectorInPixels = new Vector3(
           Mathf.Round(moveVector.x * pixelPerUnit) / pixelPerUnit,
           Mathf.Round(moveVector.y * pixelPerUnit) / pixelPerUnit, moveVector.z);
        return vectorInPixels;
    }*/
}
