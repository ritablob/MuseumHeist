using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider chargeSlider;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        chargeSlider.maxValue = playerMovement.invisibilityLimit;
    }

    // Update is called once per frame
    void Update()
    {
        chargeSlider.value = playerMovement.invisibilityTimer;
    }
}
