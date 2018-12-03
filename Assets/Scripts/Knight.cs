using LPUnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CapeTile, ControllableCharacter
{
    public Vector3 SelectedIndicatorPosition { get { return SelectedIndicatorMarker.transform.position; } }

    [SerializeField] private List<Vector2Int> CapePieceOffsets;

    [SerializeField] private GameObject CapePrefab;

    [SerializeField] private bool CanGoBackwardsWhenCaped = false;

    [SerializeField] private Material CapeMaterial;

    [SerializeField] private AudioSource KnightNoSound;

    [SerializeField] private AudioSource MyLadySound;

    [SerializeField] private SoundVariantSource MudStepSound;

    protected override void Awake()
    {
        base.Awake();

        if (CapeMaterial != null)
        {
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach ( var renderer in renderers )
            {
                MaterialUtils.SubstituteMaterial(renderer, "KnightCape", CapeMaterial);
            }
        }

        SpawnCapePieces();
    }

    private void PlaySound(AudioSource source)
    {
        if (source != null)
        {
            source.Play();
        }
    }

    public void PlayMyLadySound()
    {
        PlaySound(MyLadySound);
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
            capeOwner.NextCapePiece.CapeMaterial = CapeMaterial;
            capeOwner.NextCapePiece.RotateFrontTowards(capeOwner);
            capeOwner = capeOwner.NextCapePiece;
        }
    }

    public bool TryMove(Vector3Int direction)
    {
        if ( !CanGoBackwardsWhenCaped && this.NextCapePiece != null && Vector3.Angle(this.RotationPivot.transform.forward, direction) > 170.0f)
        {
            PlaySound(KnightNoSound);
            return false;
        }
        if (!Level.IsTileFree(this.TileCoordinates + direction))
        {
            PlaySound(KnightNoSound);
            return false;
        }
        if (this.NextCapePiece != null && this.NextCapePiece.HasLadyOnTop())
        {
            PlaySound(KnightNoSound);
            return false;
        }

        if (this.NextCapePiece != null)
        {
            this.NextCapePiece.MarkWeightsForMove();
        }

        MoveCape(null, Level.GetTopVerticalLayer(this.TileCoordinates + direction) + 1, direction);
        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);

        if (!Level.IsTileSuitableForLady(this.TileCoordinates) && Level.TileHasComponent<Puddle>(this.TileCoordinates) && this.MudStepSound != null)
        {
            this.MudStepSound.Play();
        }

        return true;
    }
}
