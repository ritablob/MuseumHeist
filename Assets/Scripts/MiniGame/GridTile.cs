using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script that sits on each of the 9 tiles in the tile puzzle game
/// holds a reference to the neighbouring tiles (e.g. the tile in the upper left corner would have two neighbours, one down, one right)
/// (de-) selects tile by setting the highlight object (in-)active
/// sets the Image's sprite to a sprite sent over by the puzzle manager
/// </summary>

public class GridTile : MonoBehaviour
{
    public List<GridTile> neighbours;
    [SerializeField] private GameObject imgObject;
    [SerializeField] GameObject highlight;
    [SerializeField] private Image image;

    public void SetImage(Sprite sprite)
    {
        if (image == null) { image = imgObject.GetComponent<Image>(); }
        image.sprite = sprite;
    }

    public void SelectTile()
    {
        highlight.SetActive(true);
    }

    public void DeselectTile()
    {
        highlight.SetActive(false);
    }

    public Sprite Current()
    {
        return image.sprite;
    }
}
