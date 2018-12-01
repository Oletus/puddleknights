using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CapeTile, ControllableCharacter
{
    public Vector3 SelectedIndicatorPosition { get { return SelectedIndicatorMarker.transform.position; } }

    [SerializeField] private List<Vector2Int> CapePieceOffsets;

    [SerializeField] private GameObject CapePrefab;

    [SerializeField] private bool CanGoBackwardsWhenCaped = false;

    protected override void Awake()
    {
        base.Awake();
        SpawnCapePieces();
    }

    private Vector3 CapeOffsetToWorldSpace(Vector2Int offset)
    {
        return this.RotationPivot.transform.rotation * new Vector3(offset.x, 0.0f, offset.y);
    }

    private void OnDrawGizmos()
    {
        if (this.NextCapePiece != null)
        {
            return;
        }
        Vector3 center = this.RotationPivot.transform.position;
        foreach ( Vector2Int offset in CapePieceOffsets )
        {
            Vector3 prevCenter = center;
            center += CapeOffsetToWorldSpace(offset);
            Gizmos.DrawLine(prevCenter, center);
            Gizmos.DrawWireCube(center, new Vector3(1.0f, 0.1f, 1.0f));
        }
    }

    private void SpawnCapePieces()
    {
        CapeTile capeOwner = this;
        foreach (Vector2Int offset in CapePieceOffsets)
        {
            capeOwner.NextCapePiece = Instantiate(CapePrefab, capeOwner.transform.position + CapeOffsetToWorldSpace(offset), Quaternion.identity, Level.transform).GetComponent<CapeTile>();
            capeOwner = capeOwner.NextCapePiece;
        }
    }

    public void TryMove(Vector3Int direction)
    {
        if ( !CanGoBackwardsWhenCaped && this.NextCapePiece != null && Vector3.Angle(this.RotationPivot.transform.forward, direction) > 170.0f)
        {
            return;
        }
        if (!Level.IsTileFree(this.MinCorner + direction))
        {
            return;
        }

        MoveCape(Level.GetTopVerticalLayer(this.MinCorner + direction) + 1, direction);
        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
    }
}
