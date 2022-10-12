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

    [SerializeField] CanvasGroup[] menus;
    [SerializeField] CanvasGroup areaSelectMenu;
    [SerializeField] CanvasGroup startGameButton;

    [SerializeField] TMP_Text areaNameText;
    [SerializeField] TMP_Text areaSizeText;

    [SerializeField] TMP_Text gemsText;

    [SerializeField] float areaSelectSensitivity;
    [SerializeField] GameObject[] menuAreaObjectPrefabs;
    [SerializeField] float menuAreaObjectDistance;
    private List<GameObject> menuAreaObjects = new List<GameObject>();
    private float initialCamPos;

    private void Start()
    {
        LoadData();

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
                initialCamPos = cam.transform.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                cam.transform.DOLocalMoveX(0, .5f).SetEase(Ease.OutSine);

                if (Mathf.Abs(cam.transform.position.x - initialCamPos) > .3f)
                {
                    int direction = (int)Mathf.Sign(cam.transform.position.x - initialCamPos);

                    if (PlayerPrefs.GetInt("selected area") + direction == 0 || PlayerPrefs.GetInt("selected area") + direction == menuAreaObjectPrefabs.Length + 1)
                    {
                        return;
                    }

                    PlayerPrefs.SetInt("selected area", PlayerPrefs.GetInt("selected area") + direction);
                    Vector3 newPos = new Vector3((PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance, 0, -(PlayerPrefs.GetInt("selected area") - 1) * menuAreaObjectDistance);
                    camPivot.DOMove(newPos, .5f).SetEase(Ease.OutSine);

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

        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("selected area", 1)}");
        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
    }

    async void ChangeArea()
    {
        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("selected area")}");
        areaNameText.DOFade(0, .2f).SetEase(Ease.Linear);
        areaSizeText.DOFade(0, .2f).SetEase(Ease.Linear);
        startGameButton.DOFade(0, .2f).SetEase(Ease.Linear);
        cam.DOColor(area.backgroundColor, .4f).SetEase(Ease.Linear);

        await Task.Delay(200);
        areaNameText.text = area.areaName;
        areaSizeText.text = area.gridSize + "x" + area.gridSize;
        areaNameText.DOFade(1, .2f).SetEase(Ease.Linear);
        areaSizeText.DOFade(1, .2f).SetEase(Ease.Linear);

        if (PlayerPrefs.GetInt("selected area") <= PlayerPrefs.GetInt("unlocked area"))
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.DOFade(1, .2f).SetEase(Ease.Linear);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
        }
    }

    public void OnClickPlay(CanvasGroup menu)
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        OpenMenu(menu);
        cam.transform.DOLocalMoveY(.25f, .6f).SetEase(Ease.OutSine);
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

    public void OnClickStoreSettings(CanvasGroup menu)
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
        OpenMenu(menu);
        cam.transform.DOLocalMoveY(-1.25f, .6f).SetEase(Ease.OutSine);
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

    public async void OnClickBackStoreSettings(CanvasGroup menu)
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
