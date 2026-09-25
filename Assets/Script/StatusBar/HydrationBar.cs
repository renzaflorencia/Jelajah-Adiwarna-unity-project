using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    private Slider Slider;
    public Text hydrationCounter;

    public GameObject playerState;
    private float currentHydration, maxHydration;
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydration;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydration;

        float fillValue = currentHydration / maxHydration;
        Slider.value = fillValue;

        hydrationCounter.text = currentHydration + "/" + maxHydration;
    }
}
