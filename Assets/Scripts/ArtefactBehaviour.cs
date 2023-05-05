using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactBehaviour : MonoBehaviour
{
    public WinManager winManager;
    [SerializeField] private PuzzleManager puzzleManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            winManager.hasArtefact = true;
            puzzleManager.SetUpMiniGame();

            Debug.Log("Inventory status = " + winManager.hasArtefact);
        }
    }
}
