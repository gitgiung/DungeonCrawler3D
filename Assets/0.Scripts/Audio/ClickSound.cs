using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    [SerializeField] private ClipType clipType;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.EffectSound(clipType);
        });
    }
}
