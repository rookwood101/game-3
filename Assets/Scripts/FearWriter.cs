using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearWriter : MonoBehaviour
{
    AmISpooked amISpooked;
    TextMesh text;

    private void Awake()
    {
        amISpooked = GetComponent<AmISpooked>();
        text = GetComponentInChildren<TextMesh>();
    }

    private void Update()
    {
        text.text = amISpooked.fear.ToString();
    }
}
