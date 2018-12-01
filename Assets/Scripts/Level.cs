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

    public bool IsTileSuitableForLady(Vector3Int coords)
    {
        return IsTileFree(coords);
    }

    public void SwitchCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % Characters.Count;
        ChosenCharacter = Characters[currentCharacterIndex];
    }
}
