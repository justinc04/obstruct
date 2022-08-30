using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text stonesText;
    [SerializeField] TMP_Text outOfStonesText;
    [SerializeField] CanvasGroup gameOverMenu;
    [SerializeField] Image[] starImages;

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
        await Task.Delay(1300);

        OpenGameOverMenu();

        await Task.Delay(200);

        for (int i = 0; i < UnitManager.Instance.stars; i++)
        {
            await Task.Delay(400);
            starImages[i].DOColor(Color.white, .4f);
        }
    }

    public async void Lost()
    {
        await Task.Delay(700);

        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
        outOfStonesText.DOFade(1, .5f).SetEase(Ease.Linear);

        await Task.Delay(1500);

        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.InSine);
        outOfStonesText.DOFade(0, .5f).SetEase(Ease.Linear);

        await Task.Delay(1000);

        OpenGameOverMenu();
    }

    void OpenGameOverMenu()
    {
        gameOverMenu.gameObject.SetActive(true);
        gameOverMenu.DOFade(1, .3f).SetEase(Ease.Linear);
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
