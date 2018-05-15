using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheMission : MonoBehaviour {

    GameObject missionStatement;
    GameObject whatAreYou;
    private bool missionActive;
	// Use this for initialization
	void Start () {
        missionStatement = GameObject.Find("MissionStatement");
        whatAreYou = GameObject.Find("YouGhost");
        missionStatement.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMission()
    {
        if (missionActive)
        {
            whatAreYou.SetActive(true);
            missionStatement.SetActive(false);
            GameObject.Find("Mission").GetComponentInChildren<Text>().text = "Mission";
            missionActive = false;
        }
        else
        {
            whatAreYou.SetActive(false);
            missionStatement.SetActive(true);
            GameObject.Find("Mission").GetComponentInChildren<Text>().text = "Back";
            missionActive = true;
        }
    }
}
