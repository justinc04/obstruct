using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AreaObject area;
    [SerializeField] float areaHeight;
    [SerializeField] float areaFloatDistance;
    [SerializeField] float areaFloatTime;
    [SerializeField] Camera cam;

    [SerializeField] CanvasGroup[] menus;

    [SerializeField] TMP_Text areaNameText;
    [SerializeField] TMP_Text starProgressText;

    [SerializeField] TMP_Text gemsText;

    private GameObject menuAreaObject;

    private void Start()
    {
        LoadData();

        menuAreaObject = Instantiate(area.menuObject, new Vector3(0, areaHeight, 0), Quaternion.identity);
        menuAreaObject.transform.DOMoveY(menuAreaObject.transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        areaNameText.text = area.areaName;
        starProgressText.text = PlayerPrefs.GetInt($"stars{PlayerPrefs.GetInt("area")}") + "/" + area.starsToComplete;
        cam.backgroundColor = area.backgroundColor;
    }

    void LoadData()
    {
        if (!PlayerPrefs.HasKey("area"))
        {
            PlayerPrefs.SetInt("area", 1);
        }

        if (!PlayerPrefs.HasKey("gems"))
        {
            PlayerPrefs.SetInt("gems", 0);
        }

        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("area", 1)}");
        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
    }

    public async void OnClickPlay(CanvasGroup menu)
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        OpenMenu(menu);
        menuAreaObject.transform.DOPause();
        menuAreaObject.transform.DOMoveY(0, .6f).SetEase(Ease.OutSine);
        cam.DOOrthoSize(6, .6f);

        await Task.Delay(600);
        menuAreaObject.transform.DOMoveY(menuAreaObject.transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnClickStoreSettings(CanvasGroup menu)
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        OpenMenu(menu);
        menuAreaObject.transform.DOPause();
        menuAreaObject.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InSine);
    }

    public async void OnClickBackAreaSelect(CanvasGroup menu)
    {
        OpenMenu(menu);
        menuAreaObject.transform.DOPause();
        menuAreaObject.transform.DOMoveY(areaHeight, .6f).SetEase(Ease.OutSine);
        cam.DOOrthoSize(7, .6f);

        await Task.Delay(600);
        menuAreaObject.transform.DOMoveY(menuAreaObject.transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public async void OnClickBackStoreSettings(CanvasGroup menu)
    {
        OpenMenu(menu);

        await Task.Delay(200);
        menuAreaObject.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutSine);

        await Task.Delay(400);
        menuAreaObject.transform.DOMoveY(menuAreaObject.transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    async void OpenMenu(CanvasGroup menu)
    {
        foreach (CanvasGroup m in menus)
        {
            m.DOFade(0, .3f).SetEase(Ease.Linear).OnComplete(() => m.gameObject.SetActive(false));
        }

        await Task.Delay(300);
        menu.gameObject.SetActive(true);
        menu.DOFade(1, .3f).SetEase(Ease.Linear);
    }

    public void OnClickStartGame()
    {
        Fade.Instance.FadeToScene(1);
    }

    public void OnClickReset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
