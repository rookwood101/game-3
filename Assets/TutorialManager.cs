using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        EventManager.AddListener(EventTypes.Dead, FailLevel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private async void FailLevel(object _)
    {
        await Wait.ForIEnumerator(new WaitForSeconds(2));
        SceneManager.LoadScene("Tutorial");
    }
}
