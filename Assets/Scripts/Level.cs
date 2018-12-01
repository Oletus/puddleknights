using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if ( obj.MinCorner.x == coords.x && obj.MinCorner.z == coords.z )
            {
                objs.Add(obj);
            }
        }
        return objs;
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

    public void SwitchCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % Characters.Count;
        ChosenCharacter = Characters[currentCharacterIndex];
    }
}
