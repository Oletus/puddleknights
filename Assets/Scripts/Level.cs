using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    List<LevelObject> LevelObjects;

    public void Awake()
    {
        LevelObjects = new List<LevelObject>(GetComponentsInChildren<LevelObject>(true));
    }

    public bool IsTileFree(Vector3Int coords)
    {
        foreach (LevelObject obj in LevelObjects )
        {
            if (!obj.IsObstacle)
            {
                continue;
            }
            if (obj.MinCorner.x == coords.x && obj.MinCorner.z == coords.z)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsTileSuitableForLady()
    {
        return false;
    }
}
