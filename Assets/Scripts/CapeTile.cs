using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;

public class CapeTile : LevelObject
{
    [UndoHistoryable] private CapeTile _NextCapePiece;
    public CapeTile NextCapePiece { get { return _NextCapePiece; } set { _NextCapePiece = value; UpdateGeometryVariantBasedOnNextPiece(); UpdateModel(); } }

    private bool _TornFromFront;
    [UndoHistoryable] private bool TornFromFront { get { return _TornFromFront;  } set { _TornFromFront = value; UpdateModel(); } }
    private bool _TornFromBack;
    [UndoHistoryable] private bool TornFromBack { get { return _TornFromBack; } set { _TornFromBack = value; UpdateModel(); } }

    [SerializeField] private CapePiecePrefabConfig PiecePrefabs;

    [SerializeField] private bool UsesCapeModel = true;

    private enum PieceGeometryVariant {
        END,
        STRAIGHT,
        TURNING_RIGHT, // The front of the piece curves to the right.
        TURNING_LEFT // The front of the piece curves to the left.
    }

    private PieceGeometryVariant _GeometryVariant = PieceGeometryVariant.END;
    [UndoHistoryable] private PieceGeometryVariant GeometryVariant { get { return _GeometryVariant; } set { _GeometryVariant = value; UpdateModel(); } }

    private GameObject PieceTurn;
    private GameObject PieceStraight;
    private GameObject PieceEnd;
    private GameObject PieceTurn_TornFront;
    private GameObject PieceStraight_TornBack;
    private GameObject PieceEnd_TornFront;
    private GameObject PieceTurn_TornBack;
    private GameObject PieceTurn_TornFront_TornBack;
    private GameObject PieceStraight_TornFront_TornBack;

    private GameObject _CurrentPieceModel;
    private GameObject CurrentPieceModel {
        get
        {
            return _CurrentPieceModel;
        }
        set
        {
            if ( value != _CurrentPieceModel )
            {
                if ( _CurrentPieceModel != null )
                {
                    _CurrentPieceModel.SetActive(false);
                }
                _CurrentPieceModel = value;
                _CurrentPieceModel.SetActive(true);
            }
        }
    }

    private bool HadWeightOnTop = false;

    protected override void Awake()
    {
        base.Awake();
        if ( PiecePrefabs != null )
        {
            PieceTurn = Instantiate(PiecePrefabs.PieceTurn, transform);
            PieceTurn.SetActive(false);
            PieceStraight = Instantiate(PiecePrefabs.PieceStraight, transform);
            PieceStraight.SetActive(false);
            PieceEnd = Instantiate(PiecePrefabs.PieceEnd, transform);
            PieceEnd.SetActive(false);
            PieceTurn_TornFront = Instantiate(PiecePrefabs.PieceTurn_TornFront, transform);
            PieceTurn_TornFront.SetActive(false);
            PieceStraight_TornBack = Instantiate(PiecePrefabs.PieceStraight_TornBack, transform);
            PieceStraight_TornBack.SetActive(false);
            PieceEnd_TornFront = Instantiate(PiecePrefabs.PieceEnd_TornFront, transform);
            PieceEnd_TornFront.SetActive(false);
            PieceTurn_TornBack = Instantiate(PiecePrefabs.PieceTurn_TornBack, transform);
            PieceTurn_TornBack.SetActive(false);
            PieceTurn_TornFront_TornBack = Instantiate(PiecePrefabs.PieceTurn_TornFront_TornBack, transform);
            PieceTurn_TornFront_TornBack.SetActive(false);
            PieceStraight_TornFront_TornBack = Instantiate(PiecePrefabs.PieceStraight_TornFront_TornBack, transform);
            PieceStraight_TornFront_TornBack.SetActive(false);
        }
        UpdateModel();
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.7f, 0.1f, 1.0f));
        if (GeometryVariant == PieceGeometryVariant.TURNING_LEFT)
        {
            Gizmos.DrawLine(Vector3.up * 0.1f, Vector3.up * 0.1f + Vector3.left * 0.5f);
        }
        else if ( GeometryVariant == PieceGeometryVariant.TURNING_RIGHT )
        {
            Gizmos.DrawLine(Vector3.up * 0.1f, Vector3.up * 0.1f + Vector3.right * 0.5f);
        }
        else if ( GeometryVariant == PieceGeometryVariant.STRAIGHT )
        {
            Gizmos.DrawLine(Vector3.up * 0.1f, Vector3.up * 0.1f + Vector3.back * 0.5f);
        }
    }

    private void UpdateModel()
    {
        if (!UsesCapeModel || PiecePrefabs == null)
        {
            return;
        }
        if (GeometryVariant == PieceGeometryVariant.END)
        {
            if ( !TornFromFront )
            {
                CurrentPieceModel = PieceEnd;
            } else
            {
                CurrentPieceModel = PieceEnd_TornFront;
            }
        }
        else if (GeometryVariant == PieceGeometryVariant.STRAIGHT)
        {
            if ( !TornFromBack && !TornFromFront )
            {
                CurrentPieceModel = PieceStraight;
            }
            else if ( TornFromBack )
            {
                CurrentPieceModel = PieceStraight_TornBack;
                CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
            }
            else if ( TornFromFront )
            {
                CurrentPieceModel = PieceStraight_TornBack;
                CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
            }
            else
            {
                CurrentPieceModel = PieceStraight_TornFront_TornBack;
            }
        }
        else 
        {
            // Turning models.
            
            if (TornFromBack && !TornFromFront)
            {
                // These variations shouldn't actually be visible with the current code.
                if ( GeometryVariant == PieceGeometryVariant.TURNING_RIGHT )
                {
                    CurrentPieceModel = PieceTurn_TornBack;
                    CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                }
                else if ( GeometryVariant == PieceGeometryVariant.TURNING_LEFT )
                {
                    CurrentPieceModel = PieceTurn_TornFront;
                    CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.up);
                }
                return;
            }
            else if ( TornFromFront && !TornFromBack )
            {
                if ( GeometryVariant == PieceGeometryVariant.TURNING_RIGHT )
                {
                    CurrentPieceModel = PieceTurn_TornFront;
                    CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                }
                else if ( GeometryVariant == PieceGeometryVariant.TURNING_LEFT )
                {
                    CurrentPieceModel = PieceTurn_TornBack;
                    CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.up);
                }
                return;
            }

            if ( !TornFromBack && !TornFromFront )
            {
                CurrentPieceModel = PieceTurn;
            }
            else if ( TornFromFront && TornFromBack )
            {
                CurrentPieceModel = PieceTurn_TornFront_TornBack;
            }
            if ( GeometryVariant == PieceGeometryVariant.TURNING_LEFT )
            {
                CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.up);
            }
            else
            {
                CurrentPieceModel.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
            }
        }
    }

    private void UpdateGeometryVariantBasedOnNextPiece()
    {
        if (TornFromBack)
        {
            GeometryVariant = PieceGeometryVariant.STRAIGHT;
            return;
        }
        if (NextCapePiece == null)
        {
            GeometryVariant = PieceGeometryVariant.END;
            return;
        }
        Vector3 nextOffset = (NextCapePiece.transform.position - this.transform.position);
        if (Vector3.Angle(nextOffset, this.transform.forward) > 170.0f)
        {
            GeometryVariant = PieceGeometryVariant.STRAIGHT;
            return;
        }

        if ( Vector3.SignedAngle(nextOffset, this.transform.forward, Vector3.up) > 0.0f )
        {
            GeometryVariant = PieceGeometryVariant.TURNING_LEFT;
        }
        else
        {
            GeometryVariant = PieceGeometryVariant.TURNING_RIGHT;
        }
    }

    private void SnapNextCapePiece()
    {
        // TODO: Sound
        // Note that TornFromBack needs to be set before resetting NextCapePiece in order to update the model correctly.
        TornFromBack = true;
        NextCapePiece.TornFromFront = true;
        NextCapePiece = null;
    }

    public void RotateFrontTowards(CapeTile tile)
    {
        Vector3 offset = (tile.transform.position - transform.position);
        offset.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(offset.normalized);
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

    protected void MoveCape(CapeTile prevPiece, int newVerticalLayer, Vector3Int direction)
    {
        Vector3 myOldPosition = transform.position;
        transform.position += direction;

        if ( prevPiece != null )
        {
            RotateFrontTowards(prevPiece);
        }

        int oldVerticalLayer = VerticalLayer;
        VerticalLayer = newVerticalLayer;

        if ( NextCapePiece != null )
        {
            if ( NextCapePiece.HadWeightOnTop )
            {
                NextCapePiece.HadWeightOnTop = false;
                SnapNextCapePiece();
                Level.DropAllVerticalLayers();
            }
            else
            {
                NextCapePiece.MoveCape(this, oldVerticalLayer, Vector3Int.RoundToInt(myOldPosition - NextCapePiece.transform.position));
                UpdateGeometryVariantBasedOnNextPiece();
                UpdateModel();
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
