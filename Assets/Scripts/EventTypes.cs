using UnityEngine;
using UnityEngine.Events;

public enum EventTypes
{
    MicrophoneVolume,
    MicrophonePitch,
    NewPitchBounds,
    Runaway,
    Investigate
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }