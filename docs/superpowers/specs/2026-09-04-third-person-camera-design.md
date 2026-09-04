# 3인칭 카메라 설계

- 작성일: 2026-09-04
- 상태: 사용자 승인 완료
- 대상 Scene: Assets/9.Scenes/0.TestRoom.unity
- 기준 버전: Unity 6.5, Cinemachine 3.1.7, Input System 1.19.0

## 1. 목적

플레이어의 뒤쪽 상단에서 따라가는 자유 회전식 3인칭 카메라를 구성한다. 카메라 회전과 플레이어 회전을 분리하고, WASD 이동은 카메라 방향을 기준으로 계산한다. 좁은 던전에서 벽이 시야를 가릴 때 카메라를 플레이어 쪽으로 당기며, UI 사용 중에는 카메라 입력을 중단한다.

## 2. 확정된 조작 방식

- 마우스로 카메라를 돌릴 때 플레이어는 회전하지 않는다.
- 플레이어는 실제 이동 입력이 있을 때만 이동 방향으로 부드럽게 회전한다.
- WASD 이동은 카메라 기준이다.
- 카메라는 사용자가 돌려놓은 방향을 유지한다.
- 마우스 가운데 버튼을 누르면 카메라가 플레이어 뒤로 부드럽게 복귀한다.
- 카메라 거리는 플레이 중 고정한다. 휠 줌은 이번 범위에서 제외한다.
- 일반 플레이 중 커서를 잠그고 숨기며, UI가 열리면 커서를 표시하고 카메라 입력을 중단한다.
- 벽이 카메라와 플레이어 사이에 들어오면 카메라를 플레이어 쪽으로 당긴다.

## 3. 현재 프로젝트 상태

- PlayerController는 PlayerInput의 Send Messages 방식에 맞는 OnMove, OnJump, OnDash, OnAttack, OnSprint 메서드를 사용한다.
- InputSystem_Actions의 Player 액션 맵에는 Move와 Look 액션이 이미 존재한다.
- Look은 Pointer delta와 게임패드 오른쪽 스틱에 연결되어 있다.
- PlayerController는 MoveInput을 현재 new Vector3(x, 0, y)로 변환하므로 월드 기준 이동이다.
- PlayerMovement는 CharacterController 이동, 완전한 공중 제어, 부드러운 회전, LastLookDirection 갱신을 이미 처리한다.
- TestRoom의 Main Camera에는 CinemachineBrain이 없고 CinemachineCamera와 CameraTarget도 없다.

## 4. 선택한 구조

### 4.1 Scene 구성

- 기존 Camera
  - Camera, AudioListener, UniversalAdditionalCameraData 유지
  - CinemachineBrain 추가
- 신규 CinemachineCamera
  - Tracking Target을 CameraTarget으로 설정
  - Position Control은 Third Person Follow 사용
  - 거리, 높이, 댐핑, 충돌 필터 담당
- 신규 CameraTarget
  - Scene 루트에 배치
  - 플레이어의 위치를 따라가지만 플레이어 회전을 상속하지 않음
  - CameraController가 월드 회전을 직접 설정
- 기존 [ Player ]
  - CameraController 추가
  - 기존 PlayerController, PlayerMovement, 상태 클래스 유지

### 4.2 코드 책임

#### CameraController

- Look 입력 저장
- 마우스와 게임패드 회전 계산 분리
- yaw와 pitch 보관
- pitch 제한
- CameraTarget 위치와 회전 동기화
- 수동 복귀 시작, 보간, 취소
- 카메라 기준 수평 이동 방향 계산
- gameplay/UI 상태에 따른 커서 잠금과 카메라 입력 차단
- 필수 참조 검사

#### PlayerController

- 기존 MoveInput 수집 유지
- 월드 고정 방향 생성 코드를 CameraController의 카메라 기준 방향 계산으로 교체
- 한 프레임 처리 순서를 조정
  1. 카메라 입력과 복귀 상태 갱신
  2. 카메라 기준 이동 방향 계산
  3. 상태 Tick
  4. PlayerMovement Tick
  5. 이동 이후 CameraTarget 위치와 회전 동기화
- 카메라 사용 가능 여부를 CameraController에 제공

#### PlayerMovement

- SetMovement로 전달받은 월드 방향을 사용
- 걷기, 달리기, 공중 제어, 회전, LastLookDirection 로직 유지
- 이번 기능을 위한 책임 추가 없음

#### State 클래스

- 수정하지 않는다.
- JumpState에서의 완전한 공중 제어와 상태 유지 동작을 보존한다.

## 5. 데이터 흐름

1. PlayerInput이 Move, Look, CameraRecenter 입력을 전달한다.
2. CameraController가 Look으로 yaw와 pitch를 갱신한다.
3. PlayerController가 MoveInput과 카메라 yaw를 이용해 수평 월드 방향을 계산한다.
4. PlayerMovement가 CharacterController.Move를 실행하고 이동 방향으로 플레이어를 회전한다.
5. CameraController가 CameraTarget을 이동이 끝난 플레이어 위치에 동기화한다.
6. CinemachineCamera가 CameraTarget을 추적한다.
7. CinemachineBrain이 계산된 카메라 상태를 실제 Camera에 적용한다.

카메라 기준 이동 계산은 다음 규칙을 따른다.

- forward와 right의 Y 성분을 제거한다.
- 각 방향을 다시 정규화한다.
- direction = forward * inputY + right * inputX로 합성한다.
- 합성 결과의 크기가 1을 넘으면 정규화한다.
- 입력이 없으면 Vector3.zero를 반환한다.

물리적인 카메라 Transform 대신 CameraController가 관리하는 yaw를 기준으로 방향을 계산한다. 벽 충돌로 실제 카메라 위치가 옆이나 앞으로 이동해도 플레이어 이동 방향이 흔들리지 않게 하기 위함이다.

## 6. 카메라 동작

### 6.1 기본 구도 시작값

- CameraTarget 높이 오프셋: 1.5m
- Camera Distance: 5m
- 초기 내려다보기 각도: 22.5도
- FOV: 기존 50 유지
- Shoulder Offset X: 0
- Position Damping 시작값: X 0.1, Y 0.2, Z 0.1

모든 값은 Inspector에서 조정할 수 있게 직렬화한다.

### 6.2 회전

- 마우스 델타에는 Time.deltaTime을 곱하지 않는다.
- 게임패드 스틱에는 초당 회전 속도와 Time.deltaTime을 적용한다.
- 위쪽 제한은 수평보다 10도 위까지로 한다.
- 아래쪽 제한은 플레이어를 60도 내려다보는 각도까지로 한다.
- pitch를 Clamp해 카메라 뒤집힘을 방지한다.

### 6.3 수동 복귀

- Player 액션 맵에 CameraRecenter Button 액션을 추가한다.
- 기본 바인딩은 Mouse middleButton이다.
- 누르면 yaw를 플레이어의 현재 정면으로 0.25초 동안 부드럽게 맞춘다.
- pitch는 유지한다.
- 복귀 중 Look 입력이 들어오면 즉시 취소한다.

### 6.4 벽 충돌

Third Person Follow의 내장 충돌 처리를 사용한다.

- Player 레이어 8은 충돌 필터에서 제외한다.
- 벽, 기둥, 천장, 큰 환경 Collider는 포함한다.
- 트리거와 공격 판정용 Collider는 제외한다.
- Camera Radius 시작값은 0.2다.
- Damping Into Collision은 0으로 시작해 시야 가림을 즉시 해소한다.
- Damping From Collision은 0.3으로 시작해 원래 거리로 부드럽게 돌아간다.

## 7. UI와 커서

카메라 입력 허용 조건은 GameManager.State가 Playing이고 카메라를 차단하는 UI가 비활성화된 경우다.

- 허용 상태: CursorLockMode.Locked, cursor visible false
- 차단 상태: CursorLockMode.None, cursor visible true
- 차단 시 Look 입력과 수동 복귀를 초기화한다.
- UI가 닫힌 첫 프레임의 잔여 마우스 델타를 폐기한다.
- 이번 범위에서는 기존 이동·공격 입력 체계를 전면 개편하지 않는다.
- 기존 inventoryUI 참조를 이용해 카메라 입력 차단 상태를 공유한다.

## 8. 오류 처리

다음 참조가 누락되면 CameraController를 비활성화하고 누락 항목을 명시하는 오류를 출력한다.

- PlayerController
- CameraTarget
- CinemachineCamera의 Tracking Target
- Main Camera의 CinemachineBrain

추가 규칙:

- 이동 방향의 제곱 크기가 임계값보다 작으면 Vector3.zero로 처리한다.
- 대각선 입력은 정규화해 속도 증가를 방지한다.
- 수평 투영 결과가 유효하지 않으면 마지막 유효 yaw를 사용한다.
- UI 전환 중 입력을 초기화해 갑작스러운 카메라 회전을 방지한다.

## 9. 테스트 계획

### 9.1 EditMode 테스트

카메라 기준 이동 계산을 순수 계산 단위로 검증한다.

- 기본 yaw에서 W는 월드 전방
- yaw 90도에서 W는 회전된 전방
- yaw 90도에서 D는 회전된 오른쪽
- pitch가 있어도 이동 방향 Y는 0
- 대각선 입력의 최대 크기는 1
- 입력이 없으면 Vector3.zero

### 9.2 TestRoom PlayMode 및 수동 테스트

- 정지 상태에서 카메라 360도 회전 시 플레이어 방향 유지
- 이동 시 카메라 기준 방향으로 이동하고 플레이어가 부드럽게 회전
- 점프 중 카메라를 돌려도 JumpState 유지 및 새 방향 공중 이동
- 대시 중 카메라를 돌려도 시작된 대시 방향 유지
- 가운데 버튼으로 플레이어 뒤 복귀
- 복귀 중 Look 입력으로 복귀 취소
- 벽 접근 시 카메라가 가까워지고 벽 이탈 시 원래 거리 복귀
- 인벤토리 활성화 시 카메라 정지와 커서 표시
- 인벤토리 종료 시 잔여 델타 없이 카메라 제어 복구
- 30, 60, 144 FPS에서 유사한 마우스 감도 유지

## 10. 변경 대상

- 신규: Assets/0.Scripts/Game/Camera/CameraController.cs
- 신규: CameraController EditMode 테스트 파일
- 수정: Assets/0.Scripts/Game/Player/PlayerController.cs
- 수정: Assets/InputSystem_Actions.inputactions
- 수정: Assets/9.Scenes/0.TestRoom.unity
- 설정: 기존 Camera의 CinemachineBrain
- 설정: 신규 CameraTarget과 CinemachineCamera

PlayerMovement와 Player State 클래스는 원칙적으로 수정하지 않는다.

## 11. 범위 제외

- 마우스 휠 줌
- 락온 카메라
- 조준 전용 카메라
- 카메라 흔들림
- 장애물 반투명 처리
- 카메라 설정 저장
- 게임패드 감도 설정 UI

위 기능은 기본 카메라 구현과 플레이 테스트가 끝난 후 별도 요구사항으로 다룬다.
