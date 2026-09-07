using NUnit.Framework;
using UnityEngine;

public class CameraDirectionUtilityTests
{
    private const float Tolerance = 0.0001f;

    [Test]
    public void GetMoveDirection_ZeroInput_ReturnsZero()
    {
        Vector3 result = CameraDirectionUtility.GetMoveDirection(Vector2.zero, 45f);

        Assert.That(result, Is.EqualTo(Vector3.zero));
    }

    [Test]
    public void GetMoveDirection_ForwardAtZeroYaw_ReturnsWorldForward()
    {
        Vector3 result = CameraDirectionUtility.GetMoveDirection(Vector2.up, 0f);

        AssertVector(result, Vector3.forward);
    }

    [Test]
    public void GetMoveDirection_ForwardAtNinetyYaw_ReturnsWorldRight()
    {
        Vector3 result = CameraDirectionUtility.GetMoveDirection(Vector2.up, 90f);

        AssertVector(result, Vector3.right);
    }

    [Test]
    public void GetMoveDirection_RightAtNinetyYaw_ReturnsWorldBack()
    {
        Vector3 result = CameraDirectionUtility.GetMoveDirection(Vector2.right, 90f);

        AssertVector(result, Vector3.back);
    }

    [Test]
    public void GetMoveDirection_DiagonalInput_StaysPlanarAndClamped()
    {
        Vector3 result = CameraDirectionUtility.GetMoveDirection(Vector2.one, 37f);

        Assert.That(result.y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(result.magnitude, Is.EqualTo(1f).Within(Tolerance));
    }

    private static void AssertVector(Vector3 actual, Vector3 expected)
    {
        Assert.That(actual.x, Is.EqualTo(expected.x).Within(Tolerance));
        Assert.That(actual.y, Is.EqualTo(expected.y).Within(Tolerance));
        Assert.That(actual.z, Is.EqualTo(expected.z).Within(Tolerance));
    }
}
