using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStart : MonoBehaviour {
    private GameObject canvas;

    // Use this for initialization
    void Start () {
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
	}
	
	// Update is called once per frame
	async void Update () {
        if (Input.GetMouseButton(0))
        {
            EventManager.TriggerEvent(EventTypes.Start, 0);
            await Wait.ForIEnumerator(new WaitForSeconds(3));
            canvas.SetActive(true);
        }
	}
}
