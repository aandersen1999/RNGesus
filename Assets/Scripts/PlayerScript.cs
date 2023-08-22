using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private PlayerColor color;
    public Color spriteColor;
    public ParticleSystem confetti;
    public PlayerColor Color { get { return color; } }
    [SerializeField] private TileCheck nextTile = TileCheck.Right;
    public TileCheck NextTile { get { return nextTile; } set { nextTile = value; } }
    public Vector3 targetPos;
    private Vector3 vel = Vector3.zero;

    private const float moveTimer = .25f;

    private void Start()
    {
        GameManager.Instance.SetPlayer(this);
        targetPos = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, .05f);
    }

    public IEnumerator Timer(int tileMoves)
    {
        Tilemap tilemap = GameManager.Instance.Map;
        for (int i = 1; i <= tileMoves; i++)
        {
            yield return new WaitForSeconds(moveTimer);
            Vector3Int pos = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            tilemap.RefreshTile(pos);

            VariableTile tile = tilemap.GetTile<VariableTile>(pos);

            MusicManager.Instance.PlaySound(MusicManager.Instance.playerMoveSound, 0.1f);

            if (tile.HasAdjacentNeighbor(nextTile))
            {
                targetPos = pos + GetDir(nextTile);
            }
            else
            {
                switch (nextTile)
                {
                    case TileCheck.Up:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Down;
                        break;
                    case TileCheck.Left:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Right;
                        break;
                    case TileCheck.Down:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Up;
                        break;
                    case TileCheck.Right:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Left;
                        break;

                }

                targetPos = pos + GetDir(nextTile);
            }
        }

        yield return new WaitForSeconds(moveTimer);

        Vector3Int pos2 = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        tilemap.RefreshTile(pos2);

        VariableTile tile2 = tilemap.GetTile<VariableTile>(pos2);
        tile2.Landed(this);

        yield return new WaitForSeconds(moveTimer);
        GameManager.Instance.PassTurn();
    }

    public void AdvanceSpaces(int spaces)
    {
        Tilemap tilemap = GameManager.Instance.Map;
        for(int i = 0; i < spaces; i++)
        {
            Vector3Int pos = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            tilemap.RefreshTile(pos);

            VariableTile tile = tilemap.GetTile<VariableTile>(pos);

            MusicManager.Instance.PlaySound(MusicManager.Instance.playerMoveSound, 0.1f);

            if (tile.HasAdjacentNeighbor(nextTile))
            {
                transform.position = pos + GetDir(nextTile);
            }
            else
            {
                switch (nextTile)
                {
                    case TileCheck.Up:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Down;
                        break;
                    case TileCheck.Left:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Right;
                        break;
                    case TileCheck.Down:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Up;
                        break;
                    case TileCheck.Right:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Left;
                        break;

                }

                transform.position = pos + GetDir(nextTile);
            }
        }
        targetPos = transform.position;
        Vector3Int pos2 = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        tilemap.RefreshTile(pos2);

        VariableTile tile2 = tilemap.GetTile<VariableTile>(pos2);
        tile2.Landed(this);
    }

    public void GoBackSpaces(int spaces)
    {
        Tilemap tilemap = GameManager.Instance.Map;
        for (int i = 0; i < spaces; i++)
        {
            Vector3Int pos = new((int)transform.position.x, (int)transform.position.y);
            tilemap.RefreshTile(pos);

            VariableTile tile = tilemap.GetTile<VariableTile>(pos);

            MusicManager.Instance.PlaySound(MusicManager.Instance.playerMoveSound, 0.1f);

            if (tile.HasAdjacentNeighbor(nextTile))
            {
                transform.position = pos + GetOppositeDir(nextTile);
            }
            else
            {
                switch (nextTile)
                {
                    case TileCheck.Up:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Down;
                        break;
                    case TileCheck.Left:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Right;
                        break;
                    case TileCheck.Down:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Up;
                        break;
                    case TileCheck.Right:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Left;
                        break;

                }

                transform.position = pos + GetOppositeDir(nextTile);
            }
        }
        targetPos = transform.position;
        Vector3Int pos2 = new((int)transform.position.x, (int)transform.position.y);
        tilemap.RefreshTile(pos2);

        VariableTile tile2 = tilemap.GetTile<VariableTile>(pos2);
        tile2.Landed(this);
    }

    private Vector3Int GetDir(TileCheck check)
    {
        return check switch
        {
            TileCheck.Left => Vector3Int.left,
            TileCheck.Right => Vector3Int.right,
            TileCheck.Up => Vector3Int.up,
            TileCheck.Down => Vector3Int.down,
            _ => Vector3Int.down
        };
    }

    private Vector3Int GetOppositeDir(TileCheck check)
    {
        return check switch
        {
            TileCheck.Left => Vector3Int.right,
            TileCheck.Right => Vector3Int.left,
            TileCheck.Up => Vector3Int.down,
            TileCheck.Down => Vector3Int.up,
            _ => Vector3Int.down
        };
    }
}

public enum PlayerColor
{
    Red,
    Blue,
    Green,
    Yellow
}