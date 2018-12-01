﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    private List<LevelObject> LevelObjects;
    private PlayerController PlayerController;
    private List<ControllableCharacter> Characters;
    private int currentCharacterIndex = 0;

    [System.NonSerialized] public ControllableCharacter ChosenCharacter = null;

    public void Awake()
    {
        LevelObjects = new List<LevelObject>(GetComponentsInChildren<LevelObject>(true));
        PlayerController = FindObjectOfType<PlayerController>();
        Characters = new List<ControllableCharacter>(GetComponentsInChildren<ControllableCharacter>(true));
        currentCharacterIndex = 0;
        ChosenCharacter = Characters[currentCharacterIndex];
    }

    private List<LevelObject> GetObjectsAt(Vector3Int coords)
    {
        List<LevelObject> objs = new List<LevelObject>();
        foreach ( LevelObject obj in LevelObjects )
        {
            if ( obj.TileCoordinates.x == coords.x && obj.TileCoordinates.z == coords.z )
            {
                objs.Add(obj);
            }
        }
        return objs;
    }

    // Get the topmost vertical layer that's taken by an object in the tile indicated by coords. If no layers are taken, returns -1.
    public int GetTopVerticalLayer(Vector3Int coords)
    {
        IEnumerable<LevelObject> objects = GetObjectsAt(coords).OrderBy(obj => obj.VerticalLayer);
        int top = -1;
        foreach ( LevelObject obj in objects )
        {
            if ( obj.IsAboveGround )
            {
                top = Mathf.Max(top, obj.VerticalLayer);
            }
        }
        return top;
    }

    public void DropAllVerticalLayers()
    {
        foreach ( LevelObject obj in LevelObjects )
        {
            IEnumerable<LevelObject> tileObjects = GetObjectsAt(obj.TileCoordinates).OrderBy(objA => objA.VerticalLayer);
            {
                int layer = 0;
                foreach ( LevelObject tileObj in tileObjects )
                {
                    if (tileObj.IsAboveGround)
                    {
                        tileObj.VerticalLayer = layer;
                        ++layer;
                    }
                }
            }
        }
    }

    public bool IsTileFree(Vector3Int coords)
    {
        List<LevelObject> objects = GetObjectsAt(coords);
        foreach ( LevelObject obj in objects )
        {
            if (obj.IsObstacle)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsTileSuitableForLady(Vector3Int coords)
    {
        List<LevelObject> objects = GetObjectsAt(coords);
        bool hadPuddle = false;
        bool hadCape = false;
        foreach ( LevelObject obj in objects )
        {
            if ( obj.IsObstacle )
            {
                return false;
            }
            if (obj.GetComponent<Puddle>())
            {
                hadPuddle = true;
            }
            if (obj.GetComponent<CapeTile>())
            {
                hadCape = true;
            }
        }
        return !hadPuddle || hadCape;
    }

    public bool IsLadyInTile(Vector3Int coords)
    {
        List<LevelObject> objects = GetObjectsAt(coords);
        foreach ( LevelObject obj in objects )
        {
            if (obj.GetComponent<Lady>())
            {
                return true;
            }
        }
        return false;
    }

    public bool IsKnightInTile(Vector3Int coords)
    {
        List<LevelObject> objects = GetObjectsAt(coords);
        foreach ( LevelObject obj in objects )
        {
            if ( obj.GetComponent<Knight>() )
            {
                return true;
            }
        }
        return false;
    }

    public void SwitchCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % Characters.Count;
        ChosenCharacter = Characters[currentCharacterIndex];
    }
}
