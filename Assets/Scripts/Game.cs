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
    public UnityAction onAllCarsMove;
    [SerializeField] AudioSource carColliderAudio;
    private bool isGameOver = false;
    private bool isLevelComplete = false;
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
        isGameOver = false;
        isLevelComplete = false;

        onCarEntersPark += OnCarEntersParkHandler;
        onCarCollision += OnCarCollisionHandler;
    }
    private void OnCarCollisionHandler()
    {
        // Prevent multiple calls when multiple cars collide at the same time
        if (isGameOver || isLevelComplete) return;
        
        isGameOver = true;
        Debug.Log("game over");
        AudioManager.InstanceAudio.carColliderSound(carColliderAudio.clip);
        DOVirtual.DelayedCall(2f, () =>
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        });
    }
    private void OnCarEntersParkHandler(Route route)
    {
        // Prevent processing if game is already over or level is complete
        if (isGameOver || isLevelComplete) return;
        
        route.car.StopDancingAnim();
        successfulParks++;
        if(successfulParks == totalRoutes)
        {
            // Prevent multiple calls if multiple cars finish at the same time
            if (isLevelComplete) return;
            
            isLevelComplete = true;
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
        onAllCarsMove?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
