using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactBehaviour : MonoBehaviour
{
    public WinManager gameManager;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            gameManager.hasArtefact = true;
            Debug.Log("Inventory status = " + gameManager.hasArtefact);
        }
    }
}
