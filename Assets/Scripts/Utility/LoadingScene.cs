using Lotus.CoreFramework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public TMP_Text progressTxt = null;
    public Slider progressBar = null;
    public CanvasGroup battleLoad = null;


    private AsyncOperation _asyncOp;

    public static SceneName sceneToLoad;
    public static float delayTime;



    private void Awake()
    {
        SetBattleLoadContent();
    }


    private void Start()
    {
        StartCoroutine(IELoadSceneAsync((int)sceneToLoad, delayTime));
    }

    public static void Initialized(SceneName _sceneToLoad, float _delayTime)
    {
        sceneToLoad = _sceneToLoad;
        delayTime = _delayTime;
    }

    private IEnumerator IELoadSceneAsync(int scene, float delayTime)
    {
        float countTime = 0f;
        float totalTimeDelay = delayTime + 0.1f;

        while (countTime <= totalTimeDelay)
        {
            progressTxt.text = Mathf.RoundToInt(((countTime / totalTimeDelay) * 50)) + "%";
            progressBar.value = (countTime / totalTimeDelay) * 0.5f;
            countTime += Time.deltaTime;
            yield return null;
        }

        Application.backgroundLoadingPriority = ThreadPriority.High;

        _asyncOp = SceneManager.LoadSceneAsync(scene);
        _asyncOp.allowSceneActivation = false;

        while (!_asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(_asyncOp.progress / 0.9f);
            progressTxt.text = Mathf.RoundToInt(((progress * 50) + 50)) + "%";
            progressBar.value = 0.5f + (progress * 0.5f);

            if (Mathf.Approximately(progress, 1f))
            {
                _asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }

        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
    }

    private void SetBattleLoadContent()
    {
        bool isBattle = sceneToLoad.IsOnBattle();
        if (!isBattle)
            return;

        battleLoad.Active(isBattle);

    }
}
