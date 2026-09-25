using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaloriesBar : MonoBehaviour
{
    private Slider Slider;
    public Text caloriesCounter;

    public GameObject playerState;
    private float currentCalories, maxCalories;
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories;
        Slider.value = fillValue;

        caloriesCounter.text = currentCalories + "/" + maxCalories;
    }
}
