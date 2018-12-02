using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : LevelObject
{
    [System.NonSerialized] public CapeTile NextCapePiece;

    bool HadWeightOnTop = false;

    private void SnapNextCapePiece()
    {
        // TODO: Sound / VFX?
        NextCapePiece = null;
    }

    public void MarkWeightsForMove()
    {
        if ( IsWeightOnTop() )
        {
            HadWeightOnTop = true;
        }
        else if ( NextCapePiece != null )
        {
            NextCapePiece.MarkWeightsForMove();
        }
    }

    protected void MoveCape(int newVerticalLayer, Vector3Int direction)
    {
        Vector3 myOldPosition = transform.position;
        transform.position += direction;

        int oldVerticalLayer = VerticalLayer;
        VerticalLayer = newVerticalLayer;

        if ( NextCapePiece != null )
        {
            if ( NextCapePiece.HadWeightOnTop )
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
        return Level.TileHasComponent<Knight>(this.TileCoordinates);
    }

    public bool HasLadyOnTop()
    {
        if ( Level.TileHasComponent<Lady>(this.TileCoordinates) )
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
