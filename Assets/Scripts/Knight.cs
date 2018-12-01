using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : LevelObject
{
    CapeTile capeStart;

    Level Level;

    public void Awake()
    {
        Level = GetComponentInParent<Level>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(new Vector3Int(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(new Vector3Int(0, 0, -1));
        }
        else if ( Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            Move(Vector3Int.left);
        }
        else if ( Input.GetKeyDown(KeyCode.RightArrow) )
        {
            Move(Vector3Int.right);
        }
    }

    public void Move(Vector3Int direction)
    {
        if (!Level.IsTileFree(this.MinCorner + direction))
        {
            return;
        }

        this.transform.position = Vector3Int.RoundToInt(this.transform.position + direction);
        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
    }
}
