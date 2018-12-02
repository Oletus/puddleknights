// Copyright Olli Etuaho 2018

using UnityEngine;
using UnityEngine.Events;

namespace LPUnityUtils
{

    // Converts axis input to discrete up/down/left/right events.
    // The axis has to return to close to zero before another input event is registered.
    // If you use this with key inputs, it's recommended to set the Gravity and Sensitivity values very high.
    class DiscretizedAxisInput : MonoBehaviour
    {
        [SerializeField] private string VerticalAxisName = "Vertical";
        [SerializeField] private string HorizontalAxisName = "Horizontal";

        [SerializeField] private float RegisterThreshold = 0.6f;
        [SerializeField] private float DeregisterThreshold = 0.5f;

        // Called with a direction vector.
        [System.NonSerialized] public UnityAction<Vector2Int> OnDirectionInput;

        private int lastVerticalSign = 0;
        private int lastHorizontalSign = 0;

        private void Awake()
        {
            Debug.Assert(RegisterThreshold >= DeregisterThreshold);
        }

        private int GetSign(string axisName, float threshold)
        {
            float value = Input.GetAxis(axisName);
            int sign = 0;
            if ( value > threshold )
            {
                sign = 1;
            }
            if (value < -threshold )
            {
                sign = -1;
            }
            return sign;
        }

        void Update()
        {
            float verticalThreshold = (lastVerticalSign == 0) ? RegisterThreshold : DeregisterThreshold;
            int verticalSign = GetSign(VerticalAxisName, verticalThreshold);
            if (verticalSign != lastVerticalSign)
            {
                lastVerticalSign = verticalSign;
                if ( OnDirectionInput != null ) {
                    if ( verticalSign == 1 ) {
                        OnDirectionInput(Vector2Int.up);
                    }
                    if ( verticalSign == -1 )
                    {
                        OnDirectionInput(Vector2Int.down);
                    }
                }
            }
            float horizontalThreshold = (lastHorizontalSign == 0) ? RegisterThreshold : DeregisterThreshold;
            int horizontalSign = GetSign(HorizontalAxisName, horizontalThreshold);
            if ( horizontalSign != lastHorizontalSign )
            {
                lastHorizontalSign = horizontalSign;
                if ( horizontalSign == 1 )
                {
                    OnDirectionInput(Vector2Int.right);
                }
                if ( horizontalSign == -1 )
                {
                    OnDirectionInput(Vector2Int.left);
                }
            }
        }
    }

}