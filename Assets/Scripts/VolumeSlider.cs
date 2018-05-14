using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    // private Button setMin;
    // private Button setMax;
    public static float sliderMin = 0;
    public static float sliderMax = 1;
    // private Text valueText;
    private float roundedVol = 0;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        // setMin = transform.Find("Set Min Vol").GetComponent<Button>();
        // setMin.onClick.AddListener(new UnityAction(OnSetMin));
        // setMax = transform.Find("Set Max Vol").GetComponent<Button>();
        // setMax.onClick.AddListener(new UnityAction(OnSetMax));
        // valueText = transform.Find("Slider Value Vol").GetComponent<Text>();
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnVolumeChange);
    }

    private void Start()
    {
        EventManager.TriggerEvent(EventTypes.NewVolumeBounds, new float[] { slider.minValue, slider.maxValue });
    }

    private void OnVolumeChange(object volObj)
    {
        float vol = (float)volObj;
        roundedVol = (float)Math.Round(vol, 2, MidpointRounding.AwayFromZero);
        slider.value = roundedVol;
        // valueText.text = roundedVol.ToString();
    }
    private void OnSetMin()
    {
        slider.minValue = roundedVol;
        sliderMin = roundedVol;
        EventManager.TriggerEvent(EventTypes.NewVolumeBounds, new float[] { slider.minValue, slider.maxValue });
    }
    private void OnSetMax()
    {
        slider.maxValue = roundedVol;
        sliderMax = roundedVol;
        EventManager.TriggerEvent(EventTypes.NewVolumeBounds, new float[] { slider.minValue, slider.maxValue });
    }
}
