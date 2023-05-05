using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Artefact", menuName = "Artefacts")]
[System.Serializable]
public class ArtefactCollection : ScriptableObject
{
    public List<Sprite> sprites;
    public Sprite fullImage;
    public Sprite ninthSprite;
}