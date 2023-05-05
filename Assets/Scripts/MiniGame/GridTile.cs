using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridTile : MonoBehaviour
{
    public List<GridTile> neighbours;
    [SerializeField] private GameObject imgObject;
    Image image;
    [SerializeField] GameObject highlight;

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
}
