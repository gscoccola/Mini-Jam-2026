using UnityEngine;
using System.Collections.Generic;

public class SFXManager : PersistentSingleton<SFXManager>
{
    private List<AudioSource> _sfxSources = new();

    [Header("UI")]
    public AudioClip[] UIClick;

    [Header("Map")]
    public AudioClip[] MapScribble;
    public AudioClip[] MapPage;

    [Header("Player")]
    public AudioClip[] PlayerAttack;
    public AudioClip[] CardFlush;
    public AudioClip[] PlayerDeath;

    [Header("Enemy")]
    public AudioClip[] EnemyAttack;
    public AudioClip[] EnemyDeath;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 10; i++)
        {
            GameObject sfxSource = new GameObject("SFXSource" + i);
            sfxSource.transform.SetParent(transform);
            sfxSource.AddComponent<AudioSource>();
            _sfxSources.Add(sfxSource.GetComponent<AudioSource>());
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in _sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        Debug.LogWarning("All SFX sources are currently playing. Consider increasing the pool size.");
        return null;
    }

    public void PlaySFX(AudioClip[] clip, bool randomizePitch, float pitch = 1f)
    {
        if (clip == null || clip.Length == 0)
        {
            Debug.LogWarning($"SFX source {clip} is null.");
            return;
        }

        AudioSource source = GetAvailableAudioSource();

        source.pitch = pitch;
        if (randomizePitch)
        {
            source.pitch = pitch * Random.Range(0.9f, 1.1f);
        }

        int index = Random.Range(0, clip.Length);
        if (source != null)
        {
            source.clip = clip[index];
            source.Play();
        }
    }
}
