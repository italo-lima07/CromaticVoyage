using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;

    public AudioClip clipPulo, clipPoção;

    private void OnEnable()
    {
        AudioObserver.PlaySfxEvent += TocarEfeitoSonoro;
    }

    private void OnDisable()
    {
        AudioObserver.PlaySfxEvent -= TocarEfeitoSonoro;
    }

    void TocarEfeitoSonoro(string nomeDoClip)
    {
        switch (nomeDoClip)
        {
            case "pulo":
                sfxSource.PlayOneShot(clipPulo);
                break;
            case "coletavel":
                sfxSource.PlayOneShot(clipPoção);
                break;
            default:
                Debug.LogError($"efeito sonoro {nomeDoClip} não encontrado");
                break;
        }
    }
}
