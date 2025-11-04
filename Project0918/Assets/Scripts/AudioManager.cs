/**
* @file AudioManager.cs
* @author Ryder
* @brief Stores all audio so it can be referenced and called from a source.
*/

using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
    }

    [Header("Audio Settings")]
    // public List<Sound> music = new List<Sound>();
    public List<Sound> sounds = new List<Sound>();


    private Dictionary<string, Sound> soundDict;
    [SerializeField] private AudioSource mainSource;  // usually player source

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        soundDict = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            if (s.clip == null) continue;
            if (!soundDict.ContainsKey(s.name))
                soundDict.Add(s.name, s);
        }

        mainSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        GameObject go = GameObject.FindWithTag("Player");

        if (go != null)
            transform.position = go.transform.position;
    }

    public void Play(string name)
    {
        if (!soundDict.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"AudioManager: Sound '{name}' not found!");
            return;
        }

        mainSource.pitch = s.pitch;
        mainSource.PlayOneShot(s.clip, s.volume);

        mainSource.clip = s.clip;
        mainSource.Play();

    }

    public void Play(string name, AudioSource source)
    {
        if (!soundDict.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"AudioManager: Sound '{name}' not found!");
            return;
        }

        source.clip = s.clip;
        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.Play();
    }

    public void Stop(AudioSource source)
    {
        if (source.isPlaying)
            source.Stop();
    }

    public AudioClip GetClip(string name)
    {
        return soundDict.TryGetValue(name, out Sound s) ? s.clip : null;
    }
}
