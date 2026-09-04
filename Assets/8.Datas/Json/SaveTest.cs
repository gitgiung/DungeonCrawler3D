using UnityEngine;
using UnityEngine.InputSystem;

public class SaveTest : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            Save();
        }

        if (Keyboard.current.f6Key.wasPressedThisFrame)
        {
            Load();
        }

        if (Keyboard.current.f7Key.wasPressedThisFrame)
        {
            Delete();
        }
    }

    private void Save()
    {
        GameSaveData data = new()
        {
            player = new PlayerSaveData
            {
                level = playerModel.Level,
                gold = playerModel.Gold,
                exp = playerModel.Exp,
                currentHP = playerModel.CurrentHP
            }
        };

        SaveManager.Instance.Save(data);

        Debug.Log(
            $"저장 - Lv:{data.player.level}, " +
            $"Gold:{data.player.gold}, " +
            $"Exp:{data.player.exp}, " +
            $"HP:{data.player.currentHP}"
        );
    }

    private void Load()
    {
        GameSaveData data = SaveManager.Instance.Load();

        if (data == null)
            return;

        PlayerSaveData playerData = data.player;

        playerModel.LoadData(
            playerData.level,
            playerData.gold,
            playerData.exp,
            playerData.currentHP
        );

        Debug.Log(
            $"로드 - Lv:{playerModel.Level}, " +
            $"Gold:{playerModel.Gold}, " +
            $"Exp:{playerModel.Exp}, " +
            $"HP:{playerModel.CurrentHP}"
        );
    }

    private void Delete()
    {
        SaveManager.Instance.Delete();
    }
}