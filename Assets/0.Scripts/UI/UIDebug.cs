using UnityEngine;
using UnityEngine.UI;

public class UIDebug : MonoBehaviour
{
    [SerializeField] PlayerModel model;

    [SerializeField] Button hpDown;
    [SerializeField] Button expUp;
    [SerializeField] Button levelUp;
    [SerializeField] Button addGold;
    [SerializeField] Button checkStats;

    private void Awake()
    {
        hpDown.onClick.AddListener(() => { HPDown(); });
        expUp.onClick.AddListener(() => { EXPUP(); });
        levelUp.onClick.AddListener(() => { LevelUP(); });
        addGold.onClick.AddListener(() => { AddGold(); });
        checkStats.onClick.AddListener(() => { CheckStats(); });
    }

    private void HPDown()
    {
        Debug.Log("HP Down: 10");
        model.ReduceHP(10);
    }

    private void EXPUP()
    {
        Debug.Log("exp Up: 30");
        model.AddExp(30);
    }

    private void LevelUP()
    {
        Debug.Log($"exp up: {model.MaxExp}");
        model.AddExp(model.MaxExp);
    }

    private void AddGold()
    {
        Debug.Log("Add Gold: 100");
        model.AddGold(100);
    }

    private void CheckStats()
    {
        Debug.Log
            (
                $"MaxHP: {model.MaxHP}\n" +
                $"MaxEXP: {model.MaxExp}\n" +
                $"FinalDamage: {model.AttackDamage}\n" +
                $"FinalDefence: {model.Defence} \n" +
                $"FinalWalkSpeed: {model.WalkSpeed} \n" +
                $"FinalSprintSpeed: {model.SprintSpeed}"
            );
    }
}
