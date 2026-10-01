using System.Collections;
using UnityEngine;

public class DespawnDebris : MonoBehaviour
{
    private int despawnPercentage;
    private float despawnTime;

    private AudioClip clip;
    private float volume;
    private float volumeVariation;
    private float pitchVariation;

    private AudioSource audioSource;

    /// <summary>
    /// DestructibleBox에서 파편 제거 및 사운드에 필요한 값을 전달받습니다.
    /// </summary>
    public void SetVariables(
        int despawnPercentage,
        float despawnTime,
        AudioClip clip,
        float volume,
        float volumeVariation,
        float pitchVariation)
    {
        this.despawnPercentage = despawnPercentage;
        this.despawnTime = despawnTime;
        this.clip = clip;
        this.volume = volume;
        this.volumeVariation = volumeVariation;
        this.pitchVariation = pitchVariation;
    }

    private void Start()
    {
        PlayBreakSound();
    }

    /// <summary>
    /// 설정된 시간이 지나면 파편을 제거하는 코루틴을 시작합니다.
    /// </summary>
    public void BeginTimedDespawn()
    {
        StartCoroutine(DespawnCoroutine());
    }

    /// <summary>
    /// 설정된 비율만큼 파편을 제거합니다.
    /// </summary>
    private void DespawnDebrisObjects()
    {
        int despawnCount = transform.childCount * despawnPercentage / 100;

        for (int i = transform.childCount - 1;
             i >= transform.childCount - despawnCount;
             i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (despawnPercentage >= 100)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 설정된 시간만큼 기다린 뒤 파편을 제거합니다.
    /// </summary>
    private IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(despawnTime);
        DespawnDebrisObjects();
    }

    /// <summary>
    /// 파편 프리팹에 AudioSource가 있고 오디오 클립이 설정되어 있을 때 파괴 사운드를 재생합니다.
    /// </summary>
    private void PlayBreakSound()
    {
        if (clip == null)
            return;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            return;

        audioSource.pitch =
            1f + Random.Range(-pitchVariation / 2f, pitchVariation / 2f);

        float playVolume =
            Mathf.Clamp01(volume + Random.Range(-volumeVariation, volumeVariation));

        audioSource.PlayOneShot(clip, playVolume);
    }
}
