using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ControllableCharacter Character;

    [SerializeField] private GameObject SelectedIndicatorPrefab;

    private GameObject SelectedIndicator;

    public void Awake()
    {
        SelectedIndicator = Instantiate(SelectedIndicatorPrefab, transform);
    }

    public void Update()
    {
        Level level = GameManager.instance.Level;
        if ( level != null )
        {
            Character = level.ChosenCharacter;
            if ( Input.GetButtonDown("Undo") )
            {
                level.Undo();
            }
            if ( Input.GetKeyDown(KeyCode.Space) )
            {
                level.SwitchCharacter();
            }
            if ( Character != null && !level.Win )
            {
                bool moved = false;
                if ( Input.GetKeyDown(KeyCode.UpArrow) )
                {
                    moved = Character.TryMove(new Vector3Int(0, 0, 1));
                }
                else if ( Input.GetKeyDown(KeyCode.DownArrow) )
                {
                    moved = Character.TryMove(new Vector3Int(0, 0, -1));
                }
                else if ( Input.GetKeyDown(KeyCode.LeftArrow) )
                {
                    moved = Character.TryMove(Vector3Int.left);
                }
                else if ( Input.GetKeyDown(KeyCode.RightArrow) )
                {
                    moved = Character.TryMove(Vector3Int.right);
                }
                if (moved)
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
