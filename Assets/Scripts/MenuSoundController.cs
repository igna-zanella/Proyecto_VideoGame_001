using UnityEngine;
using UnityEngine.Audio;

public class MenuSoundController : MonoBehaviour
{

    [SerializeField]private AudioMixer audioMixer;

    public void VolumenGeneral(float nivel)
    {
        audioMixer.SetFloat("VolumenGeneral", nivel);
    }
    public void VolumenEfectos(float nivel)
    {
        audioMixer.SetFloat("VolumenEfectos", nivel);
    } 
    public void VolumenMusica(float nivel)
    {
        audioMixer.SetFloat("VolumenMusica", nivel);
    }

}
