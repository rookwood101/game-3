using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    private Text text;
    private Slider slider;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        slider = GetComponent<Slider>();
    }
    private void LateUpdate()
    {
        text.text = slider.value.ToString();
    }
}
