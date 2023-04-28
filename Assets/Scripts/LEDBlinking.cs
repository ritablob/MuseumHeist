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
    private Controller controller;

    void Start()
    {
        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
        controller = GetComponent<Controller>();
    }

    void Update()
    {
        timePassed += (Time.deltaTime*1000f);

        if (timePassed >= currentSpeed) 
        {
            if(controller != null) { controller.SwitchLEDState(); }
            timePassed = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) { ChangeLEDSpeed(false); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { ChangeLEDSpeed(true); }
    }

    public void ChangeLEDSpeed(bool faster = true)
    {
        if (!faster) { currentSpeed += incrementSteps; }
        else { currentSpeed -= incrementSteps; }

        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
    }
}
