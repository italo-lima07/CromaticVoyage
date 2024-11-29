using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioObserver
{
    public static event Action<string> PlaySfxEvent;
    public static event Action<string> StopSfxEvent;

    public static void OnPlaySfxEvent(string obj)
    {
        PlaySfxEvent?.Invoke(obj);
    }

    public static void OnStopSfxEvent(string obj)
    {
        StopSfxEvent?.Invoke(obj);
    }
}