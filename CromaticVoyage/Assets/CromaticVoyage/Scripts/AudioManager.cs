using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioSource loopSource; // Fonte separada para sons contínuos, como andar

    public AudioClip clipPulo, clipPoção, clipMagiaPlayer, clipHitPlayer, clipHitEnemy, clipWalkPlayer, clipWalkEnemy, clipAtkEnemy, clipStar;

    private void OnEnable()
    {
        AudioObserver.PlaySfxEvent += TocarEfeitoSonoro;
        AudioObserver.StopSfxEvent += PararEfeitoSonoro;
    }

    private void OnDisable()
    {
        AudioObserver.PlaySfxEvent -= TocarEfeitoSonoro;
        AudioObserver.StopSfxEvent -= PararEfeitoSonoro;
    }

    void TocarEfeitoSonoro(string nomeDoClip)
    {
        switch (nomeDoClip)
        {
            case "walkplayer":
                if (!loopSource.isPlaying) // Evita reiniciar o som se já está tocando
                {
                    loopSource.clip = clipWalkPlayer;
                    loopSource.loop = true;
                    loopSource.Play();
                }
                break;
            default:
                sfxSource.PlayOneShot(GetClipByName(nomeDoClip));
                break;
        }
    }

    void PararEfeitoSonoro(string nomeDoClip)
    {
        if (nomeDoClip == "walkplayer" && loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }

    private AudioClip GetClipByName(string nomeDoClip)
    {
        return nomeDoClip switch
        {
            "pulo" => clipPulo,
            "coletavelpocao" => clipPoção,
            "atkplayer" => clipMagiaPlayer,
            "coletavel" => clipStar,
            "hitplayer" => clipHitPlayer,
            "hitenemy" => clipHitEnemy,
            "walkenemy" => clipWalkEnemy,
            "atkenemy" => clipAtkEnemy,
            _ => null
        };
    }
}