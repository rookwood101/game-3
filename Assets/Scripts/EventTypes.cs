using UnityEngine;
using UnityEngine.Events;

public enum EventTypes
{
    MicrophoneVolume,
    MicrophonePitch,
    NewPitchBounds
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }