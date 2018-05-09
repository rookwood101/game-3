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

    Color activeColor = new Color(255, 0, 0, 255);
    Color inActiveColor = new Color(255, 255, 255, 127);
    ColorBlock sliderColorBlock;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        setMin = GameObject.Find("Set Min").GetComponent<Button>();
        setMin.onClick.AddListener(new UnityAction(OnSetMin));
        setMax = GameObject.Find("Set Max").GetComponent<Button>();
        setMax.onClick.AddListener(new UnityAction(OnSetMax));
        sliderColorBlock = slider.colors;
        EventManager.AddListener(EventTypes.MicrophonePitch, OnPitchChange);
    }

    private void OnPitchChange(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            sliderColorBlock.disabledColor = activeColor;
            slider.colors = sliderColorBlock;
            slider.value = pitch.Value;
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
