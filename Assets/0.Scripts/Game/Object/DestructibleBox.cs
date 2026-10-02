using System.Collections.Generic;
using UnityEngine;

public class DestructibleBox : MonoBehaviour, IDamageable
{
    private enum DebrisAmount
    {
        Low,
        Medium,
        High,
        Random
    }

    private enum DespawnType
    {
        None,
        Timed
    }

    private enum DropType
    {
        None,
        Item,
        Gold
    }

    [System.Serializable]
    private struct DebrisPrefab
    {
        public string name;
        public GameObject prefab;
    }

    [System.Serializable]
    private struct DropEntry
    {
        [Tooltip("드롭 종류입니다.")]
        public DropType type;

        [Tooltip("Item 타입일 때 드롭할 프리팹입니다.")]
        public GameObject prefab;

        [Min(0f)]
        [Tooltip("이 항목이 선택될 가중치입니다.")]
        public float weight;
    }

    [Header("체력")]
    [SerializeField, Tooltip("박스의 최대 체력입니다.")]
    private int maxHP = 30;

    [Header("파편")]
    [SerializeField, Tooltip("박스가 파괴될 때 생성할 파편 프리팹 목록입니다.")]
    private List<DebrisPrefab> debrisPrefabs = new List<DebrisPrefab>();

    [SerializeField, Tooltip("박스가 파괴될 때 사용할 파편의 양을 선택합니다.")]
    private DebrisAmount debrisAmount = DebrisAmount.Medium;

    [SerializeField, Tooltip("박스가 파괴될 때 파편에 가할 힘입니다.")]
    private float breakForce = 3f;

    [SerializeField, Tooltip("파편이 위쪽으로 튀어오르도록 추가하는 힘입니다.")]
    private float upwardForce = 1.5f;

    [SerializeField, Tooltip("파편에 적용할 무작위 회전 힘입니다.")]
    private float torqueForce = 2f;

    [Header("파편 제거")]
    [SerializeField, Tooltip("파편 제거 방식을 선택합니다.")]
    private DespawnType despawnType = DespawnType.Timed;

    [SerializeField, Range(0, 100), Tooltip("제거할 파편의 비율입니다.")]
    private int despawnPercentage = 100;

    [SerializeField, Tooltip("파편이 제거되기까지의 시간입니다.")]
    private float despawnTime = 5f;

    [Header("랜덤 드롭")]
    [SerializeField, Tooltip("박스 파괴 시 선택할 드롭 목록입니다.")]
    private List<DropEntry> dropEntries = new List<DropEntry>();

    [SerializeField, Tooltip("아이템 생성 위치입니다. 비워두면 박스 위치를 사용합니다.")]
    private Transform dropPoint;

    [Header("골드 드롭")]
    [SerializeField, Min(0), Tooltip("드롭 가능한 최소 골드입니다.")]
    private int minGold = 1;

    [SerializeField, Min(0), Tooltip("드롭 가능한 최대 골드입니다.")]
    private int maxGold = 250;

    [SerializeField, Tooltip(
        "골드 프리팹 배열입니다.\n" +
        "0 = CopperCoin\n" +
        "1 = SilverCoin\n" +
        "2 = GoldCoin")]
    private GoldPickupEffect[] goldPrefabs;

    [Header("사운드")]
    [SerializeField, Tooltip("박스 파괴 시 재생할 사운드 목록입니다.")]
    private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;

    [SerializeField, Range(0f, 0.2f)]
    private float volumeVariation = 0.1f;

    [SerializeField, Range(0f, 0.5f)]
    private float pitchVariation = 0.1f;

    private int currentHP;
    private bool isBroken;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isBroken || damage <= 0)
            return;

        currentHP -= damage;

        Debug.Log(
            $"{gameObject.name} Damage : {damage}, HP : {currentHP}"
        );

        if (currentHP <= 0)
            Break();
    }

    public void Break()
    {
        if (isBroken)
            return;

        isBroken = true;

        GameObject debrisPrefab = GetDebrisPrefab();

        if (debrisPrefab != null)
        {
            GameObject debris = Instantiate(
                debrisPrefab,
                transform.position,
                transform.rotation
            );

            debris.transform.localScale = transform.localScale;

            ApplyForceToDebris(debris);
            SetupDespawn(debris);
        }
        else
        {
            Debug.LogWarning(
                $"{gameObject.name}에 사용할 파편 프리팹이 없습니다."
            );
        }

        DropRandomItem();

        Destroy(gameObject);
    }

    // --------------------------------------------------
    // 드롭
    // --------------------------------------------------

    private void DropRandomItem()
    {
        if (dropEntries == null || dropEntries.Count == 0)
            return;

        float totalWeight = 0f;

        foreach (DropEntry entry in dropEntries)
        {
            if (entry.weight > 0f)
                totalWeight += entry.weight;
        }

        if (totalWeight <= 0f)
            return;

        float randomValue = Random.Range(0f, totalWeight);

        foreach (DropEntry entry in dropEntries)
        {
            if (entry.weight <= 0f)
                continue;

            if (randomValue < entry.weight)
            {
                HandleDrop(entry);
                return;
            }

            randomValue -= entry.weight;
        }
    }

    private void HandleDrop(DropEntry entry)
    {
        switch (entry.type)
        {
            case DropType.None:
                return;

            case DropType.Item:
                DropItem(entry.prefab);
                break;

            case DropType.Gold:
                DropGold();
                break;
        }
    }

    private void DropItem(GameObject prefab)
    {
        if (prefab == null)
            return;

        Vector3 spawnPosition =
            dropPoint != null
                ? dropPoint.position
                : transform.position;

        Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    // --------------------------------------------------
    // 골드
    // --------------------------------------------------

    private void DropGold()
    {
        if (goldPrefabs == null || goldPrefabs.Length < 3)
        {
            Debug.LogError(
                $"{name}의 goldPrefabs에는 " +
                "Copper, Silver, Gold 순서로 3개의 프리팹이 필요합니다."
            );

            return;
        }

        int amount = Random.Range(
            minGold,
            maxGold + 1
        );

        if (amount <= 0)
            return;

        GoldPickupEffect prefab = GetGoldPrefab(amount);

        if (prefab == null)
            return;

        Vector3 spawnPosition =
            dropPoint != null
                ? dropPoint.position
                : transform.position + Vector3.up * 0.2f;

        GoldPickupEffect gold = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        // 프리팹의 기본 골드 가치를
        // 상자가 정한 랜덤 값으로 교체
        gold.SetAmount(amount);

        // 코인 튀어오르기
        GoldDropMotion dropMotion =
            gold.GetComponent<GoldDropMotion>();

        if (dropMotion != null)
        {
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );

            Vector3 targetPosition =
                spawnPosition + offset;

            dropMotion.Play(targetPosition);
        }
    }

    private GoldPickupEffect GetGoldPrefab(int amount)
    {
        if (amount >= 200)
            return goldPrefabs[2];

        if (amount >= 100)
            return goldPrefabs[1];

        return goldPrefabs[0];
    }

    // --------------------------------------------------
    // 파편
    // --------------------------------------------------

    private GameObject GetDebrisPrefab()
    {
        if (debrisPrefabs == null || debrisPrefabs.Count == 0)
            return null;

        int index;

        switch (debrisAmount)
        {
            case DebrisAmount.Low:
                index = 0;
                break;

            case DebrisAmount.Medium:
                index = 1;
                break;

            case DebrisAmount.High:
                index = 2;
                break;

            case DebrisAmount.Random:
                index = Random.Range(
                    0,
                    Mathf.Min(3, debrisPrefabs.Count)
                );
                break;

            default:
                index = 0;
                break;
        }

        if (index >= debrisPrefabs.Count)
            index = 0;

        return debrisPrefabs[index].prefab;
    }

    private void ApplyForceToDebris(GameObject debris)
    {
        Rigidbody[] debrisRigidbodies =
            debris.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody debrisRigidbody in debrisRigidbodies)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.2f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            Vector3 force =
                randomDirection * breakForce
                + Vector3.up * upwardForce;

            debrisRigidbody.AddForce(
                force,
                ForceMode.Impulse
            );

            debrisRigidbody.AddTorque(
                Random.insideUnitSphere * torqueForce,
                ForceMode.Impulse
            );
        }
    }

    private void SetupDespawn(GameObject debris)
    {
        if (despawnType == DespawnType.None)
            return;

        DespawnDebris despawnDebris =
            debris.GetComponent<DespawnDebris>();

        if (despawnDebris == null)
        {
            Destroy(debris, despawnTime);
            return;
        }

        AudioClip selectedClip = null;

        if (audioClips != null && audioClips.Count > 0)
        {
            selectedClip =
                audioClips[Random.Range(0, audioClips.Count)];
        }

        despawnDebris.SetVariables(
            despawnPercentage,
            despawnTime,
            selectedClip,
            volume,
            volumeVariation,
            pitchVariation
        );

        despawnDebris.BeginTimedDespawn();
    }

    private void OnValidate()
    {
        if (maxGold < minGold)
            maxGold = minGold;
    }
}