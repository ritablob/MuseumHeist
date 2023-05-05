using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
