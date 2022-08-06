using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text stonesText;
    [SerializeField] TMP_Text outOfStonesText;
    [SerializeField] GameObject gameOverMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void SetStonesText(int value)
    {
        stonesText.text = value.ToString();
    }

    public async void Won()
    {
        await Task.Delay(1500);

        gameOverMenu.SetActive(true);
    }

    public async void Lost()
    {
        await Task.Delay(500);

        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
        outOfStonesText.DOFade(1, .5f).SetEase(Ease.Linear);

        await Task.Delay(1500);

        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.InSine);
        outOfStonesText.DOFade(0, .5f).SetEase(Ease.Linear);

        await Task.Delay(1000);

        gameOverMenu.SetActive(true);
    }

    public void OnClickRetry()
    {
        Fade.Instance.FadeToScene(1);
    }

    public void OnClickHome()
    {
        Fade.Instance.FadeToScene(0);
    }

}
