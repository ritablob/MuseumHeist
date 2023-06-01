using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that deals with Arduino Buzzer
/// When the Guard spots the player --> Sets Buzzer to active (should buzz)
/// If the buzzer should buzz, counts time passed and turns the buzzer on/off to have a beeping effect
/// also turns off buzzer when the guard no longer sees the player
/// </summary>

public class Buzzer : MonoBehaviour
{
    bool shouldBuzz;
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
        if (!GameManagement.gameplayActive) return;

# if UNITY_EDITOR // for testing:
        if (Input.GetKeyDown(KeyCode.B)) SetBuzzer(!shouldBuzz);
#endif

        if (shouldBuzz)
        {
            timePassed += (Time.deltaTime * 1000f);
            // if we waited longer than the buzzer or pause duration --> send signal to arduino to turn off/on
            if ((buzzerOn && timePassed >= buzzerDuration) || (!buzzerOn && timePassed >= pauseDuration))
            {
                EventManager.Instance.EventGo("CONTROLLER", "OutgoingDataArduino", "L3");
                timePassed = 0;
            }
        }
    }

    // called from guard behaviour script to start/stop buzzing the alarm once guard sees/doesn't see player
    public void SetBuzzer(bool active)
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
