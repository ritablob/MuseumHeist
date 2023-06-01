using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// sits on the ui elements to be controlled via the arduino controller (most are buttons that also can be clicked)
/// (de-)selects the element by saving if it is currently highlighted
/// uses UnityEvents onSelect, onDeselect and onClick to define in the Editor which actions should happen once this happens
/// (activate highlighted object or do action of the button...)
/// </summary>

public class UIElements : MonoBehaviour
{
    private bool highlighted = false;

    [SerializeField] private UnityEvent onSelectEvents;
    [SerializeField] private UnityEvent onDeselectEvents;
    [SerializeField] private UnityEvent onClickEvents;

    public void SelectElement()
    {
        highlighted = true;
        onSelectEvents.Invoke();
    }

    public void DeselectElement()
    {
        highlighted = false;
        onDeselectEvents.Invoke();
    }

    public void ClickElement()
    {
        if (highlighted) onClickEvents.Invoke();
    }
}
