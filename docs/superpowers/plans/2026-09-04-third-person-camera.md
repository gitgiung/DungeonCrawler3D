# Third-Person Camera Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a Cinemachine 3.1.7 third-person camera that follows the player from above and behind, rotates from mouse/gamepad look input, drives camera-relative movement, supports smooth manual recenter, handles wall collisions, and releases the cursor while UI blocks gameplay input.

**Architecture:** A root-level CameraTarget stores the camera yaw and pitch independently from the player. CameraController lives on the player so the existing PlayerInput Send Messages behavior can deliver Look and CameraRecenter directly to it. CameraDirectionUtility converts a planar input vector using only the camera yaw; PlayerMovement and all player states keep receiving the same world-space direction they already expect. A CinemachineCamera with CinemachineThirdPersonFollow follows CameraTarget, while CinemachineBrain drives the existing render camera.

**Tech Stack:** Unity 6 / C#; Input System 1.19.0; Cinemachine 3.1.7 namespace Unity.Cinemachine; CharacterController; NUnit through Unity Test Framework; URP 17.5.0.

**Spec:** [docs/superpowers/specs/2026-09-04-third-person-camera-design.md](../specs/2026-09-04-third-person-camera-design.md)

## Global Constraints

- Keep PlayerMovement, PlayerIdleState, PlayerMoveState, and PlayerJumpState behavior unchanged. In particular, preserve full air control and the existing smooth RotateTowards character rotation.
- Camera rotation never rotates the player by itself. The player rotates only through existing movement logic while a non-zero movement direction is applied.
- Movement uses camera yaw only. Camera pitch must never introduce vertical movement.
- Camera yaw stays where the player leaves it. There is no automatic recenter.
- Middle mouse performs a smooth yaw-only recenter toward player forward. Any new look input cancels the recenter immediately.
- Mouse look is frame-rate independent because pointer delta is already a per-frame delta; gamepad look is multiplied by unscaled delta time.
- Gameplay locks and hides the cursor. Paused or inventory UI states unlock and show it, clear pending look input, and block camera rotation/recenter.
- Use Cinemachine 3.1.7 APIs and the Unity.Cinemachine namespace. Do not add CinemachineInputAxisController; the existing PlayerInput component remains the single input source.
- Keep the first version at a fixed camera distance. Do not add wheel zoom.
- Make no unrelated formatting, prefab, state-machine, or movement changes.
- After each C# change, wait for Unity compilation to finish and resolve every Console error before proceeding.
- Use the exact commit boundaries listed below so each behavior can be reviewed or reverted independently.

---

## File Map

| Path | Action | Responsibility |
|---|---|---|
| Assets/0.Scripts/Game/Camera/CameraDirectionUtility.cs | Create | Pure camera-yaw-to-world movement conversion |
| Assets/Tests/Editor/CameraDirectionUtilityTests.cs | Create | EditMode coverage for axes, yaw, diagonal clamping, and zero input |
| Assets/0.Scripts/Game/Camera/CameraController.cs | Create | Look input, cursor state, pitch clamp, recenter, CameraTarget synchronization |
| Assets/0.Scripts/Game/Player/PlayerController.cs | Modify | Initialize/tick camera and pass camera-relative direction into existing movement |
| Assets/InputSystem_Actions.inputactions | Modify | Add CameraRecenter action and middle-mouse binding |
| Assets/0.Scenes/0.TestRoom.unity | Modify via Unity Editor | Add CameraTarget, CinemachineCamera, Third Person Follow, Brain, references, collision settings |

## Task 1: Add a Tested Camera-Relative Direction Conversion

**Files:**
- Create: Assets/Tests/Editor/CameraDirectionUtilityTests.cs
- Create: Assets/0.Scripts/Game/Camera/CameraDirectionUtility.cs

- [ ] In Unity, create Assets/Tests/Editor if it does not exist.
- [ ] Create Assets/Tests/Editor/CameraDirectionUtilityTests.cs with these tests:

~~~csharp
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
~~~

- [ ] Open Window > General > Test Runner, select EditMode, and run CameraDirectionUtilityTests.
- [ ] Confirm the red phase: compilation reports that CameraDirectionUtility does not exist. This is the intended first failure.
- [ ] Create Assets/0.Scripts/Game/Camera/CameraDirectionUtility.cs:

~~~csharp
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
~~~

- [ ] Wait for compilation, rerun all five EditMode tests, and confirm they pass.
- [ ] Confirm there are no new Unity Console errors.
- [ ] Commit exactly these two files:

~~~bash
git add Assets/0.Scripts/Game/Camera/CameraDirectionUtility.cs Assets/Tests/Editor/CameraDirectionUtilityTests.cs
git commit -m "feat: add camera-relative movement conversion"
~~~

## Task 2: Add Camera Look, Cursor, and Recenter Control

**Files:**
- Create: Assets/0.Scripts/Game/Camera/CameraController.cs
- Modify: Assets/InputSystem_Actions.inputactions

- [ ] Add this action object to the Player map actions array in Assets/InputSystem_Actions.inputactions:

~~~json
{
    "name": "CameraRecenter",
    "type": "Button",
    "id": "b8b1f6d2-62bc-4cba-9b5f-2da515944d11",
    "expectedControlType": "Button",
    "processors": "",
    "interactions": "",
    "initialStateCheck": false
}
~~~

- [ ] Add this binding object to the Player map bindings array:

~~~json
{
    "name": "",
    "id": "ca201559-02c6-4a21-b0f8-c0fd39fc74ee",
    "path": "<Mouse>/middleButton",
    "interactions": "",
    "processors": "",
    "groups": "Keyboard&Mouse",
    "action": "CameraRecenter",
    "isComposite": false,
    "isPartOfComposite": false
}
~~~

- [ ] Import the input-actions asset and confirm the Player map contains CameraRecenter with only the middle mouse binding.
- [ ] Create Assets/0.Scripts/Game/Camera/CameraController.cs with the following implementation:

~~~csharp
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

    public void Initialize(PlayerController playerController)
    {
        player = playerController;
        playerInput = GetComponent<PlayerInput>();
        yaw = transform.eulerAngles.y;
        pitch = Mathf.Clamp(initialPitch, minPitch, maxPitch);
        ValidateReferences();
        ApplyCursorState(player.CanUseCameraInput);
        SyncTarget();
    }

    public void Tick()
    {
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
        if (cameraTarget == null)
        {
            return;
        }

        Vector3 position = transform.position + Vector3.up * targetHeight;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        cameraTarget.SetPositionAndRotation(position, rotation);
    }

    public Vector3 GetMoveDirection(Vector2 input)
    {
        return CameraDirectionUtility.GetMoveDirection(input, yaw);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnCameraRecenter(InputValue value)
    {
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

    private void ValidateReferences()
    {
        Debug.Assert(cameraTarget != null, "CameraController requires CameraTarget.");
        Debug.Assert(cinemachineCamera != null, "CameraController requires CinemachineCamera.");

        if (cinemachineCamera != null && cameraTarget != null)
        {
            Debug.Assert(
                cinemachineCamera.Follow == cameraTarget,
                "CinemachineCamera Tracking Target must be CameraTarget.");
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
~~~

- [ ] In the Unity Console, confirm CameraController compiles against Cinemachine 3.1.7 and Unity.Cinemachine. If CinemachineCamera.Follow is not shown in IntelliSense, inspect the installed 3.1.7 package and use cinemachineCamera.Target.TrackingTarget only for the validation expression; do not change the serialized scene relationship.
- [ ] Confirm CameraController has public methods with these exact signatures, because later integration and PlayerInput depend on them:
  - void Initialize(PlayerController playerController)
  - void Tick()
  - void SyncTarget()
  - Vector3 GetMoveDirection(Vector2 input)
  - void OnLook(InputValue value)
  - void OnCameraRecenter(InputValue value)
- [ ] Commit the controller and input asset:

~~~bash
git add Assets/0.Scripts/Game/Camera/CameraController.cs Assets/InputSystem_Actions.inputactions
git commit -m "feat: add third-person camera input control"
~~~

## Task 3: Integrate Camera-Relative Movement Without Changing Player States

**Files:**
- Modify: Assets/0.Scripts/Game/Player/PlayerController.cs
- Verify unchanged: Assets/0.Scripts/Game/Player/Logic/PlayerMovement.cs
- Verify unchanged: Assets/0.Scripts/Game/Player/State/PlayerIdleState.cs
- Verify unchanged: Assets/0.Scripts/Game/Player/State/PlayerMoveState.cs
- Verify unchanged: Assets/0.Scripts/Game/Player/State/PlayerJumpState.cs

- [ ] Add RequireComponent next to the existing PlayerController component requirements:

~~~csharp
[RequireComponent(typeof(CameraController))]
~~~

- [ ] Add the initialized camera property:

~~~csharp
public CameraController CameraControl { get; private set; }
~~~

- [ ] Add a single public camera-input gate. Use the existing inventoryUI field and GameManager state:

~~~csharp
public bool CanUseCameraInput =>
    GameManager.Instance != null &&
    GameManager.Instance.State == GameState.Playing &&
    (inventoryUI == null || !inventoryUI.activeInHierarchy);
~~~

- [ ] In Awake, after the existing component lookups and before state use, initialize the camera:

~~~csharp
CameraControl = GetComponent<CameraController>();
CameraControl.Initialize(this);
~~~

- [ ] Replace the current world-relative movement construction:

~~~csharp
Vector3 moveDirection = new Vector3(MoveInput.x, 0f, MoveInput.y);
~~~

with:

~~~csharp
Vector3 moveDirection = CameraControl.GetMoveDirection(MoveInput);
~~~

- [ ] At the beginning of the existing gameplay Update path, call CameraControl.Tick() before calculating moveDirection. This makes movement use the yaw produced by the current frame's look input.
- [ ] Immediately after Movement.Tick(), call CameraControl.SyncTarget(). This makes CameraTarget follow the final CharacterController position from the current frame before CinemachineBrain updates.
- [ ] In every existing early-return path for pause/stop, set movement to Vector3.zero and call CameraControl.SyncTarget() before returning. Do not let the last movement vector persist while gameplay is blocked.
- [ ] Keep the current state Tick calls, movement Tick call, jump handling, input reset, and state transitions in their existing relative order. Only insert the camera calls and replace the direction calculation.
- [ ] Review the diff and confirm PlayerMovement.cs and all player state files are unchanged. The existing SetMovement normalization, air control, gravity, jump, and RotateTowards code remain the authority for physical motion and player facing.
- [ ] Enter Play Mode and verify these focused integration cases in open floor space:
  - With camera yaw at 0 degrees, W moves world forward.
  - Rotate camera roughly 90 degrees right; W moves toward camera forward.
  - With no WASD input, mouse movement rotates only the camera.
  - While moving diagonally, player speed does not exceed straight-line speed.
  - Jump, then change WASD in air; full air control still works.
- [ ] Confirm there are no new Unity Console errors.
- [ ] Commit only PlayerController:

~~~bash
git add Assets/0.Scripts/Game/Player/PlayerController.cs
git commit -m "feat: drive player movement from camera yaw"
~~~

## Task 4: Build the Cinemachine 3.1.7 Rig in the Test Scene

**Files:**
- Modify via Unity Editor: Assets/0.Scenes/0.TestRoom.unity

- [ ] Open Assets/0.Scenes/0.TestRoom.unity.
- [ ] On the existing Camera GameObject, add Cinemachine Brain. Keep the existing Camera, Audio Listener, and Universal Additional Camera Data components.
- [ ] Create a root-level empty GameObject named CameraTarget. Do not parent it under the player; CameraController writes its world position and rotation each frame.
- [ ] Create a root-level GameObject named CinemachineCamera.
- [ ] Add Cinemachine Camera to CinemachineCamera.
- [ ] Set Tracking Target to CameraTarget.
- [ ] Set the Cinemachine Camera lens Field of View to 50.
- [ ] Add Third Person Follow as Position Control.
- [ ] Configure Third Person Follow:
  - Camera Side: 0.5
  - Shoulder Offset: (0, 0, 0)
  - Vertical Arm Length: 0.5
  - Camera Distance: 5
  - Damping: (0.1, 0.2, 0.1)
- [ ] Enable Avoid Obstacles and configure:
  - Collision Filter: include the environment collision layers used by the test room and exclude Player layer 8
  - Ignore Tag: empty
  - Camera Radius: 0.2
  - Damping Into Collision: 0
  - Damping From Collision: 0.3
- [ ] On [ Player ], add CameraController.
- [ ] Assign CameraTarget to Camera Controller > Camera Target.
- [ ] Assign CinemachineCamera to Camera Controller > Cinemachine Camera.
- [ ] Keep these CameraController defaults:
  - Target Height: 1.5
  - Mouse Sensitivity: 0.1
  - Gamepad Look Speed: 120
  - Min Pitch: -10
  - Max Pitch: 60
  - Initial Pitch: 22.5
  - Recenter Duration: 0.25
- [ ] Do not add Cinemachine Input Axis Controller, Cinemachine Rotation Composer, Orbital Follow, or wheel zoom.
- [ ] Save the scene and enter Play Mode.
- [ ] Confirm the camera starts above and behind the player, follows without making CameraTarget a player child, and logs no CameraController assertion.
- [ ] Walk the player next to a wall and rotate the view so the wall would be between target and camera. Confirm the camera moves inward immediately and eases back over approximately 0.3 seconds after clearing the wall.
- [ ] Confirm the collision filter never treats the player capsule as a camera obstacle.
- [ ] Commit the saved scene:

~~~bash
git add Assets/0.Scenes/0.TestRoom.unity
git commit -m "feat: configure third-person Cinemachine rig"
~~~

## Task 5: Run the Full Acceptance Matrix

**Files:**
- Verify: all files changed in Tasks 1–4
- Update only if a test exposes a defect in the approved behavior

- [ ] Open Window > General > Test Runner and run all EditMode tests. Confirm every test passes.
- [ ] Enter Play Mode in 0.TestRoom and run this complete matrix:

| Area | Action | Expected |
|---|---|---|
| Follow | Move, jump, and land | CameraTarget follows the final player position; camera remains stable above/behind |
| Free look | Move mouse with no WASD | Camera rotates; player facing does not change |
| Movement | Rotate camera, then press W/A/S/D | Movement axes are relative to camera yaw |
| Player facing | Hold a movement direction | Existing PlayerMovement rotates player smoothly toward actual motion |
| Persistent yaw | Stop moving after free look | Camera keeps the selected yaw; no automatic return |
| Recenter | Press middle mouse | Yaw eases toward player forward; pitch is unchanged |
| Recenter cancel | Press middle mouse, then move mouse | Manual look cancels recenter immediately |
| Pitch | Move mouse vertically beyond limits | Pitch stops at -10 and 60 degrees |
| Wall | Place wall between target and camera | Camera pulls inward without clipping or pushing the player |
| Wall recovery | Clear the obstruction | Camera returns smoothly with 0.3 collision-exit damping |
| Inventory | Open inventory | Cursor unlocks/shows; camera and recenter stop |
| Inventory close | Close inventory without moving mouse | Cursor locks/hides; camera does not jump from stale delta |
| Pause/stop | Change GameManager away from Playing | Camera input stops and movement direction is cleared |
| Jump control | Change WASD while airborne | Existing full air control remains |
| Speed | Hold diagonal movement | Speed matches straight movement because input is clamped |
| Gamepad | Use right stick if a gamepad is present | Look is degrees-per-second and remains frame-rate independent |

- [ ] Test at two substantially different frame rates using Game view VSync/frame limiting or the Profiler. Confirm mouse feel is not multiplied by delta time and gamepad look speed is.
- [ ] Inspect the Console after the full run and confirm zero new errors and zero missing-reference assertions.
- [ ] Run git diff --check and confirm no whitespace errors.
- [ ] Run git status --short and confirm only intended files are changed. If fixes were required, commit each behavior-specific fix with a precise message; otherwise create no empty commit.
- [ ] Review the commit range and confirm the final history contains the four planned feature commits with no unrelated files:

~~~bash
git log --oneline --decorate -4
~~~

## Completion Criteria

Implementation is complete only when all five CameraDirectionUtility EditMode tests pass, the full Play Mode acceptance matrix passes, the Console is clean, CameraTarget and Cinemachine references survive a scene reload, and git status contains no unintended changes. Wheel zoom remains deliberately absent for this version.
