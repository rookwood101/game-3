using UnityEngine;
using UnityEngine.Events;

public enum EventTypes
{
    MicrophoneVolume,
    MicrophonePitch,
    NewPitchBounds,
    RunawayToDoorNew,
    RunawayAnywhere,
    NPCExit,
    Spookometer,
    NewVolumeBounds,
    StopRunaway,
    StopRunawayAnywhere,
    UpdateScore,
    RunToDoor,
    Dead,
    Start
}

[System.Serializable]
public class UnityEventWithObject : UnityEvent<object> { }
