using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearWriter : MonoBehaviour
{
    AmISpooked amISpooked;
    [SerializeField]
    GameObject fearSliderPrefab;
    GameObject fearSlider;

    private void Awake()
    {
        amISpooked = GetComponent<AmISpooked>();
        fearSlider = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
    }

    private void Update()
    {
        // text.text = amISpooked.fear.ToString();
    }
}
