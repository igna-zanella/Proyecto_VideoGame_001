using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; } = null;
    private void Awake()
    {
        if (Instance != null && Instance != this )
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


}
