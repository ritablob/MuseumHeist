using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFromArduino : MonoBehaviour
{
    public GameObject player;

    private bool disguiseOn = false;
    public void LightBarrierTrigger()
    {
        disguiseOn = !disguiseOn;
        Debug.Log("Triggered light barrier, disguise is " + disguiseOn);
    }
    public void ForwardsButton()
    {
        // move player forward
        Debug.Log("moving forward");
    }
    public void RotaryDevice(int direction)
    {
        // rotate the player based on the input
        Debug.Log("input: " + direction);
    }
}
