using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<GridTile> tiles;
    private GridTile emptyTile;
    int currentlySelectedTile;
    [SerializeField] List<ArtefactCollection> artefactCollections;
    int currentCollection;
    [SerializeField] Sprite emptyImg;

    void Start()
    {
        // choose random artefact puzzle
        // assign a random image of selection to first 8 tiles
        emptyTile = tiles[8];
        currentlySelectedTile = 0;
        foreach (GridTile tile in tiles) 
        {
            tile.DeselectTile();
        }
        emptyTile.neighbours[currentlySelectedTile].SelectTile();
    }

    void Update()
    {
        
    }

    void MoveTile()
    {
        // move tile to empty spot
        // save new empty spot
        // check if order is now correct
        // move highlight 
    }

    void SelectTile()
    {
        // go through neighbours of emptyTile
    }
}

