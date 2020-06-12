using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _instance;
    public static SoundController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SoundController").AddComponent<SoundController>();
                _instance.Init();
            }
            return _instance;
        }
        set => _instance = value;
    }

    private Dictionary<string, AudioClip> _sounds = new Dictionary<string, AudioClip>();
    private AudioSource _audioSource;

    private void Init()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
        foreach (var clip in clips)
        {
            _sounds.Add(clip.name, clip);
        }

        _audioSource = _instance.gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlaySound(string soundName)
    {
        if (!_sounds.ContainsKey(soundName)) {Debug.LogError("ERROR Sound not found"); return;}
        
        AudioClip clip = _sounds[soundName];
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopSound(string soundName = "")
    {
        if (string.IsNullOrEmpty(soundName))
        {
            _audioSource.Stop();
        }
    }
}
