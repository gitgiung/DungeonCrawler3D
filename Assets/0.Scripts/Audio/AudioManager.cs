using UnityEngine;

public enum ClipType
{
    Click,
    Attack,
    Hit
}

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public class Clip
    {
        public ClipType type = ClipType.Click;
        public AudioClip clip;
    }

    [SerializeField] private Clip[] audioClips;
    [SerializeField] private AudioSource[] effectSources;

    private int effectPlayIndex = 0;

    public void EffectSound(ClipType clipType)
    {
        foreach (Clip clip in audioClips)
        {
            if (clip.type == clipType)
            {
                effectSources[effectPlayIndex].clip = clip.clip;
                effectSources[effectPlayIndex].Play();
                effectPlayIndex++;

                if (effectSources.Length <= effectPlayIndex)
                    effectPlayIndex = 0;

                break;
            }
        }
    }
}
