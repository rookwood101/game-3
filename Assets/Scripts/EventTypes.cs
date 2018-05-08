using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    MicrophoneVolume,
    MicrophonePitch,
    NewPitchBounds
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }