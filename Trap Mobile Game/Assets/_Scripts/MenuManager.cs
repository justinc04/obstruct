using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject areaObject;

    private void Start()
    {
        areaObject.transform.DOMoveY(areaObject.transform.position.y + .1f, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnClickPlay()
    {
        Fade.Instance.FadeToScene(1);
    }
}
