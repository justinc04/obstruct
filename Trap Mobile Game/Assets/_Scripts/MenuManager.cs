using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    public AreaObject area;
    [SerializeField] float areaHeight;
    [SerializeField] float areaFloatDistance;
    [SerializeField] float areaFloatTime;
    [SerializeField] Camera cam;

    private void Start()
    {
        GameObject menuAreaObject = Instantiate(area.menuObject, new Vector3(0, areaHeight, 0), Quaternion.identity);
        menuAreaObject.transform.DOMoveY(areaHeight + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        cam.backgroundColor = area.backgroundColor;
        Fade.Instance.fadeImage.color = area.backgroundColor;
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
