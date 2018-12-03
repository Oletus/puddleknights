using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;

public class PlayerController : MonoBehaviour
{
    public ControllableCharacter Character;

    [SerializeField] private GameObject SelectedIndicatorPrefab;

    private GameObject SelectedIndicator;

    public void Awake()
    {
        Input.simulateMouseWithTouches = false;
        SelectedIndicator = Instantiate(SelectedIndicatorPrefab, transform);
        GetComponent<DiscretizedAxisInput>().OnDirectionInput += OnDirectionInput;
    }

    public void Update()
    {
        Level level = GameManager.instance.Level;
        if ( level != null )
        {
            if ( Input.GetButtonDown("Undo") )
            {
                level.Undo();
            }
            if ( Input.GetButtonDown("NextCharacter") )
            {
                level.SwitchCharacter(1);
            }
            if ( Input.GetButtonDown("PreviousCharacter") )
            {
                level.SwitchCharacter(-1);
            }
            if ( Input.GetButtonDown("ResetLevel") )
            {
                level.Reset();
            }
        }
        ProcessPointerInput();
    }

    private void ProcessPointerInput()
    {
        Pointer p = Pointer.CreateOnPointerDown();
        if ( p != null )
        {
            var ray = p.GetRay(GameManager.instance.Camera);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("UI"));
            if ( hitInfo.collider != null )
            {
                ControllableCharacter ch = hitInfo.collider.GetComponentInParent<ControllableCharacter>();
                if ( ch != null )
                {
                    GameManager.instance.Level.SwitchCharacter(ch);
                }
            }
        }
    }

    public void OnUp()
    {
        OnDirectionInput(Vector2Int.up);
    }

    public void OnDown()
    {
        OnDirectionInput(Vector2Int.down);
    }

    public void OnRight()
    {
        OnDirectionInput(Vector2Int.right);
    }

    public void OnLeft()
    {
        OnDirectionInput(Vector2Int.left);
    }

    public void OnReset()
    {
        GameManager.instance.Level.Reset();
    }

    public void OnUndo()
    {
        GameManager.instance.Level.Undo();
    }

    public void OnPreviousLevel()
    {
        GameManager.instance.PreviousLevel();
    }

    public void OnNextLevel()
    {
        GameManager.instance.NextLevel();
    }

    private void OnDirectionInput(Vector2Int direction)
    {
        Level level = GameManager.instance.Level;

        if ( level != null )
        {
            Character = level.ChosenCharacter;
            if ( Character != null && !level.Win )
            {
                Vector3Int worldDirection = GroundPlane.CameraRelativeDirectionToWorldCardinalDirection(
                    new Vector3(direction.x, 0.0f, direction.y),
                    GameManager.instance.Camera);
                if ( Character.TryMove(worldDirection) )
                {
                    level.CommitToUndoHistory();
                }
            }
        }
    }

    private void LateUpdate()
    {
        bool indicator = GameManager.instance.Level != null && GameManager.instance.Level.ChosenCharacter != null;
        SelectedIndicator.SetActive(indicator);
        if ( indicator )
        {
            SelectedIndicator.transform.position = GameManager.instance.Level.ChosenCharacter.SelectedIndicatorPosition;
        }
    }
}
