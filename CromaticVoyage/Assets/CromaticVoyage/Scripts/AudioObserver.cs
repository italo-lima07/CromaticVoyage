using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioObserver
{
    public static event Action<string> PlaySfxEvent;

    public static void OnPlaySfxEvent(string obj)
    {
        PlaySfxEvent?.Invoke(obj);
    }
}
