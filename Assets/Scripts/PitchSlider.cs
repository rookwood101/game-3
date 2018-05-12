using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PitchSlider : MonoBehaviour
{
    private Slider slider;
    private Button setMin;
    private Button setMax;
    private Text valueText;

    Color activeColor = new Color(255, 0, 0, 255);
    Color inActiveColor = new Color(255, 255, 255, 127);
    ColorBlock sliderColorBlock;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        sliderColorBlock = slider.colors;
        valueText = GameObject.Find("Slider Value").GetComponent<Text>();
        EventManager.AddListener(EventTypes.MicrophonePitch, OnPitchChange);
    }

    private void Start()
    {
        EventManager.TriggerEvent(EventTypes.NewPitchBounds, new float[] { slider.minValue, slider.maxValue });
    }

    private void OnPitchChange(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            sliderColorBlock.disabledColor = activeColor;
            slider.colors = sliderColorBlock;
            slider.value = pitch.Value;
            float roundedPitch = (float)Math.Round(pitch.Value, 1, MidpointRounding.AwayFromZero);
            valueText.text = roundedPitch.ToString();
        }
        else
        {
            sliderColorBlock.disabledColor = inActiveColor;
            slider.colors = sliderColorBlock;
        }
    }
    private void OnSetMin()
    {
        slider.minValue = slider.value;
        EventManager.TriggerEvent(EventTypes.NewPitchBounds, new float[] { slider.minValue, slider.maxValue });
    }
    private void OnSetMax()
    {
        slider.maxValue = slider.value;
        EventManager.TriggerEvent(EventTypes.NewPitchBounds, new float[] { slider.minValue, slider.maxValue });
    }
}
