using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameState;

    [HideInInspector] public AreaObject area;
    [HideInInspector] public int starsEarned;
    [HideInInspector] public int gemsEarned;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadData();

        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnObstacles:
                GridManager.Instance.SpawnObstacles();
                break;
            case GameState.SpawnEnemy:
                UnitManager.Instance.SpawnEnemy();
                break;
            case GameState.Overview:
                UnitManager.Instance.Overview();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                UnitManager.Instance.MoveEnemy();
                break;
            case GameState.Won:
                UnitManager.Instance.CalculateStars();
                UIManager.Instance.Won();
                UpdateData();
                break;
            case GameState.Lost:
                UIManager.Instance.Lost();
                break;
        }
    }

    void LoadData()
    {
        area = Resources.Load<AreaObject>($"Areas/{PlayerPrefs.GetInt("area")}");

        GridManager.Instance.Initialize();
        UnitManager.Instance.Initialize();
    }

    public void UpdateData()
    {
        PlayerPrefs.SetInt($"stars{PlayerPrefs.GetInt("area")}", PlayerPrefs.GetInt($"stars{PlayerPrefs.GetInt("area")}") + starsEarned);

        if (PlayerPrefs.GetInt($"stars{PlayerPrefs.GetInt("area")}") >= area.starsToComplete && PlayerPrefs.GetInt("area") < Resources.LoadAll("Areas").Length)
        {
            PlayerPrefs.SetInt("area", PlayerPrefs.GetInt("area") + 1);  
        }

        gemsEarned = area.gemValues[starsEarned - 1];
        PlayerPrefs.SetInt("gems", PlayerPrefs.GetInt("gems") + gemsEarned);
    }
}

public enum GameState
{
    Initialize,
    GenerateGrid,
    SpawnObstacles,
    SpawnEnemy,
    Overview,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost,
}
