using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using DG.Tweening;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string adUnitID = "Rewarded_Android";

    [SerializeField] CanvasGroup loadingScreen;
    [SerializeField] GameObject watchAdButton;

    private void Start()
    {
        Advertisement.Initialize("4963581", true, this);
    }

    public void PlayRewardedAd()
    {
        if (Advertisement.isInitialized)
        {
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.DOFade(1, .2f).SetEase(Ease.Linear);
            Advertisement.Load(adUnitID, this);
        }
        else
        {
            watchAdButton.SetActive(false);
        }
    }

    public void OnInitializationComplete() { }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }

    public void OnUnityAdsAdLoaded(string placementId) 
    {
        Advertisement.Show(adUnitID, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) 
    {
        loadingScreen.gameObject.SetActive(false);
        watchAdButton.SetActive(false);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) 
    {
        loadingScreen.gameObject.SetActive(false);
        watchAdButton.SetActive(false);
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            loadingScreen.gameObject.SetActive(false);
            GameManager.Instance.UpdateData();
            GameManager.Instance.starsEarned *= 2;
            GameManager.Instance.gemsEarned *= 2;
            UIManager.Instance.DoubleReward();
        }
    }
}
