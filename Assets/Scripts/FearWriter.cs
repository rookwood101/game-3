using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FearWriter : MonoBehaviour
{
    AmISpooked amISpooked;
    [SerializeField]
    GameObject fearSliderPrefab;
    Slider fearSlider;
    Slider tensenessSlider;

    private void Awake()
    {
        amISpooked = GetComponent<AmISpooked>();
        GameObject fearSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 fearFollowEntity = fearSliderGO.GetComponent<FollowEntity2>();
        fearFollowEntity.toTrack = gameObject;
        fearFollowEntity.offset = new Vector3(0, 2, 0);

        GameObject tensenessSliderGO = Instantiate(fearSliderPrefab, GameObject.Find("Canvas").transform);
        FollowEntity2 tenseFollowEntity = tensenessSliderGO.GetComponent<FollowEntity2>();
        tenseFollowEntity.toTrack = gameObject;
        tenseFollowEntity.offset = new Vector3(0, 3, 0);

        this.fearSlider = fearSliderGO.GetComponent<Slider>();
        this.tensenessSlider = tensenessSliderGO.GetComponent<Slider>();

        fearSlider.minValue = 0;
        fearSlider.maxValue = 1000;
        tensenessSlider.minValue = 0;
        tensenessSlider.maxValue = 1000;
    }

    private void LateUpdate()
    {
        fearSlider.value = amISpooked.fear;
        tensenessSlider.value = amISpooked.tenseness;
    }
}
