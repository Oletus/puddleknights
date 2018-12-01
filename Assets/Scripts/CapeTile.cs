using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : LevelObject
{
    [System.NonSerialized] public CapeTile NextCapePiece;

    protected void MoveCape(Vector3Int direction)
    {
        Vector3 myOldPosition = transform.position;
        transform.position += direction;
        if ( NextCapePiece != null )
        {
            NextCapePiece.MoveCape(Vector3Int.RoundToInt(myOldPosition - NextCapePiece.transform.position));
        }
    }
}
