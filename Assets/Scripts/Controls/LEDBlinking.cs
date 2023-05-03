using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDBlinking : MonoBehaviour
{
    // time measured in ms
    [SerializeField] private float minTimePassed = 200f;
    [SerializeField] private float maxTimePassed = 3000f;
    [SerializeField] private float incrementSteps = 100f;
    [SerializeField] private float currentSpeed = 800f;
    [SerializeField] private float timePassed = 0;
    private GameplayController controller;

    bool ledOn = false;

    void Start()
    {
        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
        controller = GetComponent<GameplayController>();
    }

    void Update()
    {
        timePassed += (Time.deltaTime*1000f);

        if (timePassed >= currentSpeed) 
        {
            ledOn = !ledOn;
            if(controller != null) { controller.SwitchLEDState(ledOn); }
            timePassed = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) { ChangeLEDSpeed(false); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { ChangeLEDSpeed(true); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ledOn = !ledOn;
            controller.SwitchLEDState(ledOn);
        }
    }

    public void ChangeLEDSpeed(bool faster = true)
    {
        if (!faster) { currentSpeed += incrementSteps; }
        else { currentSpeed -= incrementSteps; }

        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
    }
}
