using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    MicrophoneVolume,
    MicrophonePitch
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }