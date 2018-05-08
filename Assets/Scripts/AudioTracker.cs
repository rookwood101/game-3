using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;
using System.Threading.Tasks;
using System;

public class AudioTracker : MonoBehaviour
{
    private int lastSample = 0;
    private AudioClip micRecording;
    private PitchTracker pitchTracker;
    private float? currentPitch = null;
    private int numPitchSamples = 0;
    private bool isTrackingAudio = false;

    private string DEVICE_NAME;
    private const int RECORD_FREQUENCY = 44100;

    private void Start()
    {
        DEVICE_NAME = Microphone.devices[0];

        pitchTracker = new PitchTracker
        {
            SampleRate = RECORD_FREQUENCY,
            DetectLevelThreshold = 0.0001f,
            RecordPitchRecords = false,
        };
        pitchTracker.PitchDetected += PitchDetected;
        StartRecording();
    }

    private void StartRecording()
    {
        micRecording = Microphone.Start(DEVICE_NAME, true, 100, RECORD_FREQUENCY);
        isTrackingAudio = true;
    }

    private void FixedUpdate()
    {
        if (isTrackingAudio)
        {
            float[] samples = GetSamples();

            if (samples == null)
            {
                return;
            }
            float rmsVolume = CalculateRmsVolume(samples);
            float? logPitch = CalculateLogPitch(samples);
            EventManager.TriggerEvent(EventType.MicrophoneVolume, rmsVolume);
            EventManager.TriggerEvent(EventType.MicrophonePitch, logPitch);
        }
    }

    private float[] GetSamples()
    {
        int pos = Microphone.GetPosition(DEVICE_NAME);
        float[] samples;
        if (pos - lastSample > 0)
        {
            int start = lastSample;
            int end = pos;

            samples = new float[(end - start) * micRecording.channels];
            micRecording.GetData(samples, start);
        }
        else if (pos - lastSample != 0)
        {
            int start1 = lastSample;
            int end1 = micRecording.samples - 1;
            float[] samples1 = new float[(end1 - start1) * micRecording.channels];
            micRecording.GetData(samples1, start1);
            int start2 = 0;
            int end2 = pos;
            float[] samples2 = new float[(end2 - start2) * micRecording.channels];
            micRecording.GetData(samples2, start2);

            samples = new float[samples1.Length + samples2.Length];
            samples1.CopyTo(samples, 0);
            samples2.CopyTo(samples, samples1.Length);
        }
        else
        {
            return null;
        }

        lastSample = pos;
        return samples;
    }

    private float CalculateRmsVolume(float[] samples)
    {
        float totalSquares = 0;
        foreach (float sample in samples)
        {
            totalSquares += sample * sample;
        }
        float rmsVolume = Mathf.Sqrt(totalSquares / samples.Length);

        return rmsVolume;
    }

    private float? CalculateLogPitch(float[] samples)
    {
        currentPitch = null;
        numPitchSamples = 0;
        pitchTracker.ProcessBuffer(samples);
        if (currentPitch.HasValue)
        {
            return currentPitch.Value / numPitchSamples;
        }
        else
        {
            return null;
        }
    }

    private void PitchDetected(PitchTracker sender, PitchTracker.PitchRecord pitchRecord)
    {
        float logPitch = Mathf.Log(pitchRecord.Pitch, 2);
        if (!float.IsInfinity(logPitch))
        {
            if (!currentPitch.HasValue)
            {
                currentPitch = 0;
            }
            currentPitch += logPitch;
            numPitchSamples++;
        }

    }
}
