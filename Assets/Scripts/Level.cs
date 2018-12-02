using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LPUnityUtils;

public class Level : MonoBehaviour
{
    private List<LevelObject> LevelObjects;
    private List<ControllableCharacter> Characters;
    private int currentCharacterIndex;

    UndoHistorian History;

    public bool Win { get; private set; }

    [System.NonSerialized] public ControllableCharacter ChosenCharacter = null;

    public void Awake()
    {
        LevelObjects = new List<LevelObject>(GetComponentsInChildren<LevelObject>(true));
        Characters = new List<ControllableCharacter>(GetComponentsInChildren<ControllableCharacter>(true));
        currentCharacterIndex = 0;
        ChosenCharacter = Characters[currentCharacterIndex];
        GameManager.instance.Level = this;

        History = GetComponent<UndoHistorian>();
        CommitToUndoHistory();
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

    public bool TileHasComponent<T>(Vector3Int coords)
        where T : Component
    {
        List<LevelObject> objects = GetObjectsAt(coords);
        foreach ( LevelObject obj in objects )
        {
            if ( obj.GetComponent<T>() )
            {
                return true;
            }
        }
        return false;
    }

    public void SwitchCharacter(int direction)
    {
        currentCharacterIndex += direction;
        WrapIndex.Wrap(ref currentCharacterIndex, Characters);
        ChosenCharacter = Characters[currentCharacterIndex];
    }

    public void ReachedGoal()
    {
        if ( !Win )
        {
            Win = true;
            Debug.Log("Goal reached!");
            GameManager.instance.OnLevelWin();
        }
    }

    public void CommitToUndoHistory()
    {
        History.Commit("", LevelObjects);
    }

    public void Undo()
    {
        History.Undo(LevelObjects);
    }
}
