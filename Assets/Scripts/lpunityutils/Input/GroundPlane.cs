// Copyright Olli Etuaho 2018

using UnityEngine;

namespace LPUnityUtils
{

    // Plane aligned with x/z dimensions.
    static class GroundPlane
    {
        // Useful for converting camera-relative controls to world space move left/right/up/down commands.
        public static Vector3Int CameraRelativeDirectionToWorldCardinalDirection(Vector3 cameraRelativeDirection, Camera camera)
        {
            Debug.Assert(Mathf.Abs(cameraRelativeDirection.y) < Mathf.Epsilon);
            Vector3 worldDirection = camera.transform.right * cameraRelativeDirection.x + camera.transform.forward * cameraRelativeDirection.z;
            return GetClosestCardinalDirection(worldDirection);
        }

        public static Vector3Int GetClosestCardinalDirection(Vector3 direction)
        {
            if ( Mathf.Abs(direction.x) > Mathf.Abs(direction.z) )
            {
                if ( direction.x > 0 )
                {
                    return Vector3Int.right;
                }
                else
                {
                    return Vector3Int.left;
                }
            }
            else
            {
                if ( direction.z > 0 )
                {
                    return new Vector3Int(0, 0, 1);
                }
                else
                {
                    return new Vector3Int(0, 0, -1);
                }
            }
        }
    }

}