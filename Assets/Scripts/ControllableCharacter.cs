using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControllableCharacter
{
    // Returns true if actually moved.
    bool TryMove(Vector3Int direction);

    Vector3 SelectedIndicatorPosition { get; }
}
