using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : LevelObject
{
    [System.NonSerialized] public CapeTile NextCapePiece;

    private void SnapNextCapePiece()
    {
        // TODO: Sound / VFX?
        NextCapePiece = null;
    }

    protected void MoveCape(int newVerticalLayer, Vector3Int direction)
    {
        Vector3 myOldPosition = transform.position;
        transform.position += direction;

        int oldVerticalLayer = VerticalLayer;
        VerticalLayer = newVerticalLayer;

        if ( NextCapePiece != null )
        {
            if ( NextCapePiece.IsWeightOnTop() )
            {
                SnapNextCapePiece();
                Level.DropAllVerticalLayers();
            }
            else
            {
                NextCapePiece.MoveCape(oldVerticalLayer, Vector3Int.RoundToInt(myOldPosition - NextCapePiece.transform.position));
            }
        }
        else
        {
            Level.DropAllVerticalLayers();
        }
    }

    private bool IsWeightOnTop()
    {
        return Level.IsWeightInTile(this.TileCoordinates);
    }

    public bool HasLadyOnTop()
    {
        if ( Level.IsLadyInTile(this.TileCoordinates) )
        {
            return true;
        }
        if ( NextCapePiece != null )
        {
            return NextCapePiece.HasLadyOnTop();
        }
        return false;
    }
}
