using UnityEngine;

public enum VFXType
{
    Heal
}

public class VFXManager : Singleton<VFXManager>
{
    [System.Serializable]
    public class VFXData
    {
        public VFXType type;
        public GameObject vfxObject;
    }    

    [SerializeField] private VFXData[] vfxDatas;


    // 임시 테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 pos = FindAnyObjectByType<PlayerController>().transform.position;
            ShowVFX(VFXType.Heal, pos);
        }

    }

    public void ShowVFX(VFXType vfxType, Vector3 position)
    {
        foreach (var vfx in vfxDatas)
        {
            if (vfxType == vfx.type)
            {
                Instantiate(
                    vfx.vfxObject,
                    position,
                    Quaternion.identity)
                    .transform.SetParent(transform);
                break;
            }
        }
    }
}
