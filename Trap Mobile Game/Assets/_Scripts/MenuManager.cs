using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AreaObject area;
    [SerializeField] float areaFloatDistance;
    [SerializeField] float areaFloatTime;
    [SerializeField] Transform camPivot;
    [SerializeField] Camera cam;
    [SerializeField] Light lightSource;

    [SerializeField] CanvasGroup[] menus;
    [SerializeField] CanvasGroup areaSelectMenu;
    [SerializeField] CanvasGroup startGameButton;
    [SerializeField] CanvasGroup unlockAtPanel;
    [SerializeField] TMP_Text unlockAtText;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [SerializeField] TMP_Text areaNameText;
    [SerializeField] TMP_Text areaSizeText;
    [SerializeField] TMP_Text starsText;
    [SerializeField] TMP_Text gemsText;

    [SerializeField] float areaSelectSensitivity;
    [SerializeField] GameObject[] menuAreaObjectPrefabs;
    [SerializeField] float menuAreaObjectDistance;
    private List<GameObject> menuAreaObjects = new List<GameObject>();
    private float initialTouchPos;

    private void Start()
    {
        LoadData();

        if (PlayerPrefs.GetInt("selected area") > PlayerPrefs.GetInt("unlocked area"))
        {
            PlayerPrefs.SetInt("selected area", PlayerPrefs.GetInt("unlocked area"));
        }

        ChangeArea();

        for (int i = 0; i < menuAreaObjectPrefabs.Length; i++)
        {
            menuAreaObjects.Add(Instantiate(menuAreaObjectPrefabs[i], new Vector3(menuAreaObjectDistance * i, 0, -menuAreaObjectDistance * i), Quaternion.identity));
            menuAreaObjects[i].transform.DOMoveY(areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

            if (i != PlayerPrefs.GetInt("selected area") - 1)
            {
                menuAreaObjects[i].transform.localScale = Vector3.zero;
            }
        }

        areaNameText.text = area.areaName;
        areaSizeText.text = area.gridSize + "x" + area.gridSize;
        cam.backgroundColor = area.backgroundColor;
        camPivot.transform.position = new Vector3((PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance, 0, -(PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance);
        startGameButton.gameObject.SetActive(PlayerPrefs.GetInt("selected area") <= PlayerPrefs.GetInt("unlocked area"));
    }

    private void Update()
    {
        if (!areaSelectMenu.interactable)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.deltaPosition;
            cam.transform.Translate(-touchPos.x * areaSelectSensitivity * Time.deltaTime, 0, 0);

            if (touch.phase == TouchPhase.Began)
            {
                cam.transform.DOPause();
                initialTouchPos = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                cam.transform.DOLocalMoveX(0, .5f).SetEase(Ease.OutSine);

                if (Mathf.Abs(initialTouchPos - touch.position.x) > 120)
                {
                    int direction = (int)Mathf.Sign(initialTouchPos - touch.position.x);

                    if (PlayerPrefs.GetInt("selected area") + direction == 0 || PlayerPrefs.GetInt("selected area") + direction == menuAreaObjectPrefabs.Length + 1)
                    {
                        return;
                    }

                    PlayerPrefs.SetInt("selected area", PlayerPrefs.GetInt("selected area") + direction);
                    ChangeArea();
                }
            }
        }
    }

    void LoadData()
    {
        if (!PlayerPrefs.HasKey("unlocked area"))
        {
            PlayerPrefs.SetInt("unlocked area", 1);
        }

        if (!PlayerPrefs.HasKey("selected area"))
        {
            PlayerPrefs.SetInt("selected area", 1);
        }

        if (!PlayerPrefs.HasKey("stars"))
        {
            PlayerPrefs.SetInt("stars", 0);
        }

        if (!PlayerPrefs.HasKey("gems"))
        {
            PlayerPrefs.SetInt("gems", 0);
        }

        if (!PlayerPrefs.HasKey("1"))
        {
            PlayerPrefs.SetInt("1", 2);
            PlayerPrefs.SetInt("stone", 1);

            for (int i = 2; i <= Resources.LoadAll("Stones").Length; i++)
            {
                PlayerPrefs.SetInt($"{i}", 0);
            }
        }

        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("selected area")}");
        starsText.text = PlayerPrefs.GetInt("stars").ToString();
        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
    }

    async void ChangeArea()
    {
        Vector3 newPos = new Vector3((PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance, 0, -(PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance);
        camPivot.DOMove(newPos, .5f).SetEase(Ease.OutSine);
        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("selected area")}");
        areaNameText.DOFade(0, .15f).SetEase(Ease.Linear);
        areaSizeText.DOFade(0, .15f).SetEase(Ease.Linear);
        unlockAtPanel.DOFade(0, .15f).SetEase(Ease.Linear);
        cam.DOColor(area.backgroundColor, .3f).SetEase(Ease.Linear);

        await Task.Delay(150);
        areaNameText.text = area.areaName;
        areaSizeText.text = area.gridSize + "x" + area.gridSize;
        areaNameText.DOFade(1, .15f).SetEase(Ease.Linear);
        areaSizeText.DOFade(1, .15f).SetEase(Ease.Linear);

        if (PlayerPrefs.GetInt("selected area") > PlayerPrefs.GetInt("unlocked area"))
        {
            unlockAtText.text = "Unlock at " + area.starsToUnlock;
            startGameButton.DOFade(0, .15f).SetEase(Ease.Linear);
            unlockAtPanel.DOFade(1, .15f).SetEase(Ease.Linear);
            lightSource.DOIntensity(.6f, .15f).SetEase(Ease.Linear);
        }
        else
        {
            startGameButton.DOFade(1, .2f).SetEase(Ease.Linear);
            lightSource.DOIntensity(1, .2f).SetEase(Ease.Linear);
        }

        if (PlayerPrefs.GetInt("selected area") == 1)
        {
            leftButton.interactable = false;
        }
        else if (PlayerPrefs.GetInt("selected area") == menuAreaObjectPrefabs.Length)
        {
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    public void OnClickPlay(CanvasGroup menu)
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        OpenMenu(menu);
        cam.transform.DOLocalMoveY(.1f, .6f).SetEase(Ease.OutSine);
        cam.DOOrthoSize(6, .6f);

        foreach (GameObject areaObject in menuAreaObjects)
        {
            if (areaObject == menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1])
            {
                continue;
            }

            areaObject.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutSine);
        }
    }

    public void OnClickShopSettings(CanvasGroup menu)
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        OpenMenu(menu);
        menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1].transform.DOPause();
        menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1].transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InSine);
    }

    public void OnClickBackAreaSelect(CanvasGroup menu)
    {
        if (PlayerPrefs.GetInt("selected area") > PlayerPrefs.GetInt("unlocked area"))
        {
            PlayerPrefs.SetInt("selected area", PlayerPrefs.GetInt("unlocked area"));
            ChangeArea();
        }

        OpenMenu(menu);
        cam.transform.DOLocalMoveY(-1.2f, .6f).SetEase(Ease.OutSine);
        cam.DOOrthoSize(7, .6f);
        cam.transform.DOLocalMoveX(0, .5f).SetEase(Ease.OutSine);

        foreach (GameObject areaObject in menuAreaObjects)
        {
            if (areaObject == menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1])
            {
                continue;
            }

            areaObject.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InSine);
        }
    }

    public async void OnClickBackShopSettings(CanvasGroup menu)
    {
        OpenMenu(menu);

        await Task.Delay(200);
        menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1].transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutSine);

        await Task.Delay(400);
        menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1].transform.DOMoveY(menuAreaObjects[PlayerPrefs.GetInt("selected area") - 1].transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    async void OpenMenu(CanvasGroup menu)
    {
        foreach (CanvasGroup m in menus)
        {
            m.interactable = false;
            m.DOFade(0, .3f).SetEase(Ease.Linear).OnComplete(() => m.gameObject.SetActive(false));
        }

        await Task.Delay(300);
        menu.gameObject.SetActive(true);
        menu.interactable = true;
        menu.DOFade(1, .3f).SetEase(Ease.Linear);
    }

    public void OnClickArrow(int direction)
    {
        if (PlayerPrefs.GetInt("selected area") + direction == 0 || PlayerPrefs.GetInt("selected area") + direction == menuAreaObjectPrefabs.Length + 1)
        {
            return;
        }

        PlayerPrefs.SetInt("selected area", PlayerPrefs.GetInt("selected area") + direction);
        ChangeArea();
    }

    public void OnClickStartGame()
    {
        if (PlayerPrefs.GetInt("selected area") > PlayerPrefs.GetInt("unlocked area"))
        {
            return;
        }

        areaSelectMenu.interactable = false;
        Fade.Instance.FadeToScene(1);
    }
}
