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
    [SerializeField] Color starColor;
    [SerializeField] Color noCountStarColor;

    [SerializeField] GameObject retryImage;
    [SerializeField] GameObject nextImage;

    [SerializeField] TMP_Text starsText;
    [SerializeField] TMP_Text starsEarnedText;
    [SerializeField] CanvasGroup starsEarnedPanel;

    [SerializeField] TMP_Text gemsText;
    [SerializeField] CanvasGroup gemsEarnedPanel;
    [SerializeField] TMP_Text gemsEarnedText;

    [SerializeField] CanvasGroup doubleRewardsButton;
    [SerializeField] CanvasGroup buttonsPanel;
    [SerializeField] TMP_Text rewardGrantedText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverMenu.gameObject.SetActive(false);
        gameOverMenu.alpha = 0;
        starsText.text = PlayerPrefs.GetInt("stars").ToString();
        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
    }

    public void SetStonesText(int value)
    {
        stonesText.text = value.ToString();
    }

    public async void Won()
    {
        await Task.Delay(1300);

        OpenGameOverMenu();
        retryImage.SetActive(false);
        nextImage.SetActive(true);

        await Task.Delay(400);
        Color color = PlayerPrefs.GetInt("selected area") < PlayerPrefs.GetInt("unlocked area") ? noCountStarColor : starColor;

        for (int i = 0; i < GameManager.Instance.starsEarned; i++)
        {
            await Task.Delay(400);
            starImages[i].DOColor(color, .4f).SetEase(Ease.Linear);
        }

        if (PlayerPrefs.GetInt("selected area") == PlayerPrefs.GetInt("unlocked area"))
        {
            await Task.Delay(700);
            starsEarnedText.text = "+" + GameManager.Instance.starsEarned;
            starsEarnedPanel.transform.DOLocalMoveY(starsEarnedPanel.transform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
            starsEarnedPanel.DOFade(1, .4f).SetEase(Ease.Linear);
        }
        else
        {
            gemsEarnedPanel.transform.position = starsEarnedPanel.transform.position;
        }

        await Task.Delay(700);
        gemsEarnedText.text = "+" + GameManager.Instance.gemsEarned;
        gemsEarnedPanel.transform.DOLocalMoveY(gemsEarnedPanel.transform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
        gemsEarnedPanel.DOFade(1, .4f).SetEase(Ease.Linear);

        await Task.Delay(500);

        if (PlayerPrefs.GetInt("selected area") == PlayerPrefs.GetInt("unlocked area"))
        {
            starsText.text = PlayerPrefs.GetInt("stars").ToString();
            starsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);
        }

        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
        gemsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);

        await Task.Delay(700);
        doubleRewardsButton.gameObject.SetActive(true);
        doubleRewardsButton.DOFade(1, .4f).SetEase(Ease.Linear);
        buttonsPanel.gameObject.SetActive(true);
        buttonsPanel.DOFade(1, .4f).SetEase(Ease.Linear);
    }

    public async void Lost()
    {
        await Task.Delay(700);
        stonesText.DOFade(0, .5f).SetEase(Ease.Linear);

        await Task.Delay(250);
        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
        outOfStonesText.DOFade(1, .5f).SetEase(Ease.Linear);

        await Task.Delay(1500);
        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.InSine);
        outOfStonesText.DOFade(0, .5f).SetEase(Ease.Linear);

        await Task.Delay(1000);

        OpenGameOverMenu();

        if (GameManager.Instance.gemsEarned != 0)
        {            
            await Task.Delay(700);
            gemsEarnedText.text = GameManager.Instance.gemsEarned.ToString();
            gemsEarnedPanel.transform.position = starsEarnedPanel.transform.position - 15 * Vector3.one;
            gemsEarnedPanel.transform.DOLocalMoveY(gemsEarnedPanel.transform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
            gemsEarnedPanel.DOFade(1, .4f).SetEase(Ease.Linear);

            await Task.Delay(500);
            gemsText.text = PlayerPrefs.GetInt("gems").ToString();
            gemsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);

        }

        await Task.Delay(700);
        buttonsPanel.gameObject.SetActive(true);
        buttonsPanel.DOFade(1, .4f).SetEase(Ease.Linear);
    }

    public async void DoubleReward()
    {
        doubleRewardsButton.gameObject.SetActive(false);

        await Task.Delay(250);
        rewardGrantedText.rectTransform.DOLocalMoveY(rewardGrantedText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.OutSine);
        rewardGrantedText.DOFade(1, .4f).SetEase(Ease.Linear);

        await Task.Delay(500);

        if (PlayerPrefs.GetInt("selected area") == PlayerPrefs.GetInt("unlocked area"))
        {
            starsEarnedText.text = "+" + GameManager.Instance.starsEarned;
            starsEarnedText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);
        }

        gemsEarnedText.text = "+" + GameManager.Instance.gemsEarned;
        gemsEarnedText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);

        await Task.Delay(500);

        if (PlayerPrefs.GetInt("selected area") == PlayerPrefs.GetInt("unlocked area"))
        {
            starsText.text = PlayerPrefs.GetInt("stars").ToString();
            starsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);
        }

        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
        gemsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);

        await Task.Delay(1000);
        rewardGrantedText.rectTransform.DOLocalMoveY(rewardGrantedText.rectTransform.localPosition.y - 30, .5f).SetEase(Ease.InSine);
        rewardGrantedText.DOFade(0, .4f).SetEase(Ease.Linear);
    }

    void OpenGameOverMenu()
    {
        gameOverMenu.gameObject.SetActive(true);
        gameOverMenu.DOFade(1, .3f).SetEase(Ease.Linear);
        stonesText.DOFade(0, .3f).SetEase(Ease.Linear);
    }

    public void OnClickRetry()
    {
        if (GameManager.Instance.areaUnlocked)
        {
            Fade.Instance.FadeToScene(0);
        }
        else
        {
            Fade.Instance.FadeToScene(1);
        }
    }

    public void OnClickHome()
    {
        Fade.Instance.FadeToScene(0);
    }
}
