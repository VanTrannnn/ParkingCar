using UnityEngine;

public class Park : MonoBehaviour
{
    public Route route;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem fx;
    private ParticleSystem.MainModule fxMainModule;
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Car car))
        {
            if(car.route == route)
            {
                Game.Instance.onCarEntersPark?.Invoke(route);
            }
        }
    }
    private void StartFX()
    {
        fxMainModule.startColor = route.carColor;
        fx.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
