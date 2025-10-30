/**
* @file AudioManager.cs
* @author Ryder
* @brief Stores all audio so it can be referenced and called from a source.
*/

using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

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
    public List<Sound> music = new List<Sound>();
    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, Sound> musicDict;
    private Dictionary<string, Sound> soundDict;
    private AudioSource musicSource;
    [SerializeField] private AudioSource mainSource;  // usually player source

    private Coroutine fadeOutCoroutine;

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

        musicDict = new Dictionary<string, Sound>();
        foreach (Sound m in music)
        {
            if (m.clip == null) continue;
            if (!musicDict.ContainsKey(m.name))
                musicDict.Add(m.name, m);
        }

        soundDict = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            if (s.clip == null) continue;
            if (!soundDict.ContainsKey(s.name))
                soundDict.Add(s.name, s);
        }

        musicSource = GetComponent<AudioSource>();
        mainSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        GameObject go = GameObject.FindWithTag("Player");

        if (go != null)
            transform.position = go.transform.position;
    }

    public void PlayMusic(string name)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        if (!musicDict.TryGetValue(name, out Sound m))
        {
            Debug.LogWarning($"AudioManager: Sound '{name}' not found!");
            return;
        }

        musicSource.clip = m.clip;
        musicSource.volume = m.volume;
        musicSource.pitch = m.pitch;
        musicSource.loop = m.loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
            else
            {
                fadeOutCoroutine = StartCoroutine(FadeOutMusic());
            }
        }
    }

    private IEnumerator FadeOutMusic(float duration = 1f)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // reset for next playback

        fadeOutCoroutine = null;
    }

    public void PlaySound(string name)
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

    public void PlaySound(string name, AudioSource source)
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
