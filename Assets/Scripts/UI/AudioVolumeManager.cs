using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeManager : MonoBehaviour
{
    private const string MUSIC_VOLUME_PARAMETER_NAME = "MusicVolume";
    private const string SFX_VOLUME_PARAMETER_NAME = "SFXVolume";

    [SerializeField] private AudioMixer mainMixer = null;

    public void SetMusicVolume(float _volumeToSet)
    {
        mainMixer.SetFloat(MUSIC_VOLUME_PARAMETER_NAME, Mathf.Log10(_volumeToSet) * 20);
    }

    public void SetSFXVolume(float _volumeToSet)
    {
        mainMixer.SetFloat(SFX_VOLUME_PARAMETER_NAME, Mathf.Log10(_volumeToSet) * 20);
    }
}
