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
        if ( GameManager.instance.Level != null )
        {
            Character = GameManager.instance.Level.ChosenCharacter;
            if ( Input.GetKeyDown(KeyCode.Space) )
            {
                GameManager.instance.Level.SwitchCharacter();
            }
            if ( Character != null && !GameManager.instance.Level.Win )
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
        bool indicator = GameManager.instance.Level != null && GameManager.instance.Level.ChosenCharacter != null;
        SelectedIndicator.SetActive(indicator);
        if ( indicator )
        {
            SelectedIndicator.transform.position = GameManager.instance.Level.ChosenCharacter.SelectedIndicatorPosition;
        }
    }
}
