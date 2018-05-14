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
    Image fearBG;
    Slider tensenessSlider;
    Image tensenessBG;
    GameObject fearSliderGO;
    GameObject tensenessSliderGO;
    GameObject ghost;
    Animator anim;

    private void Awake()
    {

        ghost = GameObject.Find("Ghost");
        anim = GetComponent<Animator>();
        amISpooked = GetComponent<AmISpooked>();

        fearSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 fearFollowEntity = fearSliderGO.GetComponent<FollowEntity2>();
        fearFollowEntity.toTrack = gameObject;
        fearFollowEntity.offset = new Vector3(0, 2, 0);

        tensenessSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 tenseFollowEntity = tensenessSliderGO.GetComponent<FollowEntity2>();
        tenseFollowEntity.toTrack = gameObject;
        tenseFollowEntity.offset = new Vector3(0, 3, 0);

        this.fearSlider = fearSliderGO.GetComponent<Slider>();
        this.tensenessSlider = tensenessSliderGO.GetComponent<Slider>();
        this.fearBG = fearSliderGO.transform.Find("Fill Area/Fill").GetComponent<Image>();
        this.tensenessBG = tensenessSliderGO.transform.Find("Fill Area/Fill").GetComponent<Image>();

        fearSlider.minValue = 0;
        fearSlider.maxValue = AmISpooked.FEAR_THRESHOLD;
        tensenessSlider.minValue = 0;
        tensenessSlider.maxValue = AmISpooked.TENSENESS_THRESHOLD;

        fearBG.color = Color.red;
        tensenessBG.color = Color.yellow;

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

    private async void LateUpdate()
    {
        if (amISpooked.dead)
        {
            anim.SetTrigger("death");
            await Wait.ForIEnumerator(new WaitForSeconds(2));
            Destroy(fearSliderGO);
            Destroy(tensenessSliderGO);
            this.enabled = false;
            return;
        }

        if (amISpooked.tenseness == 0)
        {
            tensenessSliderGO.SetActive(false);
        }
        else
        {
            tensenessSliderGO.SetActive(true);
        }

        if (amISpooked.fear == 0)
        {
            fearSliderGO.SetActive(false);
        }
        else
        {
            fearSliderGO.SetActive(true);
        }

        fearSlider.value = amISpooked.fear;
        tensenessSlider.value = amISpooked.tenseness;
    }
}
