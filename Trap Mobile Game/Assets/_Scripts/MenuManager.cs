using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    private AreaObject area;
    [SerializeField] float areaHeight;
    [SerializeField] float areaFloatDistance;
    [SerializeField] float areaFloatTime;
    [SerializeField] Camera cam;

    [SerializeField] TMP_Text starProgressText;

    private void Start()
    {
        LoadData();

        GameObject menuAreaObject = Instantiate(area.menuObject, new Vector3(0, areaHeight, 0), Quaternion.identity);
        menuAreaObject.transform.DOMoveY(areaHeight + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        starProgressText.text = PlayerPrefs.GetInt($"stars{PlayerPrefs.GetInt("area")}") + "/" + area.starsToComplete;
        cam.backgroundColor = area.backgroundColor;
    }

    void LoadData()
    {
        if (PlayerPrefs.HasKey("area"))
        {
            area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("area")}");
        }
        else
        {
            PlayerPrefs.SetInt("area", 1);
            area = Resources.Load<AreaObject>("Areas/1");
        }
    }

    public void OnClickPlay()
    {
        if (!SplashScreen.isFinished)
        {
            return;
        }

        Fade.Instance.FadeToScene(1);
    }
}
