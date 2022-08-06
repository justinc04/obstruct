using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject areaObject;
    [SerializeField] float areaFloatDistance;
    [SerializeField] float areaFloatTime;

    private void Start()
    {
        areaObject.transform.DOMoveY(areaObject.transform.position.y + areaFloatDistance, areaFloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnClickPlay()
    {
        Fade.Instance.FadeToScene(1);
    }
}
