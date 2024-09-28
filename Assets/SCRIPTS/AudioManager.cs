using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class BGMTrack
    {
        public string name;
        public AudioClip clip;
    }

    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<BGMTrack> bgmTracks = new List<BGMTrack>();
    [SerializeField] private float fadeTime = 1f;

    private AudioSource bgmSource;
    private int currentBgmIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        
        if (bgmTracks.Count > 0)
        {
            ChangeBGM(0);
        }
    }

    public void ChangeBGM(int index)
    {
        if (index < 0 || index >= bgmTracks.Count || index == currentBgmIndex)
        {
            return;
        }

        currentBgmIndex = index;
        StartCoroutine(FadeBGM(bgmTracks[index].clip));
    }

    public void ChangeBGM(string trackName)
    {
        int index = bgmTracks.FindIndex(track => track.name == trackName);
        if (index != -1)
        {
            ChangeBGM(index);
        }
        else
        {
            Debug.LogWarning($"BGM track '{trackName}' not found.");
        }
    }

    private IEnumerator FadeBGM(AudioClip newClip)
    {
        float timeElapsed = 0;
        float startVolume = bgmSource.volume;

        if (bgmSource.clip != null)
        {
            while (timeElapsed < fadeTime)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        bgmSource.clip = newClip;
        bgmSource.Play();

        timeElapsed = 0;
        while (timeElapsed < fadeTime)
        {
            bgmSource.volume = Mathf.Lerp(0, startVolume, timeElapsed / fadeTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        currentBgmIndex = -1;
    }
}