using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public delegate void OnVolumeChange(SoundType soundType, float volume);
    public OnVolumeChange onVolumeChange;

    public void ChangeVolume(SoundType soundType, float volume)
    {
        switch (soundType)
        {
            case SoundType.Music: PlayerPrefs.SetFloat("MusicVolume", volume); break;
            case SoundType.Sound: PlayerPrefs.SetFloat("SoundVolume", volume); break;
        }

        onVolumeChange?.Invoke(soundType, volume);
    }

    public float GetVolume(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Music: return PlayerPrefs.GetFloat("MusicVolume", 0.75f);

            default:
            case SoundType.Sound: return PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        }
    }
}
