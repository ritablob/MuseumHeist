using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactBehaviour : MonoBehaviour
{
    bool isPickedUp;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            isPickedUp = true;
            Debug.Log("Inventory status = " + isPickedUp);
        }
    }
}
