using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeComponent : MonoBehaviour
{
    private AudioSource audioSource;

    public SoundType soundType;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        VolumeController volumenController = GameObject.Find("GameController").GetComponent<VolumeController>();
        volumenController.onVolumeChange += OnVolumeChange;

        OnVolumeChange(soundType, volumenController.GetVolume(soundType));
    }

    public void OnVolumeChange(SoundType soundType, float volume)
    {
        if(this.soundType == soundType)
        {
            audioSource.volume = volume;
        }
    }
}
