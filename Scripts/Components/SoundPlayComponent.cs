using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayComponent : MonoBehaviour
{
    private AudioSource audioSource;

    public SoundClip[] clips = new SoundClip[0];
    private Dictionary<string, SoundClip> dict = new Dictionary<string, SoundClip>();

    [System.Serializable]
    public class SoundClip : DictionaryItem
    {
        public AudioClip audioClip;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        dict = clips.ArrayToDictionary();
    }

    public void PlayAudioClip(string id, bool playIfPlaying = true)
    {
        if (!playIfPlaying && audioSource.isPlaying) return;

        if(dict.ContainsKey(id))
        {
            audioSource.clip = dict[id].audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log(string.Format("Dictionary is missing key: {0}", id));
        }

    }
}
