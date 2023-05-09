using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour
{
    public bool shouldBuzz;
    bool buzzerOn;
    [SerializeField] private float buzzerDuration = 200f;
    [SerializeField] private float pauseDuration = 500f;
    private float timePassed = 0;


    void Start()
    {
        buzzerOn = false;
    }

    void Update()
    {
        if (!GameManagement.portOpen) return;
        if (!GameManagement.guardsActive) return;
        
        if (Input.GetKeyDown(KeyCode.B)) ActivateBuzzer(!shouldBuzz);

        if (shouldBuzz)
        {
            timePassed += (Time.deltaTime * 1000f);
            if (buzzerOn && timePassed >= buzzerDuration)
            {
                EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", "L3");
                timePassed = 0;
            }
            else if (!buzzerOn && timePassed >= pauseDuration) 
            {
                EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", "L3");
                timePassed = 0;
            }
        }
    }

    public void ActivateBuzzer(bool active)
    {
        if (active)
        {
            shouldBuzz = true;
            timePassed = 0;
            buzzerOn = true;
            EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", "L3");
        }
        else
        {
            shouldBuzz = false;
            if (buzzerOn) EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", "L4");
        }
    }
}
