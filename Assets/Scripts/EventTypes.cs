using UnityEngine;
using UnityEngine.Events;

public enum EventTypes
{
    MicrophoneVolume,
    MicrophonePitch,
    NewPitchBounds,
    Runaway,
    Investigate,
    Spookometer,
    NewVolumeBounds,
    StopRunaway
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }
