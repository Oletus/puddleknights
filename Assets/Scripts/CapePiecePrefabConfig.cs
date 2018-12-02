using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CapePiecePrefabConfig")]
public class CapePiecePrefabConfig : ScriptableObject
{
    public GameObject PieceTurn;
    public GameObject PieceStraight;
    public GameObject PieceEnd;
    public GameObject PieceTurn_TornFront;
    public GameObject PieceStraight_TornBack;
    public GameObject PieceEnd_TornFront;
    public GameObject PieceTurn_TornBack;
    public GameObject PieceTurn_TornFront_TornBack;
    public GameObject PieceStraight_TornFront_TornBack;
}
