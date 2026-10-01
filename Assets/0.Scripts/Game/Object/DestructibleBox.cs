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

    [System.Serializable]
    private struct DebrisPrefab
    {
        public string name;
        public GameObject prefab;
    }

    [System.Serializable]
    private struct DropEntry
    {
        [Tooltip("드롭할 프리팹입니다. 비워두면 '아무것도 드롭하지 않음'으로 처리합니다.")]
        public GameObject prefab;

        [Min(0f), Tooltip("이 항목이 선택될 가중치입니다. 값이 클수록 선택될 확률이 높아집니다.")]
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

    [SerializeField, Range(0, 100), Tooltip("제거할 파편의 비율입니다. 100이면 모든 파편을 제거합니다.")]
    private int despawnPercentage = 100;

    [SerializeField, Tooltip("Timed 방식일 때 파편이 제거되기까지의 시간(초)입니다.")]
    private float despawnTime = 5f;

    [Header("드롭")]
    [SerializeField, Tooltip("박스가 파괴될 때 선택할 드롭 후보 목록입니다. 프리팹이 비어 있는 항목은 '아무것도 없음'으로 처리됩니다.")]
    private List<DropEntry> dropEntries = new List<DropEntry>();

    [SerializeField, Tooltip("아이템이 생성될 위치입니다. 비어 있으면 박스의 현재 위치에서 생성합니다.")]
    private Transform dropPoint;

    [Header("사운드")]
    [SerializeField, Tooltip("박스가 파괴될 때 무작위로 재생할 오디오 클립 목록입니다.")]
    private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField, Range(0f, 1f), Tooltip("파괴 사운드의 기본 볼륨입니다.")]
    private float volume = 1f;

    [SerializeField, Range(0f, 0.2f), Tooltip("파괴 사운드 볼륨에 적용할 무작위 변화량입니다.")]
    private float volumeVariation = 0.1f;

    [SerializeField, Range(0f, 0.5f), Tooltip("파괴 사운드 피치에 적용할 무작위 변화량입니다.")]
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

        Debug.Log($"{gameObject.name} Damage : {damage}, HP : {currentHP}");

        if (currentHP <= 0)
        {
            Break();
        }
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
            Debug.LogWarning($"{gameObject.name}에 사용할 파편 프리팹이 없습니다.");
        }

        DropRandomItem();

        Destroy(gameObject);
    }

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
                index = Random.Range(0, Mathf.Min(3, debrisPrefabs.Count));
                break;

            default:
                index = 0;
                break;
        }

        if (index >= debrisPrefabs.Count)
        {
            index = 0;
        }

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

            debrisRigidbody.AddForce(force, ForceMode.Impulse);

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

        DespawnDebris despawnDebris = debris.GetComponent<DespawnDebris>();

        if (despawnDebris == null)
        {
            Destroy(debris, despawnTime);
            return;
        }

        AudioClip selectedClip = null;

        if (audioClips != null && audioClips.Count > 0)
        {
            selectedClip = audioClips[Random.Range(0, audioClips.Count)];
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

    private void DropRandomItem()
    {
        if (dropEntries == null || dropEntries.Count == 0)
            return;

        float totalWeight = 0f;

        foreach (DropEntry entry in dropEntries)
        {
            if (entry.weight > 0f)
            {
                totalWeight += entry.weight;
            }
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
                // prefab이 null이면 "아무것도 드롭하지 않음"
                if (entry.prefab == null)
                    return;

                Vector3 spawnPosition =
                    dropPoint != null ? dropPoint.position : transform.position;

                Instantiate(
                    entry.prefab,
                    spawnPosition,
                    Quaternion.identity
                );

                return;
            }

            randomValue -= entry.weight;
        }
    }
}
