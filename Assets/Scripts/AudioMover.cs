using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;

public class AudioMover : MonoBehaviour
{
    int lastSample = 0;
    AudioClip micRecording;
    PitchTracker pitchTracker;
    float currentPitch = 0;
    float speed = 10;
    bool isPitchChanged = false;

    const string DEVICE_NAME = null;
    const int FREQUENCY = 44100;
    const float midPitch = 6f;

    private Rigidbody2D rb;

    private void Start()
    {
        pitchTracker = new PitchTracker
        {
            SampleRate = FREQUENCY,
            DetectLevelThreshold = 0.0001f,
            RecordPitchRecords = false,
        };
        pitchTracker.PitchDetected += PitchDetected;
        micRecording = Microphone.Start(null, true, 100, FREQUENCY);
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isPitchChanged = false;
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
        } else
        {
            return;
        }
        //pitchTracker.ProcessBuffer(samples);
        //float totalSquares = 0;
        //foreach (float sample in samples)
        //{
        //    totalSquares += sample * sample;
        //}
        //float rms_volume = Mathf.Sqrt(totalSquares / samples.Length);

        //Debug.Log(rms_volume);
        if (currentPitch > 0.01f)
        {
            currentPitch -= 0.01f;
        }
        else if (currentPitch < -0.01f)
        {
            currentPitch += 0.01f;
        }
        else
        {
            currentPitch = 0;
        }
        pitchTracker.ProcessBuffer(samples);

        if (isPitchChanged)
        {
            rb.AddForce(new Vector2(0, currentPitch * speed));
        }

        lastSample = pos;
    }
    private void PitchDetected(PitchTracker sender, PitchTracker.PitchRecord pitchRecord)
    {
        float logPitch = Mathf.Log(pitchRecord.Pitch, 2);
        if (!float.IsInfinity(logPitch))
        {
            currentPitch = logPitch - midPitch;
            isPitchChanged = true;
        }
        
    }

}
