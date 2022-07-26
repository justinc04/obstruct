using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text cubesText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCubesText(int value)
    {
        cubesText.text = value.ToString();
    }
}
