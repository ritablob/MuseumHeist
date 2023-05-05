using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject puzzleObject;

    [SerializeField] List<GridTile> tiles;
    private GridTile emptyTile;
    int currentlySelectedTile;
    [SerializeField] List<ArtefactCollection> artefactCollections;
    int currentCollection;
    [SerializeField] Sprite emptyImg;
    [SerializeField] Image fullImageRenderer;

    [Space]
    [SerializeField] private GuardBehaviour guardBehaviour;
    [SerializeField] private GameObject invisCounterObj;
    [SerializeField] private PlayerMovement player;

    bool won = false;

    void Start()
    {
        puzzleObject.SetActive(false);
        EventManager.Instance.AddEventListener("MINIGAME", ArtefactListener);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener("MINIGAME", ArtefactListener);
    }

    void ArtefactListener(string eventName, object param)
    {
        if (eventName == "Start") SetUpMiniGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectTile(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectTile(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTile();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)) CorrectOrder();
    }

    public void SetUpMiniGame()
    {
        GameManagement.guardsActive = false;
        GameManagement.currentMode = GameManagement.GameMode.Puzzle;
        puzzleObject.SetActive(true);

        // choose random artefact puzzle
        if (artefactCollections.Count > 0)
        {
            currentCollection = Random.Range(0, artefactCollections.Count);
        }
        List<Sprite> spritesToAdd = new List<Sprite>();
        foreach (Sprite sprite in artefactCollections[currentCollection].sprites) spritesToAdd.Add(sprite);
        fullImageRenderer.sprite = artefactCollections[currentCollection].fullImage;

        foreach (GridTile tile in tiles)
        {
            // assign a random image of selection to first 8 tiles
            if (spritesToAdd.Count > 0)
            {
                int random = Random.Range(0, spritesToAdd.Count);
                tile.SetImage(spritesToAdd[random]);
                spritesToAdd.RemoveAt(random);
            }

            // deselect all tiles
            tile.DeselectTile();
        }

        // set empty tile, 
        emptyTile = tiles[8];
        currentlySelectedTile = 0;
        emptyTile.neighbours[currentlySelectedTile].SelectTile();
        won = false;

        // set up other in game stuff
        if (guardBehaviour != null) guardBehaviour.StartPuzzle();
        if (invisCounterObj != null) { invisCounterObj.SetActive(false); }
        if (player != null) player.Movement(false);

        EventManager.Instance.EventGo("AUDIO", "Puzzle");
    }

    public void MoveTile()
    {
        if (won) return;

        GridTile newEmpty = emptyTile.neighbours[currentlySelectedTile];
        // move image to empty spot and empty currently selected
        emptyTile.SetImage(emptyTile.neighbours[currentlySelectedTile].Current());
        newEmpty.SetImage(emptyImg);

        // move highlight and save new empty spot
        newEmpty.DeselectTile();
        emptyTile = newEmpty;
        currentlySelectedTile = 0;
        emptyTile.neighbours[currentlySelectedTile].SelectTile();

        // check if order is now correct
        bool correct = true;
        for (int i = 0; i < 8; i++)
        {
            if (tiles[i].Current() != artefactCollections[currentCollection].sprites[i]) correct = false;
        }
        if (correct) { CorrectOrder(); }
    }

    public void SelectTile(bool up)
    {
        if (won) return;

        // go through neighbours of emptyTile
        emptyTile.neighbours[currentlySelectedTile].DeselectTile();
        if (up)
        {
            currentlySelectedTile++;
            if (currentlySelectedTile >= emptyTile.neighbours.Count) currentlySelectedTile = 0;
        }
        else
        {
            currentlySelectedTile--;
            if (currentlySelectedTile < 0) currentlySelectedTile = emptyTile.neighbours.Count - 1;
        }
        emptyTile.neighbours[currentlySelectedTile].SelectTile();
    }

    public void CorrectOrder()
    {
        Debug.Log("won");
        won = true;

        puzzleObject.SetActive(false);
        GameManagement.guardsActive = true;
        GameManagement.currentMode = GameManagement.GameMode.Gameplay;

        if (invisCounterObj != null) { invisCounterObj.SetActive(true); }
        EventManager.Instance.EventGo("AUDIO", "Collect");
    }
}

