using LPUnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    private const float CAPE_THICKNESS = 0.05f;

    protected Level Level;

    [SerializeField] protected GameObject SelectedIndicatorMarker;

    protected virtual void Awake()
    {
        Level = GetComponentInParent<Level>();
    }

    // Integer coordinates for the corner of this object that has the smallest x and z coordinates.
    public virtual Vector3Int TileCoordinates {
        get
        {
            return Vector3Int.RoundToInt(transform.position);
        }
    }

    // This will offset the object vertically if it's standing on top of cape layers.
    private int _VerticalLayer = 0;
    [UndoHistoryable] public int VerticalLayer
    {
        get
        {
            return _VerticalLayer;
        }
        set
        {
            if ( this.IsAboveGround )
            {
                _VerticalLayer = value;
                this.transform.position = new Vector3(this.transform.position.x, _VerticalLayer * CAPE_THICKNESS, this.transform.position.z);
            }
        }
    }

    protected GameObject RotationPivot { get { return gameObject; } }

    [SerializeField] private bool _IsObstacle = false;
    public bool IsObstacle { get { return _IsObstacle; } }

    [SerializeField] private bool _IsAboveGround = false;
    public bool IsAboveGround { get { return _IsAboveGround; } }
}
