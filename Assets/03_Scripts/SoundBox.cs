using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundBox : MonoBehaviour
{
    public static SoundBox Box;

    private void Awake() {
        if (Box == null)
        {
            Box = this;
            DontDestroyOnLoad(Box);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioMixer MyAudioMixer;
    [Header("Source")]
    public AudioSource BGMAudio;
    public AudioSource SFXAudio;
    [Header("BGM")]
    public AudioClip TitleBGM;
    public AudioClip LastBGM;
    [Header("SFX")]
    public AudioClip CheckHandSFX;

    public void ChangeBGM(AudioClip clip)
    {
        if (clip == null)
        {
            BGMAudio.clip = null;
            BGMAudio.Stop();
        }

        BGMAudio.clip = clip;
        BGMAudio.Play();

    }

    public void ChangeSFX(AudioClip clip)
    {

        if (clip == null)
        {
            SFXAudio.clip = null;
            SFXAudio.Stop();
        }

        SFXAudio.PlayOneShot(clip);

    }
}
