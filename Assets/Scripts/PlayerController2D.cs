using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController2D : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    public Tilemap[] GroundTiles;
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        GroundTiles = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // ѕолучаем ввод от пользовател€
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        // Ќормализуем вектор движени€, чтобы избежать ускорени€ по диагонали
        if (_movement.sqrMagnitude > 1)
        {
            _movement.Normalize();
        }
    }

    void FixedUpdate()
    {
        if (LevelTransitionTrigger.PlayerInteraction)
            return;
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + _movement * MoveSpeed * Time.deltaTime;

        // ѕровер€ем возможность движени€ по горизонтали и вертикали
        currentPosition.x = CanMoveTo(new Vector2(newPosition.x, currentPosition.y)) ? newPosition.x : currentPosition.x;
        currentPosition.y = CanMoveTo(new Vector2(currentPosition.x, newPosition.y)) ? newPosition.y : currentPosition.y;

        _rb.MovePosition(currentPosition);
    }

    private bool CanMoveTo(Vector2 targetPosition)
    {
        // ѕолучаем границы и смещение коллайдера
        Bounds bounds = _boxCollider2D.bounds;
        Vector2 size = bounds.size;
        Vector2 offset = _boxCollider2D.offset;

        // —мещаем границы коллайдера в новую позицию
        Vector2 bottomLeft = targetPosition + new Vector2(-size.x / 2, -size.y / 2) + offset;
        Vector2 bottomRight = targetPosition + new Vector2(size.x / 2, -size.y / 2) + offset;
        Vector2 topLeft = targetPosition + new Vector2(-size.x / 2, size.y / 2) + offset;
        Vector2 topRight = targetPosition + new Vector2(size.x / 2, size.y / 2) + offset;

        // ѕровер€ем наличие тайлов по границам коллайдера
        return IsTilePresent(bottomLeft) && IsTilePresent(bottomRight) && IsTilePresent(topLeft) && IsTilePresent(topRight);
    }

    private bool IsTilePresent(Vector2 position)
    {
        foreach (var tilemap in GroundTiles)
        {
            Vector3Int tilePosition = tilemap.WorldToCell(position);
            if (tilemap.HasTile(tilePosition))
            {
                return true;
            }
        }
        return false;
    }
}
