using UnityEngine;

public class SoundFXController : MonoBehaviour
{
    public static SoundFXController Instance { get; private set; } = null;
    [SerializeField] private AudioSource prefabAudioSource;
    [SerializeField] private AudioClip clipJugadorSalto;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void JugadorSalto(Transform transformObject)
    {
        AudioSource tempAudioSource = Instantiate(prefabAudioSource, transformObject.position, Quaternion.identity);
        tempAudioSource.clip = clipJugadorSalto;
        tempAudioSource.Play();
        Destroy(tempAudioSource.gameObject, tempAudioSource.clip.length);
    }
}
