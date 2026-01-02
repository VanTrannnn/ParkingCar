using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

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



    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bodyTransform.DOLocalMoveY(danceValue, .15f)
            .SetLoops(-1,LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Car otherCar))
        {
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
            .SetEase(Ease.Linear);
    }
    public void StopDancingAnim()
    {
        bodyTransform.DOKill(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
