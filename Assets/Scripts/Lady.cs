using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lady : LevelObject, ControllableCharacter
{
    public Vector3 SelectedIndicatorPosition { get { return SelectedIndicatorMarker.transform.position; } }

    public void TryMove(Vector3Int direction)
    {
        if ( !Level.IsTileSuitableForLady(this.MinCorner + direction) )
        {
            return;
        }

        this.transform.position = Vector3Int.RoundToInt(this.transform.position + direction);

        VerticalLayer = Level.GetTopVerticalLayer(MinCorner) + 1;

        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
    }
}
