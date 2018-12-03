using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lady : LevelObject, ControllableCharacter
{
    public Vector3 SelectedIndicatorPosition { get { return SelectedIndicatorMarker.transform.position; } }

    [SerializeField] private AudioSource LadyNoSound;

    public bool TryMove(Vector3Int direction)
    {
        if ( !Level.IsTileSuitableForLady(this.TileCoordinates + direction) )
        {
            this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
            if ( LadyNoSound != null )
            {
                LadyNoSound.Play();
            }
            return false;
        }

        Knight prevKnight = Level.GetKnightForCapeInTile(this.TileCoordinates);

        int newVerticalLayer = Level.GetTopVerticalLayer(this.TileCoordinates + direction) + 1;
        this.transform.position = Vector3Int.RoundToInt(this.transform.position + direction);
        VerticalLayer = newVerticalLayer;

        if (prevKnight != null && Level.GetKnightForCapeInTile(this.TileCoordinates) == null)
        {
            prevKnight.PlayMyLadySound();
        }

        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);

        if (Level.TileHasComponent<Goal>(TileCoordinates))
        {
            Level.ReachedGoal();
        }
        return true;
    }
}
