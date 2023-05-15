using DG.Tweening;
using UnityEngine;

public class ElevatorAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource moveSFXSource = null;
    [SerializeField] private AudioSource dingSFXSource = null;
    [SerializeField] private AudioSource musicSource = null;

    [SerializeField] private float audioFadeDuration = 1f;

    public void PlayMusic()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        musicSource.Play();
        musicSource.volume = 1;
    }

    public void StopMusic()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        musicSource.DOFade(0, audioFadeDuration).OnComplete(()=> musicSource.Stop());
    }

    public void PlayElevatorMoveSFX()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        moveSFXSource.Play();
        moveSFXSource.volume = 0;
        moveSFXSource.DOFade(1, audioFadeDuration);
    }

    public void StopElevatorMoveSFX()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        moveSFXSource.DOFade(0, audioFadeDuration).OnComplete(() => moveSFXSource.Stop());
    }

    public void PlayElevatorDingSFX()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        dingSFXSource.Play();
    }

    private bool isAnyRequiredComponentNull()
    {
        if (moveSFXSource == null || musicSource == null || dingSFXSource == null)
        {
            Debug.LogError("ElevatorAudioHandler :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
