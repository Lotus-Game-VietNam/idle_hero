using Lotus.CoreFramework;
using System.Collections;
using TMPro;
using UnityEngine;

public class UILose : MonoUI
{
    public TMP_Text countdownTxt = null;



    private CanvasGroup _canvasGr = null;
    public CanvasGroup canvasGr => this.TryGetComponent(ref _canvasGr);



    private Coroutine countdownCrt = null;


    public void UpdateContent()
    {
        canvasGr.Active();

        if (countdownCrt != null)
            StopCoroutine(countdownCrt);
        countdownCrt = StartCoroutine(IECountdown());
    }

    private IEnumerator IECountdown()
    {
        countdownTxt.text = "5";
        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 4; i >= 0; i--)
        {
            countdownTxt.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        OnReturn();
    }

    public void Revive()
    {
        StopCoroutine(countdownCrt);
        canvasGr.DeActive();
        this.SendMessage(EventName.OnRevive);
    }

    public void OnReturn()
    {
        this.LoadSceneAsync(Utilities.GetCurrentFarmScene(), 0.5f);
    }
}
