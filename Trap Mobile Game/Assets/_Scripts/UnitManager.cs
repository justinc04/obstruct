using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    
    [SerializeField] int cubes;

    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject cubePrefab;

    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera viewCam;

    private Enemy enemy;
    private int currentCubes;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentCubes = cubes;
        UIManager.Instance.SetCubesText(currentCubes);
        viewCam.m_Lens.OrthographicSize = GridManager.Instance.size * 1.7f;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.PlayerTurn)
        {
            return;
        }

        if (currentCubes == 0)
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

                    SpawnCube(hit.transform.gameObject.GetComponent<Tile>());
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

    void SpawnCube(Tile hitTile)
    {
        GameObject cube = Instantiate(cubePrefab, hitTile.pivot.position + .5f * cubePrefab.transform.localScale.y * Vector3.down, Quaternion.identity);
        hitTile.occupied = true;
        cube.transform.DOMove(hitTile.pivot.position + .5f * cubePrefab.transform.localScale.y * Vector3.up, .2f).SetEase(Ease.OutSine);
        currentCubes--;
        UIManager.Instance.SetCubesText(currentCubes);
        GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void MoveEnemy()
    {
        enemy.Move();
    }

    public async void Overview()
    {
        await Task.Delay(2000);
        followCam.Priority = 2;
        await Task.Delay(1000);
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }
}