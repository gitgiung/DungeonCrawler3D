using UnityEngine;
using System;

// Model: 런타임 중 변동되는 데이터 관리, 이를 다시 로드해주는 역할
public class PlayerModel : MonoBehaviour
{
    public event Action<int> OnGoldChanged;
    public event Action<int, int> OnHPChanged;
    public event Action<int, int> OnExpChanged;
    public event Action<int> OnLevelChanged;
    public event Action OnStatsChanged;
    public event Action<bool> OnDeathStateChanged;

    private PlayerData data;
    private int equipmentMaxHP;
    private int equipmentDamage;
    private int equipmentDefence;
    private float equipmentSpeed;

    public int MaxHP { get; private set; }
    public int AttackDamage { get; private set; }
    public int Defence { get; private set; }
    public float WalkSpeed { get; private set; }
    public float SprintSpeed { get; private set; }
    
    public bool IsDead { get; private set; }
    public int CurrentHP { get; private set; }
    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int Level { get; private set; } = 1;
    public int MaxExp => data == null ? 1 : data.GetMaxExp(Level);
    
    public void Init(PlayerData playerData)
    {
        data = playerData;
        // 데이터 로드 전 초기화
        Level = 1;
        Gold = 0;
        Exp = 0;
        equipmentDefence = 0;
        equipmentMaxHP = 0;
        equipmentDamage = 0;
        equipmentSpeed = 0f;

        CalculateStats(false, false);
        SetHP(MaxHP, false);
    }

    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public void AddExp(int amount)
    {
        if (amount <= 0 || data == null)
            return;

        // 최대 레벨에 도달하면 경험치는 0으로 고정
        if (Level >= data.MaxLevel)
        {
            Exp = 0;
            OnExpChanged?.Invoke(Exp, MaxExp);
            return;
        }

        int previousLevel = Level;
        int previousExp = Exp;
        int availableExp = Exp + amount; // 경험치 획득

        while (Level < data.MaxLevel)
        {
            int maxExp = data.GetMaxExp(Level);

            if (maxExp <= 0 || availableExp < maxExp)
                break;

            // 레벨 업 시 초과된 경험치가 이월됨
            availableExp -= maxExp;
            Level++;
        }

        // 최종 경험치
        Exp = Level >= data.MaxLevel
            ? 0
            : availableExp;

        if (Level != previousLevel)
        {
            CalculateStats(true);
            OnLevelChanged?.Invoke(Level);
        }

        if (Exp != previousExp)
            OnExpChanged?.Invoke(Exp, MaxExp);
    }

    // notify: 이벤트를 알려야 할 때만 알리기 위한 매개변수
    private void SetHP(int value, bool notify = true)
    {
        // 세팅할 HP는 0 이상 MaxHP 이하
        int setHP = Mathf.Clamp(value, 0, MaxHP);
        bool wasDead = IsDead;
        bool hpChanged = CurrentHP != setHP;

        CurrentHP = setHP;
        IsDead = CurrentHP <= 0;

        if (!notify)
            return;

        if (hpChanged)
            OnHPChanged?.Invoke(CurrentHP, MaxHP);

        if (wasDead != IsDead)
            OnDeathStateChanged?.Invoke(IsDead);
    }

    public bool ReduceHP(int damage)
    {
        if (damage <= 0 || IsDead)
            return false;

        SetHP(CurrentHP - damage);
        return true;
    }

    public void SetEquipmentStats(int maxhp, int damage, int defence, float speed)
    {
        equipmentMaxHP = Mathf.Max(0, maxhp);
        equipmentDamage = Mathf.Max(0, damage);
        equipmentDefence = Mathf.Max(0, defence);
        equipmentSpeed = Mathf.Max(0f, speed); // 이동속도 감소 아이템 설계 시 수정

        if (data == null)
            return;

        CalculateStats(false);
    }

    private bool CalculateStats(bool levelUpHeal, bool notify = true)
    {
        int previousMaxHP = MaxHP;
        bool wasDead = IsDead;
        int statsPerLevel = Mathf.Max(0, Level - 1);

        // 레벨, 장비 장착으로 인한 최종 스탯 계산
        int finalMaxHP = Mathf.Max(1,
            data.MaxHP + statsPerLevel * data.MaxHpPerLevel + equipmentMaxHP);
        int finalDamage = Mathf.Max(0,
            data.AttackDamage + statsPerLevel * data.AttackDamagePerLevel + equipmentDamage);
        int finalDefence = Mathf.Max(0, equipmentDefence);
        float finalWalkSpeed = Mathf.Max(0f, data.WalkSpeed + equipmentSpeed);
        float finalSprintSpeed = Mathf.Max(0f, data.SprintSpeed + equipmentSpeed);

        // 스탯이 바뀌었다
        bool statsChanged =
            MaxHP != finalMaxHP || AttackDamage != finalDamage || Defence != finalDefence ||
            !Mathf.Approximately(WalkSpeed, finalWalkSpeed) ||
            !Mathf.Approximately(SprintSpeed, finalSprintSpeed);

        MaxHP = finalMaxHP;
        AttackDamage = finalDamage;
        Defence = finalDefence;
        WalkSpeed = finalWalkSpeed;
        SprintSpeed = finalSprintSpeed;

        // 레벨 업하면 레벨 당 maxhp 만큼 회복, 죽은 상태에서는 안 됨
        // 장비 해제 등을 이유로 현재 HP가 maxHP보다 높아지면 currentHP = MaxHP
        if (levelUpHeal && !wasDead && MaxHP > previousMaxHP)
            SetHP(CurrentHP + data.MaxHpPerLevel, notify);
        else if (CurrentHP > MaxHP)
            SetHP(MaxHP, notify);

        if (notify && statsChanged)
            OnStatsChanged?.Invoke();

        return statsChanged;
    }

    public void LoadData(int level, int gold, int exp, int currentHP)
    {
        if (data == null)
            return;

        int previousLevel = Level;
        int previousGold = Gold;
        int previousExp = Exp;
        int previousHP = CurrentHP;

        // 로드 할 때도 동일하게 1과 최대 레벨 사이로 조정
        Level = Mathf.Clamp(level, 1, data.MaxLevel);
        Gold = Mathf.Max(0, gold);

        int maxExp = data.GetMaxExp(Level);
        Exp = Level >= data.MaxLevel
            ? 0
            : Mathf.Clamp(exp, 0, maxExp - 1);

        // 아래에서 변동을 검사해서 이벤트를 알리기 때문에 여기서 알릴 필요 없음
        bool statsChanged = CalculateStats(false, false);
        SetHP(currentHP, false);

        if (statsChanged)
            OnStatsChanged?.Invoke();
        if (previousLevel != Level)
            OnLevelChanged?.Invoke(Level);
        if (previousGold != Gold)
            OnGoldChanged?.Invoke(Gold);
        if (previousExp != Exp)
            OnExpChanged?.Invoke(Exp, MaxExp);
        if (previousHP != CurrentHP)
            OnHPChanged?.Invoke(CurrentHP, MaxHP);

    }
}
