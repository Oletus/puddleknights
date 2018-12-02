using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lady : LevelObject, ControllableCharacter
{
    public Vector3 SelectedIndicatorPosition { get { return SelectedIndicatorMarker.transform.position; } }

    public void TryMove(Vector3Int direction)
    {
        if ( !Level.IsTileSuitableForLady(this.TileCoordinates + direction) )
        {
            return;
        }

        int newVerticalLayer = Level.GetTopVerticalLayer(this.TileCoordinates + direction) + 1;
        this.transform.position = Vector3Int.RoundToInt(this.transform.position + direction);
        VerticalLayer = newVerticalLayer;

        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);

        if (Level.TileHasComponent<Goal>(TileCoordinates))
        {
            Level.ReachedGoal();
        }
    }
}
