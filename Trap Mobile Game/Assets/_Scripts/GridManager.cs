using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int size;
    [SerializeField] int obstacles;
    [SerializeField] Tile grassTile, waterTile;

    private Dictionary<Vector2, Tile> tiles;
    private List<Vector2> obstaclePositions;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        int radius = size / 2;
        GenerateObstaclePositions(radius);

        tiles = new Dictionary<Vector2, Tile>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (obstaclePositions.Contains(new Vector2(x, y)))
                {
                    continue;
                }

                SpawnTile(grassTile, new Vector2(x, y));
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


    void GenerateObstaclePositions(int radius)
    {
        obstaclePositions = new List<Vector2>();

        for (int i = 0; i < obstacles; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1));
            bool edgeTile = Mathf.Abs(randomPos.x) == radius || Mathf.Abs(randomPos.y) == radius;
            bool centerTile = Mathf.Abs(randomPos.x) <= 1 && Mathf.Abs(randomPos.y) <= 1;

            while (obstaclePositions.Contains(randomPos) || edgeTile || centerTile)
            {
                randomPos = new Vector2(Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1));
                edgeTile = Mathf.Abs(randomPos.x) == radius || Mathf.Abs(randomPos.y) == radius;
                centerTile = Mathf.Abs(randomPos.x) <= 1 && Mathf.Abs(randomPos.y) <= 1;
            }

            obstaclePositions.Add(randomPos);
        }
    }

    public void SpawnObstacles()
    {
        foreach (Vector2 pos in obstaclePositions)
        {
            int randomType = Random.Range(0, 2);

            if (randomType == 0)
            {
                bool spawnObstacle = Random.Range(0, 3) == 0;
                
                if (spawnObstacle)
                {
                    SpawnTile(waterTile, pos).SpawnObstacle();
                }
                else
                {
                    SpawnTile(waterTile, pos);
                }
            }
            else
            {
                SpawnTile(grassTile, pos).SpawnObstacle();
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
