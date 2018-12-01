using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : LevelObject
{
    CapeTile Next;

    public void Move(Vector3Int direction)
    {
        Vector3 oldPosition = transform.position;
        transform.position += direction;
        Next.transform.position = oldPosition;
    }
}
