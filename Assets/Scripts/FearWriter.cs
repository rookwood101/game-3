using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FearWriter : MonoBehaviour
{
    AmISpooked amISpooked;
    [SerializeField]
    GameObject fearSliderPrefab;
    Slider fearSlider;
    Slider tensenessSlider;
    GameObject fearSliderGO;
    GameObject tensenessSliderGO;
    GameObject ghost;

    private void Awake()
    {

        ghost = GameObject.Find("Ghost");

        amISpooked = GetComponent<AmISpooked>();

        fearSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 fearFollowEntity = fearSliderGO.GetComponent<FollowEntity2>();
        fearFollowEntity.toTrack = gameObject;
        fearFollowEntity.offset = new Vector3(0, 2, 0);
        fearSliderGO.transform.Find("Label").GetComponent<Text>().text = "FEAR";

        tensenessSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 tenseFollowEntity = tensenessSliderGO.GetComponent<FollowEntity2>();
        tenseFollowEntity.toTrack = gameObject;
        tenseFollowEntity.offset = new Vector3(0, 3, 0);
        tensenessSliderGO.transform.Find("Label").GetComponent<Text>().text = "TENSENESS";

        this.fearSlider = fearSliderGO.GetComponent<Slider>();
        this.tensenessSlider = tensenessSliderGO.GetComponent<Slider>();

        fearSlider.minValue = 0;
        fearSlider.maxValue = 10;
        tensenessSlider.minValue = 0;
        tensenessSlider.maxValue = 1;

        EventManager.AddListener(EventTypes.NPCExit, DestroyNPC);
    }

    private void DestroyNPC(object exitingNpc)
    {
        if ((GameObject)exitingNpc == gameObject)
        {
            ghost.layer = 0;
            Destroy(fearSliderGO);
            Destroy(tensenessSliderGO);
            Destroy(gameObject);
            Destroy(((GameObject)exitingNpc).GetComponent<NPCMovement>().target);

            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Tutorial")
            {
                SceneManager.LoadScene(currentScene.buildIndex + 1);
            }
        }
    }

    private void LateUpdate()
    {
        fearSlider.value = amISpooked.fear;
        tensenessSlider.value = amISpooked.tenseness;
    }
}
