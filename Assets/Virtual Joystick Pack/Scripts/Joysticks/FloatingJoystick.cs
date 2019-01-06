using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector2 joystickCenter = Vector2.zero;
    public GameObject animArrow;
    private Vector2 intialPos;

    void Start()
    {
        //background.gameObject.SetActive(false);
        intialPos = background.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.SetParent(background.transform);
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
        if (animArrow != null)
            Destroy(animArrow);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
       // background.gameObject.SetActive(false);
        inputVector = Vector2.zero;
        background.position = intialPos;
        handle.localPosition = inputVector;
    }
}