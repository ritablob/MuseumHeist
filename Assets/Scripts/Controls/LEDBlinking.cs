using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls led light
/// different speeds (duration of light on/off in ms) for whether the guard sees the player or not and how close they are
/// measures time passed in update function with time.deltaTime
/// tells controller to send message to arduino to turn led on/off after x amount of time has passed
/// </summary>

public class LEDBlinking : MonoBehaviour
{
    // time measured in ms
    // variables for testing:
    [SerializeField] private float minTimePassed = 200f;
    [SerializeField] private float maxTimePassed = 3000f;
    [SerializeField] private float incrementSteps = 100f;

    [SerializeField] private float currentSpeed = 800f;
    [SerializeField] private float timePassed = 0;

    [Space]
    [SerializeField] private Transform guard;
    [SerializeField] private Transform player;

    private GameplayController controller;
    bool guardSeesPlayer;
    bool ledOn = false;

    [Space] // variables for the different blinking speeds
    [SerializeField] private float calm = 1500f;
    [SerializeField] private float calmNearRobot = 700f;
    [SerializeField] private float seen10 = 400f;
    [SerializeField] private float seen5 = 200f;
    [SerializeField] private float seen0 = 100f;

    void Start()
    {
        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
        controller = GetComponent<GameplayController>();
    }

    void Update()
    {
        if (!GameManagement.portOpen) return;
        if (!GameManagement.gameplayActive) return;

        timePassed += (Time.deltaTime*1000f);

        if (timePassed >= currentSpeed) 
        {
            ledOn = !ledOn;
            if(controller != null) { controller.SwitchLEDState(ledOn); }
            timePassed = 0;
        }

        if (guardSeesPlayer)
        {
            currentSpeed = seen0;
            if (Vector3.Distance(guard.position, player.position) > 5)
                currentSpeed = seen5;
            else if (Vector3.Distance(guard.position, player.position) > 5)
                currentSpeed = seen10;
        }
        else
        {
            currentSpeed = calm;
            if (Vector3.Distance(guard.position, player.position) < 5)
                currentSpeed = calmNearRobot;
        }

        /* used for testing:
        if (Input.GetKeyDown(KeyCode.DownArrow)) { ChangeLEDSpeed(false); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { ChangeLEDSpeed(true); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ledOn = !ledOn;
            controller.SwitchLEDState(ledOn);
        }
        */
    }

    // used for testing
    public void ChangeLEDSpeed(bool faster = true)
    {
        if (!faster) { currentSpeed += incrementSteps; }
        else { currentSpeed -= incrementSteps; }

        if (currentSpeed < minTimePassed) { currentSpeed = minTimePassed; }
        else if (currentSpeed > maxTimePassed) { currentSpeed = maxTimePassed; }
    }

    public void GuardSeesPlayer(bool seesPlayer)
    {
        guardSeesPlayer = seesPlayer;
        if (guardSeesPlayer)
        {
            currentSpeed = seen0;
            if (Vector3.Distance(guard.position, player.position) > 4)
                currentSpeed = seen5;
            else if (Vector3.Distance(guard.position, player.position) > 7)
                currentSpeed = seen10;
        }
        else
        {
            currentSpeed = calm;
            if (Vector3.Distance(guard.position, player.position) < 5)
                currentSpeed = calmNearRobot;
        }
    }
}
