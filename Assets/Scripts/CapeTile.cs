using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : LevelObject
{
    [System.NonSerialized] public CapeTile NextCapePiece;

    protected void MoveCape(int newVerticalLayer, Vector3Int direction)
    {
        Vector3 myOldPosition = transform.position;
        transform.position += direction;

        int oldVerticalLayer = VerticalLayer;
        VerticalLayer = newVerticalLayer;

        if ( NextCapePiece != null )
        {
            NextCapePiece.MoveCape(oldVerticalLayer, Vector3Int.RoundToInt(myOldPosition - NextCapePiece.transform.position));
        }
        else
        {
            Level.DropAllVerticalLayers();
        }
    }
}
