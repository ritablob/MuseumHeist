using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridTile : MonoBehaviour
{
    public List<GridTile> neighbours;
    private Image image;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
