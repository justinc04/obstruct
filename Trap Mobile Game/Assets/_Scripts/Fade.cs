using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    public static Fade Instance;

    [SerializeField] Image fadeImage;
    [SerializeField] float duration;
    [SerializeField] float delay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void FadeToScene(int scene)
    {
        fadeImage.enabled = true;
        fadeImage.color = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("selected area")}").backgroundColor;
        fadeImage.DOPause();
        fadeImage.DOFade(1, duration).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(scene));
    }  

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.DOFade(0, duration).SetEase(Ease.Linear).SetDelay(delay).OnComplete(() => fadeImage.enabled = false);
    }
}
