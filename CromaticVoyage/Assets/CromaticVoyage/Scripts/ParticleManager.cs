using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject prefabFumaçaaa;


    private void OnEnable()
    {
        ParticleObserver.ParticleSpawnEvent += SpawnarParticulas;
    }

    private void OnDisable()
    {
        ParticleObserver.ParticleSpawnEvent -= SpawnarParticulas;
    }

    public void SpawnarParticulas(Vector3 posicao)
    {
        Instantiate(prefabFumaçaaa, posicao, quaternion.identity);
    }
}
