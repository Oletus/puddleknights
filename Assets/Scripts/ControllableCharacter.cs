using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControllableCharacter
{
    void TryMove(Vector3Int direction);
}
