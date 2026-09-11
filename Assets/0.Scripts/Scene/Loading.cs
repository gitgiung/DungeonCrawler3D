using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    //[SerializeField] private Transform loadingImage;

    public static string TargetScene { get; private set; } = string.Empty;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        TargetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op =
            SceneManager.LoadSceneAsync(TargetScene, LoadSceneMode.Single);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            loadingBar.value = progress;
            loadingText.text =
                $"Loading . . . {(progress * 100f):F0}%";

            yield return null;
        }

        loadingBar.value = 1f;
        loadingText.text = "Loading . . . 100%";

        // 필요하다면 Loading 화면을 최소 시간 동안 유지
        yield return new WaitForSeconds(2f);

        // 여기서 처음으로 0.TestRoom 활성화
        op.allowSceneActivation = true;
    }
}
