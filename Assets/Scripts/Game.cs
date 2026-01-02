using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class Game : MonoBehaviour
{
    public static Game Instance;
    [HideInInspector] public List<Route> readyRoutes = new();
    private int totalRoutes;
    private int successfulParks;
    public UnityAction<Route> onCarEntersPark;
    public UnityAction onCarCollision;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;

        onCarEntersPark += OnCarEntersParkHandler;
        onCarCollision += OnCarCollisionHandler;
    }
    private void OnCarCollisionHandler()
    {
        Debug.Log("game over");
        DOVirtual.DelayedCall(2f, () =>
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        });
    }
    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnim();
        successfulParks++;
        if(successfulParks == totalRoutes)
        {
            Debug.Log("win");
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            PlayerPrefs.SetInt("highestLevel", nextLevel);
            PlayerPrefs.Save();
            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (nextLevel < SceneManager.sceneCountInBuildSettings)
                    SceneManager.LoadScene(nextLevel);
                else
                    Debug.Log("No next level to load");
            });
        }
    }
    public void RegisterRoute(Route route)
    {
        readyRoutes.Add(route);
        if (readyRoutes.Count == totalRoutes)
            MoveAllCars();
    }
    public void MoveAllCars()
    {
        foreach (var route in readyRoutes)
            route.car.Move(route.linePoints);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
