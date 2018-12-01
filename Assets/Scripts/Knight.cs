﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : LevelObject, ControllableCharacter
{
    CapeTile capeStart;

    Level Level;

    public void Awake()
    {
        Level = GetComponentInParent<Level>();
    }

    public void TryMove(Vector3Int direction)
    {
        if (!Level.IsTileFree(this.MinCorner + direction))
        {
            return;
        }

        this.transform.position = Vector3Int.RoundToInt(this.transform.position + direction);
        this.RotationPivot.transform.rotation = Quaternion.LookRotation(direction);
    }
}
