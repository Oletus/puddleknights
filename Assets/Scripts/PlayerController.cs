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
