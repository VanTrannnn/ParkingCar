using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public class LineDrawer : MonoBehaviour
{
    [SerializeField] UserInput userInput;
    [SerializeField] int interactableLayer;
    private Line currentLine;
    private Route currentRoute;

    RaycastDetector raycastDetector = new();
    public UnityAction<Route> onBeginDraw;
    public UnityAction onDraw;
    public UnityAction onEndDraw;


    public UnityAction<Route, List<Vector3>> onParkLinkedToLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userInput.onMouseDown += OnMouseDownHandler;
        userInput.onMouseMove += OnMouseMoveHandler;
        userInput.onMouseUp += OnMouseUpHandler;
    }

    private void OnMouseDownHandler()
    {
        ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);
        if (contactInfo.contacted)
        {
            bool isCar = contactInfo.collider.TryGetComponent(out Car _car);
            if (isCar && _car.route.isActive)
            {
                currentRoute = _car.route;
                currentLine = currentRoute.line;
                currentLine.Init();

                onBeginDraw?.Invoke(currentRoute);
            }
        }
    }
    private void OnMouseMoveHandler()
    {
        if (currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);
            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;
                if (currentLine.length >= currentRoute.maxLineLength) {
                    currentLine.Clear();
                    OnMouseUpHandler();
                    return;
                }
                currentLine.AddPoint(newPoint);
                onDraw?.Invoke();
                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);
                if (isPark)
                {
                    Route parkRoute = _park.route;
                    if(parkRoute == currentRoute)
                    {
                        currentLine.AddPoint(contactInfo.transform.position);
                        onDraw?.Invoke();
                    }
                    else
                    {
                        currentLine.Clear();
                    }
                    OnMouseUpHandler();
                }
            }
        }
    }
    private void OnMouseUpHandler()
    {
        if (currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);
            if (contactInfo.contacted)
            {
                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);
                if (currentLine.pointsCount < 2 || !isPark)
                {
                    //delete line
                    currentLine.Clear();
                }
                else
                {
                    onParkLinkedToLine?.Invoke(currentRoute, currentLine.points);
                    currentRoute.Disactive();
                }
            }
            else
            {
                currentLine.Clear();
            }
        }
        ResetDrawner();
        onEndDraw?.Invoke();
    }
    private void ResetDrawner()
    {
        currentLine = null;
        currentRoute = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
