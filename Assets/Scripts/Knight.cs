using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CapeTile, ControllableCharacter
{
    private Level Level;

    [SerializeField] private List<Vector2Int> CapePieceOffsets;

    [SerializeField] private GameObject CapePrefab;

    public void Awake()
    {
        Level = GetComponentInParent<Level>();
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
            capeOwner.NextCapePiece = Instantiate(CapePrefab, capeOwner.transform.position + CapeOffsetToWorldSpace(offset), Quaternion.identity).GetComponent<CapeTile>();
            capeOwner = capeOwner.NextCapePiece;
        }
    }

    public void TryMove(Vector3Int direction)
    {
        if (!Level.IsTileFree(this.MinCorner + direction))
        {
            return;
        }

        MoveCape(direction);
        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
    }
}
