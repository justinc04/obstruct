using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveDelay;
    [SerializeField] float deathDelay;
    [SerializeField] float moveSpeed;

    [HideInInspector] public Vector2 gridPosition;

    public void Move()
    {
        GridManager.Instance.GetTileAtPosition(gridPosition).occupied = false;

        List<Tile> possibleMoves = new List<Tile>();
        List<Tile> goodMoves = new List<Tile>();
        List<Tile> badMoves = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || !GridManager.Instance.CheckPositionInBounds(gridPosition + new Vector2(x, y)) || GridManager.Instance.GetTileAtPosition(gridPosition + new Vector2(x, y)).occupied)
                {
                    continue;
                }

                possibleMoves.Add(GridManager.Instance.GetTileAtPosition(gridPosition + new Vector2(x, y)));
            }
        }

        if (possibleMoves.Count == 0)
        {
            transform.DOShakePosition(.3f, .05f, 20, fadeOut: false).SetDelay(deathDelay);
            GameManager.Instance.ChangeState(GameState.Won);
            return;
        }
        
        foreach (Tile tile in possibleMoves)
        {
            if(CheckMoves(tile) == 1)
            {
                badMoves.Add(tile);
            }
            else
            {
                goodMoves.Add(tile);
            }
        }

        Tile targetTile;

        if (goodMoves.Count > 0)
        {
            targetTile = goodMoves[Random.Range(0, goodMoves.Count)];
        }
        else
        {
            targetTile = badMoves[Random.Range(0, badMoves.Count)];
        }

        Vector3 targetPos = targetTile.pivot.position + .5f * gameObject.transform.localScale.y * Vector3.up;
        gridPosition = targetTile.gridPosition;
        targetTile.occupied = true;
        transform.DOMove(targetPos, moveSpeed).SetSpeedBased(true).SetEase(Ease.InOutSine).SetDelay(moveDelay).OnComplete(() => GameManager.Instance.ChangeState(GameState.PlayerTurn));
    }

    int CheckMoves(Tile tile)
    {
        int moves = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || !GridManager.Instance.CheckPositionInBounds(tile.gridPosition + new Vector2(x, y)) || GridManager.Instance.GetTileAtPosition(tile.gridPosition + new Vector2(x, y)).occupied)
                {
                    continue;
                }

                moves++;
            }
        }

        return moves;
    }
}
