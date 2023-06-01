using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// handles the puzzle functionality
/// sets up the minigame by activating the puzzle interface and creating a random solvable puzzle
/// selects and moves tiles (if prompted by puzzle controller)
/// every time it moves tiles, checks if new order is the correct order and the puzzle is solved
/// if so, deactivates puzzle interface and resumes game
/// </summary>

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
        // set game management variables
        GameManagement.gameplayActive = false;
        GameManagement.currentMode = GameManagement.GameMode.Puzzle;
        puzzleObject.SetActive(true);

        // choose a random artefact puzzle
        if (artefactCollections.Count > 0)
        {
            currentCollection = Random.Range(0, artefactCollections.Count);
        }

        bool solvable = false;

        // create a solvable puzzle
        // keeps creating puzzles, checks if they are solvable, if not creates a new one etc
        while (!solvable)
        {
            // create a list of sprites that need to be randomly sorted onto the grid tiles
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (Sprite sprite in artefactCollections[currentCollection].sprites) spritesToAdd.Add(sprite);
            // set the full image (solution image on the left of the screen)
            fullImageRenderer.sprite = artefactCollections[currentCollection].fullImage;

            // list of ints --> the order in which the tiles are randomly put, we need that to check if the puzzle is solvable
            List<int> order = new List<int>();

            foreach (GridTile tile in tiles)
            {
                // for every tile that needs to be filled with a sprite
                if (spritesToAdd.Count > 0)
                {
                    // assign a random image of selection to the tile
                    int random = Random.Range(0, spritesToAdd.Count);
                    tile.SetImage(spritesToAdd[random]);

                    // add which number it is (in correct order) to the list of ints
                    order.Add(artefactCollections[currentCollection].sprites.IndexOf(spritesToAdd[random]));

                    // remove the sprite from the list so we don't add it again
                    spritesToAdd.RemoveAt(random);
                }

                // deselect all tiles
                tile.DeselectTile();
            }

            int inversions = 0;

            // check if generated puzzle is solvable (check for number of inversions, needs to be even)
            for (int i = 0; i < order.Count; i++)
            {
                // for every int in order, check if any of the ints after it are lower --> add to inversion count
                for (int j = i; j < order.Count; j++) 
                {
                    if (order[i] > order[j]) inversions++;
                }
            }

            // if the number of inversions is even --> puzzle is solvable
            if (inversions % 2 == 0)
            {
                solvable = true;
            }

            //Debug.Log("randomated puzzle was solvable ? " + solvable);
        }

        // set empty tile, set currentlySelectedTile to 0, select the selected tile (a neighbour of the empty tile)
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

        // go through neighbours of emptyTile and select next one
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
        // if the correct order is found set won to true
        Debug.Log("won");
        won = true;

        // deactivate the puzzle interface and reactivate the gameplay
        puzzleObject.SetActive(false);
        GameManagement.gameplayActive = true;
        GameManagement.currentMode = GameManagement.GameMode.Gameplay;

        // reactivate the invisibility counter
        if (invisCounterObj != null) { invisCounterObj.SetActive(true); }
        // play audio that artefact was collected
        EventManager.Instance.EventGo("AUDIO", "Collect");
    }
}

