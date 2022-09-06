using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private int size;
    private int radius;
    private int obstacles;
    private Tile primaryTile, secondaryTile;

    private Dictionary<Vector2, Tile> tiles;
    private List<Vector2> obstaclePositions;

    private void Awake()
    {
        Instance = this;
        size = GameManager.Instance.area.gridSize;
        radius = size / 2;
        obstacles = GameManager.Instance.area.obstacles;
        primaryTile = GameManager.Instance.area.primaryTile;
        secondaryTile = GameManager.Instance.area.secondaryTile;
    }

    public void GenerateGrid()
    {
        GenerateObstaclePositions();

        tiles = new Dictionary<Vector2, Tile>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (obstaclePositions.Contains(new Vector2(x, y)))
                {
                    continue;
                }

                SpawnTile(primaryTile, new Vector2(x, y));
            }
        }

        GameManager.Instance.ChangeState(GameState.SpawnObstacles);
    }

    Tile SpawnTile(Tile tile, Vector2 pos)
    {
        Tile spawnedTile = Instantiate(tile, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        spawnedTile.name = $"Tile {pos}";
        spawnedTile.Init(pos);
        tiles[pos] = spawnedTile;

        return spawnedTile;
    }

    void GenerateObstaclePositions()
    {
        obstaclePositions = new List<Vector2>();
        int centerObstacles = 0;

        for (int i = 0; i < obstacles; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1));
            bool centerTile = Mathf.Abs(randomPos.x) <= 1 && Mathf.Abs(randomPos.y) <= 1;

            while (obstaclePositions.Contains(randomPos) || randomPos == Vector2.zero || centerTile && centerObstacles > 0)
            {
                randomPos = new Vector2(Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1));
                centerTile = Mathf.Abs(randomPos.x) <= 1 && Mathf.Abs(randomPos.y) <= 1;
            }

            if (centerTile)
            {
                centerObstacles++;
            }

            obstaclePositions.Add(randomPos);
        }
    }

    public void SpawnObstacles()
    {
        int waterTiles = (int)(obstacles * Random.Range(.3f, .7f));

        foreach (Vector2 pos in obstaclePositions)
        {
            bool edgeTile = Mathf.Abs(pos.x) == radius || Mathf.Abs(pos.y) == radius;

            if (waterTiles > 0 && !edgeTile)
            {
                bool spawnObstacle = Random.Range(0, 3) == 0;
                
                if (spawnObstacle)
                {
                    SpawnTile(secondaryTile, pos).SpawnObstacle();
                }
                else
                {
                    SpawnTile(secondaryTile, pos);
                }

                waterTiles--;
            }
            else
            {
                SpawnTile(primaryTile, pos).SpawnObstacle();
            }
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemy);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }

        return null;
    }

    public bool CheckPositionInBounds(Vector2 pos)
    {
        return Mathf.Abs(pos.x) <= size / 2 && Mathf.Abs(pos.y) <= size / 2;
    }
}
