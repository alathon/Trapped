using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource backgroundSource;
    private AudioSource soundFxSource;

    [SerializeField]
    private AudioClip ingameBackgroundClip;

    [SerializeField]
    private AudioClip buttonClick;

    void Awake()
    {
        this.backgroundSource = this.gameObject.AddComponent("AudioSource") as AudioSource;
        this.backgroundSource.volume = 0.35f;
        this.backgroundSource.loop = true;
        this.soundFxSource = this.gameObject.AddComponent("AudioSource") as AudioSource;
        this.soundFxSource.volume = 1.0f;
    }

    public void PlaySFX(AudioClip clip)
    {
        this.soundFxSource.clip = clip;
        this.soundFxSource.Play();
    }

    public void PlayIngameBackgroundClip()
    {
        this.backgroundSource.clip = ingameBackgroundClip;
        this.backgroundSource.Play();
    }

    public void PlayButtonClickClip()
    {
        this.soundFxSource.clip = buttonClick;
        this.soundFxSource.Play();
    }
}