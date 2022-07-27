using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text stonesText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetStonesText(int value)
    {
        stonesText.text = value.ToString();
    }
}
