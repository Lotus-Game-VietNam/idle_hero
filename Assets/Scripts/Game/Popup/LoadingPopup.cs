using Lotus.CoreFramework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPopup : PopupBase
{
    public TMP_Text progressTxt = null;
    public Slider progressBar = null;


    private AsyncOperation _asyncOp;



    public void LoadSceneAsync(int buildIndex, float delayTime = 0.01f)
    {
        StartCoroutine(IELoadSceneAsync(buildIndex, delayTime));
    }

    protected override void UpdateContent()
    {
        
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

        yield return new WaitForEndOfFrame();
        yield return null;
    }

    //private IEnumerator IELoadSceneAsync(int scene, float delayTime)
    //{
    //    yield return new WaitForSeconds(0.1f + delayTime);

    //    Application.backgroundLoadingPriority = ThreadPriority.High;

    //    _asyncOp = SceneManager.LoadSceneAsync(scene);
    //    _asyncOp.allowSceneActivation = false;
    //    while (!_asyncOp.isDone)
    //    {
    //        progressTxt.text = (_asyncOp.progress * 100) + "%";
    //        progressBar.value = _asyncOp.progress;

    //        if (Mathf.Approximately(_asyncOp.progress, 0.9f))
    //        {
    //            _asyncOp.allowSceneActivation = true;
    //        }
    //        yield return null;
    //    }

    //    Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

    //    yield return new WaitForEndOfFrame();
    //    yield return null;
    //}
}
