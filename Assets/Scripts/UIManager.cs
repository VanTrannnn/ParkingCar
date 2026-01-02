using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] LineDrawer lineDrawer;

    [Space]
    [SerializeField] private CanvasGroup availableCanvasGroup;
    [SerializeField] private GameObject availableLineHolder;
    [SerializeField] private Image availableFill;
    private bool isAvailableLineUIActive = false;
    [Space]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration;

    private Route activeRoute;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadePanel.DOFade(0f, fadeDuration).From(1f);

        availableCanvasGroup.alpha = 0f;
        lineDrawer.onBeginDraw += OnBeginDrawHandler;
        lineDrawer.onDraw += OnDrawHandler;
        lineDrawer.onEndDraw += OnEndDrawHandler;
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;

        availableFill.color = activeRoute.carColor;
        availableFill.fillAmount = 1f;
        availableCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvailableLineUIActive = true;
    }

    private void OnDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            availableFill.fillAmount = 1 - (lineLength/maxLineLength);
        }
    }

    
    private void OnEndDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;
            availableCanvasGroup.DOFade(0f, .3f).From(1f);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
