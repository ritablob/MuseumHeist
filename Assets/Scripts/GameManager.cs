using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winCanvas;
    [HideInInspector] public bool hasArtefact;
    public bool UIMode;
    // Start is called before the first frame update
    void Start()
    {
        UIMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckWin();
    }
    public void CheckWin()
    {
        if (hasArtefact)
        {
            // win canvas visible
            // ui input viable
            // game input disabled 
            // game time to zero
            // display gameplay time 
            UIMode = true;
            winCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
