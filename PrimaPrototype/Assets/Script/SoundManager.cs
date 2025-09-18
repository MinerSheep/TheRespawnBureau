using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrimaF
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public AudioSource BGMAudioSource;
        public List<AudioSource> BGMList;

        public GameObject audioSourcePrefab;
        public int AudioSourceCount = 5;

        void Update()
        {

        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            CreateAudioSourceList();
        }

        private void CreateAudioSourceList()
        {
            BGMList = new List<AudioSource>();
            for (int i = 0; i < AudioSourceCount; i++)
            {
                GameObject sourceObject = Instantiate(audioSourcePrefab, transform);
                AudioSource source = sourceObject.GetComponent<AudioSource>();
                sourceObject.SetActive(false);
                BGMList.Add(source);
            }
        }
        public void PlayBGM(AudioClip clip, Vector3 position, float volume)
        {
            AudioSource source = GetAvailableBGMSource();
            source.transform.position = position;
            source.volume = volume;
            source.clip = clip;
            source.Play();
            StartCoroutine(StopPlay(clip, source));
        }

        public IEnumerator StopPlay(AudioClip clip, AudioSource source)
        {
            yield return new WaitForSeconds(clip.length);
            GameObject sourceObject = source.gameObject;
            sourceObject.SetActive(false);
        }


        private AudioSource GetAvailableBGMSource()
        {
            foreach (AudioSource source in BGMList)
            {
                if (!source.gameObject.activeSelf)
                {
                    source.gameObject.SetActive(true);
                    return source;
                }
            }

            GameObject sourceObject = Instantiate(audioSourcePrefab, transform);
            AudioSource newSource = sourceObject.GetComponent<AudioSource>();
            BGMList.Add(newSource);
            return newSource;
        }
    }
}
