using UnityEngine;
using UnityEngine.Events;

public class UserInput : MonoBehaviour
{
    public UnityAction onMouseDown;
    public UnityAction onMouseMove;
    public UnityAction onMouseUp;

    private bool isMouseDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            onMouseDown?.Invoke();
        }
        if (isMouseDown)
            onMouseMove?.Invoke();

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            onMouseDown?.Invoke();
        }

        //mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                onMouseDown?.Invoke();
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                onMouseMove?.Invoke();
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                onMouseUp?.Invoke();
        }
    }
}
