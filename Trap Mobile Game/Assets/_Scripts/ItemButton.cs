using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemButton : MonoBehaviour
{
    public int number;
    public int price;
    public Sprite sprite;

    [SerializeField] TMP_Text priceText;
    [SerializeField] Image image;
    [SerializeField] Image gemImage;
    [SerializeField] Button button;

    private void Start()
    {
        priceText.text = price.ToString();
        image.sprite = sprite;
        UpdateButton();
    }

    public void UpdateButton()
    {
        if (PlayerPrefs.GetInt($"{number}") == 0)
        {
            if (PlayerPrefs.GetInt("gems") >= price)
            {
                button.interactable = true;
                gemImage.DOColor(Color.white, .1f).SetEase(Ease.Linear);
            }
            else
            {
                button.interactable = false;
                gemImage.DOColor(new Color(1, 1, 1, .5f), .1f).SetEase(Ease.Linear);
            }
        }
        else
        {
            gemImage.gameObject.SetActive(false);
            priceText.rectTransform.sizeDelta = new Vector2(button.GetComponent<RectTransform>().sizeDelta.x, priceText.rectTransform.sizeDelta.y);
            priceText.rectTransform.localPosition = new Vector2(0, priceText.rectTransform.localPosition.y);
            priceText.fontSize = 40;
            priceText.alignment = TextAlignmentOptions.Center;

            if (PlayerPrefs.GetInt($"{number}") == 2)
            {
                priceText.text = "EQUIPPED";
                priceText.DOColor(Color.white, .1f).SetEase(Ease.Linear);
            }
            else if (PlayerPrefs.GetInt($"{number}") == 1)
            {
                priceText.text = "OWNED";
                priceText.DOColor(Color.grey, .1f).SetEase(Ease.Linear);
            }
        }
    }
}
