using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource dogSound;
    [SerializeField] AudioClip dogCrySound;

    private Tween moveTween;
    private bool isHit;

    public void SetMoveTween(Tween tween)
    {
        moveTween = tween;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;

        if (collision.transform.TryGetComponent(out Car otherCar))
        {
            isHit = true;
            moveTween?.Pause();

            Vector3 hitPoint = collision.contacts[0].point;
            AddExplosionForce(hitPoint);
        }
    }

    private void AddExplosionForce(Vector3 point)
    {
        AfterDogCollision();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddExplosionForce(30f, point, 1f, 0.1f, ForceMode.Impulse);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 6f);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        Destroy(gameObject, 2f);
    }

    private void OnDestroy()
    {
        // Kill all tweens on this object to prevent errors when object is destroyed
        moveTween?.Kill();
        transform.DOKill();
    }

    private void AfterDogCollision()
    {
        dogSound.clip = dogCrySound;
        dogSound.loop = false;
        dogSound.Play();
    }
}
