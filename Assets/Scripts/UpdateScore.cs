using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour {

    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
        EventManager.AddListener(EventTypes.UpdateScore, DisplayScore);
    }

    private void DisplayScore(object scoreString)
    {
        text.text = (string) scoreString;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
