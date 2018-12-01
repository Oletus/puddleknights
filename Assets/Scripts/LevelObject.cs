using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    // Integer coordinates for the corner of this object that has the smallest x and z coordinates.
    public Vector3Int MinCorner {
        get
        {
            return Vector3Int.RoundToInt(transform.position);
        }
    }

    private int VerticalLayer;  // This will offset the object vertically if it's standing on top of cape layers.

    [SerializeField] private GameObject _RotationPivot;
    protected GameObject RotationPivot { get { return _RotationPivot; } }

    [SerializeField] private bool _IsObstacle = false;
    public bool IsObstacle { get { return _IsObstacle; } }

    [SerializeField] private bool _IsAboveGround = false;
    public bool IsAboveGround { get { return _IsAboveGround; } }
}
