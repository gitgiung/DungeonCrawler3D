using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handle;
    private GameObject monster;

    private void Start()
    {
        LoadEnemy();
    }

    private void LoadEnemy()
    {
        handle =
            Addressables.LoadAssetAsync<GameObject>("Melee Skeleton");

        handle.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(
        AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("어드레서블 로드 실패");
            return;
        }

        GameObject prefab = operation.Result;

        monster = Instantiate(prefab);

        Debug.Log("어드레서블 로드 성공");
    }

    private void OnDestroy()
    {
        if (handle.IsValid())
            Addressables.Release(handle);
    }
}