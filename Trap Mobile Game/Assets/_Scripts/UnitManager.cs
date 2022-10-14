using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    
    private int stones;

    [SerializeField] Enemy enemyPrefab;

    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera viewCam;

    private GameObject stonePrefab;
    private Enemy enemy;
    private int currentStones;

    private RaycastHit initialTouch;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        stones = GameManager.Instance.area.stones;
        cam.backgroundColor = GameManager.Instance.area.backgroundColor;
    }

    private void Start()
    {
        currentStones = stones;
        UIManager.Instance.SetStonesText(currentStones);
        viewCam.m_Lens.OrthographicSize = GameManager.Instance.area.gridSize * 1.7f;
        stonePrefab = Resources.Load<GameObject>($"Stones/{PlayerPrefs.GetInt("stone")}");
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.PlayerTurn)
        {
            return;
        }

        if (currentStones == 0)
        {
            GameManager.Instance.ChangeState(GameState.Lost);
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(cam.ScreenToWorldPoint(touch.position), cam.transform.forward, out RaycastHit hit))
                {
                    initialTouch = hit;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(cam.ScreenToWorldPoint(touch.position), cam.transform.forward, out RaycastHit hit))
                {
                    if (initialTouch.transform != hit.transform|| initialTouch.transform == null || hit.transform.GetComponent<Tile>().occupied)
                    {
                        return;
                    }

                    SpawnStone(hit.transform.GetComponent<Tile>());
                }        
            }
        }
    }

    public void SpawnEnemy()
    {
        enemy = Instantiate(enemyPrefab, GridManager.Instance.GetTileAtPosition(Vector2.zero).pivot.position + .5f * enemyPrefab.transform.localScale.y * Vector3.up, Quaternion.identity);
        GridManager.Instance.GetTileAtPosition(Vector2.zero).occupied = true;
        followCam.LookAt = enemy.transform;
        followCam.Follow = enemy.transform;

        GameManager.Instance.ChangeState(GameState.Overview);
    }

    void SpawnStone(Tile hitTile)
    {
        GameObject stone = Instantiate(stonePrefab, hitTile.pivot.position - .25f * Vector3.up, Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
        hitTile.occupied = true;
        stone.transform.DOMoveY(hitTile.pivot.position.y, .2f).SetEase(Ease.OutSine);
        currentStones--;
        UIManager.Instance.SetStonesText(currentStones);
        GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void MoveEnemy()
    {
        enemy.Move();
    }

    public async void Overview()
    {
        await Task.Delay(1500);
        followCam.Priority = 2;
        await Task.Delay(1000);
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    public void CalculateStars()
    {
        if (currentStones >= GameManager.Instance.area.starValues[1])
        {
            GameManager.Instance.starsEarned = 3;
        }
        else if (currentStones >= GameManager.Instance.area.starValues[0])
        {
            GameManager.Instance.starsEarned = 2;
        }
        else
        {
            GameManager.Instance.starsEarned = 1;
        }
    }
}