// Copyright Olli Etuaho 2018

using UnityEngine;

namespace LPUnityUtils
{

    [CreateAssetMenu(fileName = "RandomRotationAndOffsetConfig")]
    public class RandomRotationAndOffsetConfig : ScriptableObject
    {
        [SerializeField] public float XOffsetMult = 0.03f;
        [SerializeField] public float ZOffsetMult = 0.03f;
        [SerializeField] public AnimationCurve OffsetCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] public float YRotationMult = 3.0f;
        [SerializeField] public AnimationCurve RotationExtentCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    }

}  // namespace LPUnityUtils
