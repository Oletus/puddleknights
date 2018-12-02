using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ControllableCharacter Character;

    public Level Level;

    [SerializeField] private GameObject SelectedIndicatorPrefab;

    private GameObject SelectedIndicator;

    public void Awake()
    {
        SelectedIndicator = Instantiate(SelectedIndicatorPrefab);
    }

    public void Update()
    {
        if ( Level != null )
        {
            Character = Level.ChosenCharacter;
            if ( Input.GetKeyDown(KeyCode.Space) )
            {
                Level.SwitchCharacter();
            }
            if ( Character != null && !Level.Win )
            {
                if ( Input.GetKeyDown(KeyCode.UpArrow) )
                {
                    Character.TryMove(new Vector3Int(0, 0, 1));
                }
                else if ( Input.GetKeyDown(KeyCode.DownArrow) )
                {
                    Character.TryMove(new Vector3Int(0, 0, -1));
                }
                else if ( Input.GetKeyDown(KeyCode.LeftArrow) )
                {
                    Character.TryMove(Vector3Int.left);
                }
                else if ( Input.GetKeyDown(KeyCode.RightArrow) )
                {
                    Character.TryMove(Vector3Int.right);
                }
            }
        }
    }

    private void LateUpdate()
    {
        SelectedIndicator.transform.position = Level.ChosenCharacter.SelectedIndicatorPosition;
    }
}
