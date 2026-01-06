using Unity.Jobs;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager InstanceAudio;
    [SerializeField] AudioSource carCollider;
    [SerializeField] AudioSource backgroundMusic;
    public bool isMuted = false;
    private void Awake()
    {
        if (InstanceAudio != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceAudio = this;
        DontDestroyOnLoad(gameObject);
    }

    public void carColliderSound(AudioClip clip)
    {
        carCollider.volume = backgroundMusic.volume;
        carCollider.PlayOneShot(clip);
    }
    public void toggleOnOffSound()
    {
        isMuted = !isMuted;
        backgroundMusic.volume = isMuted ? 0f : .4f;
    }
    
}
