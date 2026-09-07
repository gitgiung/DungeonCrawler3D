using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Target")]
    [SerializeField] private float targetHeight = 1.5f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float gamepadLookSpeed = 120f;
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float initialPitch = 22.5f;

    [Header("Recenter")]
    [SerializeField] private float recenterDuration = 0.25f;

    private PlayerController player;
    private PlayerInput playerInput;
    private Vector2 lookInput;
    private float yaw;
    private float pitch;
    private float recenterVelocity;
    private bool isRecentering;
    private bool wasInputBlocked;
    private bool hasLoggedValidationError;

    public void Initialize(PlayerController playerController)
    {
        if (!enabled)
        {
            return;
        }

        player = playerController;
        playerInput = GetComponent<PlayerInput>();
        yaw = transform.eulerAngles.y;
        pitch = Mathf.Clamp(initialPitch, minPitch, maxPitch);

        if (!ValidateReferences())
        {
            return;
        }

        ApplyCursorState(player.CanUseCameraInput);
        SyncTarget();
    }

    public void Tick()
    {
        if (!enabled)
        {
            return;
        }

        bool canUseInput = player != null && player.CanUseCameraInput;
        ApplyCursorState(canUseInput);

        if (!canUseInput)
        {
            lookInput = Vector2.zero;
            isRecentering = false;
            wasInputBlocked = true;
            return;
        }

        if (wasInputBlocked)
        {
            lookInput = Vector2.zero;
            wasInputBlocked = false;
            return;
        }

        Vector2 frameLook = lookInput;
        bool usesGamepad = playerInput != null &&
                           playerInput.currentControlScheme == "Gamepad";

        if (!usesGamepad)
        {
            lookInput = Vector2.zero;
        }

        if (frameLook.sqrMagnitude > Mathf.Epsilon)
        {
            isRecentering = false;
            float scale = usesGamepad
                ? gamepadLookSpeed * Time.unscaledDeltaTime
                : mouseSensitivity;

            yaw += frameLook.x * scale;
            pitch = Mathf.Clamp(pitch - frameLook.y * scale, minPitch, maxPitch);
            return;
        }

        if (isRecentering)
        {
            yaw = Mathf.SmoothDampAngle(
                yaw,
                player.transform.eulerAngles.y,
                ref recenterVelocity,
                recenterDuration,
                Mathf.Infinity,
                Time.unscaledDeltaTime);

            if (Mathf.Abs(Mathf.DeltaAngle(yaw, player.transform.eulerAngles.y)) < 0.1f)
            {
                yaw = player.transform.eulerAngles.y;
                recenterVelocity = 0f;
                isRecentering = false;
            }
        }
    }

    public void SyncTarget()
    {
        if (!enabled || cameraTarget == null)
        {
            return;
        }

        Vector3 position = transform.position + Vector3.up * targetHeight;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        cameraTarget.SetPositionAndRotation(position, rotation);
    }

    public Vector3 GetMoveDirection(Vector2 input)
    {
        if (!enabled)
        {
            return Vector3.zero;
        }

        return CameraDirectionUtility.GetMoveDirection(input, yaw);
    }

    public void OnLook(InputValue value)
    {
        if (!enabled)
        {
            return;
        }

        if (player == null || !player.CanUseCameraInput)
        {
            lookInput = Vector2.zero;
            return;
        }

        lookInput = value.Get<Vector2>();
    }

    public void OnCameraRecenter(InputValue value)
    {
        if (!enabled)
        {
            return;
        }

        if (value.isPressed && player != null && player.CanUseCameraInput)
        {
            recenterVelocity = 0f;
            isRecentering = true;
        }
    }

    private void ApplyCursorState(bool cameraInputEnabled)
    {
        Cursor.lockState = cameraInputEnabled
            ? CursorLockMode.Locked
            : CursorLockMode.None;
        Cursor.visible = !cameraInputEnabled;
    }

    private bool ValidateReferences()
    {
        List<string> missingReferences = new List<string>();

        if (player == null)
        {
            missingReferences.Add("PlayerController");
        }

        if (cameraTarget == null)
        {
            missingReferences.Add("CameraTarget");
        }

        if (cinemachineCamera == null)
        {
            missingReferences.Add("CinemachineCamera");
        }
        else if (cinemachineCamera.Target.TrackingTarget != cameraTarget)
        {
            missingReferences.Add("CinemachineCamera Tracking Target");
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null || mainCamera.GetComponent<CinemachineBrain>() == null)
        {
            missingReferences.Add("Main Camera CinemachineBrain");
        }

        if (missingReferences.Count == 0)
        {
            return true;
        }

        if (!hasLoggedValidationError)
        {
            Debug.LogError(
                $"CameraController missing required references: {string.Join(", ", missingReferences)}.",
                this);
            hasLoggedValidationError = true;
        }

        enabled = false;
        return false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
