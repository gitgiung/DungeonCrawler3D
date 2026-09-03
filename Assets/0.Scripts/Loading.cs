using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : Singleton<Loading>
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
            SceneManager.LoadSceneAsync(TargetScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.value = progress;
            loadingText.text = $"Loading . . . {(progress * 100f):F0}%";
            yield return null;
        }

        loadingBar.value = 1f;
        loadingText.text = $"Loading . . . 100%";

        // Scene 활성화
        op.allowSceneActivation = true;

        // Scene Load and Finish Wait
        while(!op.isDone)
            yield return null;

        // ** Target Scene을 Active로 설정 **
        Scene target = SceneManager.GetSceneByName(TargetScene);
        SceneManager.SetActiveScene(target);

        yield return new WaitForSeconds(2f);

        // Scene 삭제
        SceneManager.UnloadSceneAsync("Loading");
    }
}
