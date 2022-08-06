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

    private void Awake()
    {
        Instance = this;
    }

    public void SetStonesText(int value)
    {
        stonesText.text = value.ToString();

        if (value <= 3)
        {
            stonesText.color = Color.red;
        }
    }

    public async void Lost()
    {
        await Task.Delay(500);

        stonesText.DOFade(0, .5f).SetEase(Ease.Linear);
        outOfStonesText.rectTransform.DOLocalMoveY(outOfStonesText.rectTransform.localPosition.y - 30, .5f);
        outOfStonesText.DOFade(1, .5f).SetEase(Ease.Linear);
    }
}
