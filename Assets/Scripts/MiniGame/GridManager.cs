using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<GridTile> tiles;
    private GridTile emptyTile;
    [SerializeField] List<ArtefactCollection> artefactCollections;
    int currentCollection;
    [SerializeField] Sprite emptyImg;

    void Start()
    {
        
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

    }
}

