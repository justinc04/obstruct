using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Transform itemButtons;
    [SerializeField] CanvasGroup confirmPurchasePanel;
    [SerializeField] Image confirmImage;
    [SerializeField] TMP_Text confirmPriceText;
    [SerializeField] TMP_Text gemsText;
    private ItemButton selectedItem;

    public void OnClickItemButton(ItemButton item)
    {
        selectedItem = item;

        if (PlayerPrefs.GetInt($"{item.number}") == 0)
        {
            confirmPurchasePanel.interactable = true;
            confirmPurchasePanel.gameObject.SetActive(true);
            confirmPurchasePanel.DOFade(1, .15f).SetEase(Ease.Linear);
            confirmImage.sprite = selectedItem.sprite;
            confirmPriceText.text = selectedItem.price.ToString();
        }
        else if (PlayerPrefs.GetInt($"{item.number}") == 1)
        {
            for (int i = 1; i <= Resources.LoadAll("Stones").Length; i++)
            {
                if (PlayerPrefs.GetInt($"{i}") == 2)
                {
                    PlayerPrefs.SetInt($"{i}", 1);
                }
            }

            PlayerPrefs.SetInt($"{item.number}", 2);
            PlayerPrefs.SetInt("stone", item.number);
        }

        UpdateButtons();
    }

    public void OnClickConfirmPurchase()
    {
        for (int i = 1; i <= Resources.LoadAll("Stones").Length; i++)
        {
            if (PlayerPrefs.GetInt($"{i}") == 2)
            {
                PlayerPrefs.SetInt($"{i}", 1);
            }
        }

        PlayerPrefs.SetInt("gems", PlayerPrefs.GetInt("gems") - selectedItem.price);
        PlayerPrefs.SetInt($"{selectedItem.number}", 2);
        PlayerPrefs.SetInt("stone", selectedItem.number);
        confirmPurchasePanel.interactable = false;
        confirmPurchasePanel.DOFade(0, .15f).SetEase(Ease.Linear).OnComplete(() => confirmPurchasePanel.gameObject.SetActive(false));
        gemsText.text = PlayerPrefs.GetInt("gems").ToString();
        gemsText.rectTransform.DOPunchAnchorPos(10 * Vector2.up, .4f, 1, 0);
        UpdateButtons();
    }

    public void OnClickCancel()
    {
        confirmPurchasePanel.interactable = false;
        confirmPurchasePanel.DOFade(0, .15f).SetEase(Ease.Linear).OnComplete(() => confirmPurchasePanel.gameObject.SetActive(false));
    }

    void UpdateButtons()
    {
        foreach (Transform button in itemButtons)
        {
            button.GetComponent<ItemButton>().UpdateButton();
        }
    }
}
