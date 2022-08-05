using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
                Invoke("HandleGameOver", 4);
                break;
            case GameState.Lost:
                Invoke("HandleGameOver", 4);
                break;
        }
    }

    void HandleGameOver()
    {
        Fade.Instance.FadeToScene(0);
    }
}

public enum GameState
{
    GenerateGrid,
    SpawnObstacles,
    SpawnEnemy,
    Overview,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost,
}
