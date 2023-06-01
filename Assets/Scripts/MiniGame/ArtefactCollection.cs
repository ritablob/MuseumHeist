using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// scriptable object so that we can have multiple different sliding tile puzzles
/// a list of 8 sprites that comprise the images that will be moved around
/// and a sprite of the full image to give as a clue to the player
/// currently, we have 3 different artefacts
/// </summary>

[CreateAssetMenu(fileName = "new Artefact", menuName = "Artefacts")]
[System.Serializable]
public class ArtefactCollection : ScriptableObject
{
    public List<Sprite> sprites;
    public Sprite fullImage;
    public Sprite ninthSprite;
}