// Copyright Olli Etuaho 2018

using UnityEngine;

namespace LPUnityUtils
{
    public class RandomRotationAndOffset : MonoBehaviour
    {
        public static float RandomSign
        {
            get
            {
                return Random.value > 0.5f ? 1.0f : -1.0f;
            }
        }

        [SerializeField] private RandomRotationAndOffsetConfig Config;

        void Awake()
        {
            transform.position += new Vector3(RandomSign * Config.XOffsetMult * Config.OffsetCurve.Evaluate(Random.value), 0.0f, RandomSign * Config.ZOffsetMult * Config.OffsetCurve.Evaluate(Random.value));

            transform.rotation *= Quaternion.AngleAxis(RandomSign * Config.YRotationMult * Config.RotationExtentCurve.Evaluate(Random.value), Vector3.up);
        }
    }

}  // namespace LPUnityUtils
