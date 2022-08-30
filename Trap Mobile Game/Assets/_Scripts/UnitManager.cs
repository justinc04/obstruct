using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    
    [SerializeField] int stones;

    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject stonePrefab;

    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera viewCam;

    private Enemy enemy;
    private int currentStones;
    [HideInInspector] public int stars;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentStones = stones;
        UIManager.Instance.SetStonesText(currentStones);
        viewCam.m_Lens.OrthographicSize = GridManager.Instance.size * 1.7f;
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

            if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(cam.ScreenToWorldPoint(touch.position), cam.transform.forward, out RaycastHit hit))
                {
                    if (hit.transform.gameObject.GetComponent<Tile>().occupied)
                    {
                        return;
                    }

                    SpawnStone(hit.transform.gameObject.GetComponent<Tile>());
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
        if (currentStones == 0)
        {
            stars = 1;
        }
        else if (currentStones < 3)
        {
            stars = 2;
        }
        else
        {
            stars = 3;
        }
    }
}