using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{

    private int score;
    private bool isInPerson;

    private void Awake()
    {
        EventManager.AddListener(EventTypes.NPCExit, AddPoints);
        EventManager.AddListener(EventTypes.Dead, TakePoints);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPoints(object npcScared)
    {
        score++;
        EventManager.TriggerEvent(EventTypes.UpdateScore, score.ToString());
    }
    private void TakePoints(object npcScared)
    {
        score--;
        EventManager.TriggerEvent(EventTypes.UpdateScore, score.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "FrontDoor" && SceneManager.GetActiveScene().name == "Tutorial 2")
        {
            SceneManager.LoadScene(2);
        }

    }

}
