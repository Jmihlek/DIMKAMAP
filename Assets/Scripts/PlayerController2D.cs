using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController2D : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    public Tilemap[] WaterTiles;
    // Смещение для точки проверки на воде
    public Vector2 WaterCheckOffset;

    private BoxCollider2D _boxCollider2D;
    private PlayerInput _input;

    private void Awake()
    {
        _input = FindAnyObjectByType<PlayerInput>();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input.SwitchCurrentActionMap("Player");
    }

    void Update()
    {
        var moveInput = _input.actions["Move"].ReadValue<Vector2>();
        _movement = Vector2Int.RoundToInt(moveInput);
        // Нормализуем вектор движения, чтобы избежать ускорения по диагонали
        if (_movement.sqrMagnitude > 1)
        {
            _movement.Normalize();
        }

        // Изменение направления взгляда
        if (_movement.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_movement.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (LevelTransitionTrigger.PlayerInteraction)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + _movement * MoveSpeed * Time.deltaTime;

        // Движение по отдельности чтобы избежать трения
        currentPosition.x = newPosition.x;
        currentPosition.y = newPosition.y;

        _rb.MovePosition(currentPosition);

        // Проверка нахождение на воде
        if (IsOnWater(currentPosition))
        {
            Debug.Log("Player is on water and should die.");
            // Здесь будет логика смерти игрока
        }
    }

    private bool IsOnWater(Vector2 position)
    {
        Vector2 checkPosition = position + WaterCheckOffset;
        foreach (var tilemap in WaterTiles)
        {
            Vector3Int tilePosition = tilemap.WorldToCell(checkPosition);
            if (tilemap.HasTile(tilePosition))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 checkPosition = (Vector2)transform.position + WaterCheckOffset;
        Gizmos.DrawSphere(checkPosition, 0.1f);
    }
}
