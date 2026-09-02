using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogUI : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    private Action onAccept;
    private GameState previousGameState;

    private bool isOpen;
    private bool previousCursorVisible;
    private CursorLockMode previousCursorLockMode;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        acceptButton.onClick.AddListener(Accept);
        rejectButton.onClick.AddListener(Reject);

        dialogPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        acceptButton.onClick.RemoveListener(Accept);
        rejectButton.onClick.RemoveListener(Reject);
    }

    public void Open(QuestData questData, Action acceptAction)
    {
        if (questData == null || isOpen)
            return;

        isOpen = true;
        onAccept = acceptAction;

        titleText.text = questData.questTitle;
        descriptionText.text = questData.questDescription;

        previousGameState = GameManager.Instance.State;
        GameManager.Instance.State = GameState.Pause;

        previousCursorVisible = Cursor.visible;
        previousCursorLockMode = Cursor.lockState;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialogPanel.SetActive(true);
    }

    private void Accept()
    {
        Action acceptAction = onAccept;

        Close();
        acceptAction?.Invoke();
    }

    private void Reject()
    {
        // 퀘스트를 등록하지 않고 창만 닫는다.
        Close();
    }

    private void Close()
    {
        if (!isOpen)
            return;

        isOpen = false;
        onAccept = null;

        dialogPanel.SetActive(false);

        GameManager.Instance.State = previousGameState;

        Cursor.visible = previousCursorVisible;
        Cursor.lockState = previousCursorLockMode;
    }
}