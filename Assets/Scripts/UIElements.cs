using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
