using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    public Route route;
    public Transform bottomTransform;
    public Transform bodyTransform;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem smokeFX;
    [SerializeField] Rigidbody rb;
    [SerializeField] float danceValue;
    [SerializeField] float durationMultiplier;
    private bool hasCollided = false;
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bodyTransform.DOLocalMoveY(danceValue, .15f)
            .SetLoops(-1,LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple collision events from the same car
        if (hasCollided) return;
        
        if(collision.transform.TryGetComponent(out Car otherCar) || collision.gameObject.CompareTag("block"))
        {
            hasCollided = true;
            StopDancingAnim();
            rb.DOKill(false);
            Vector3 hitPoint = collision.contacts[0].point;
            AddExplosionForce(hitPoint);
            smokeFX.Play();
            Game.Instance.onCarCollision?.Invoke();
        }
    }
    private void AddExplosionForce(Vector3 point)
    {
        rb.AddExplosionForce(400f, point, 3f);
        rb.AddForceAtPosition(Vector3.up * 2f, point, ForceMode.Impulse);
        rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
    }
    private float GetRandomAngle()
    {
        float angle = 10f;
        float rand = Random.value;
        return rand > .5f ? angle : -angle;
    }
    public void Move(Vector3[] path)
    {
        rb.DOLocalPath(path, 2f * durationMultiplier * path.Length)
            .SetLookAt(.1f, false)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }
    public void StopDancingAnim()
    {
        bodyTransform.DOKill(true);
    }

    private void OnDestroy()
    {
        // Kill all tweens on this object to prevent errors when object is destroyed
        bodyTransform.DOKill();
        rb.DOKill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
