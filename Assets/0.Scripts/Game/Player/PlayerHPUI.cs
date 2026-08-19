using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] private GameObject hpBG;
    [SerializeField] private Image hpImg;

    private PlayerCondition health;

    private void Awake()
    {
        health = GetComponent<PlayerCondition>();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateHP();
    }

    private void UpdatePosition()
    {
        Vector3 pos =
            Camera.main.WorldToScreenPoint(
                transform.position
            );

        pos.y -= 5f;

        hpBG.transform.position = pos;
    }

    private void UpdateHP()
    {
        hpImg.fillAmount =
            (float)health.CurrentHP /
            health.MaxHP;
    }
}