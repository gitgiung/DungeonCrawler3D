using UnityEngine;

public static class CameraDirectionUtility
{
    public static Vector3 GetMoveDirection(Vector2 input, float yawDegrees)
    {
        Vector2 clampedInput = Vector2.ClampMagnitude(input, 1f);
        if (clampedInput.sqrMagnitude <= Mathf.Epsilon)
        {
            return Vector3.zero;
        }

        Vector3 localDirection = new Vector3(clampedInput.x, 0f, clampedInput.y);
        return Quaternion.Euler(0f, yawDegrees, 0f) * localDirection;
    }
}
