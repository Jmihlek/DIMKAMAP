using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileMover : MonoBehaviour
{
    public Tilemap[] MoveTragetsTilemaps;  // Массив тайлмапов с водой
    public Tilemap mainTilemap;  // Основная тайлмапа для размещения объектов
    public GameObject cursorObject;  // Объект курсора
    public Vector3Int cursorPosition;  // Позиция курсора на тайлмапе

    private Tile currentTile;  // Тайл, который перемещается
    private Vector3Int initialPosition;  // Начальная позиция перемещаемого тайла
    private bool isHoldingTile = false;  // Флаг, удерживается ли тайл
    private PlayerInput _input;

    private void Awake()
    {
        _input = FindAnyObjectByType<PlayerInput>();
    }

    void Start()
    {
        cursorObject.SetActive(false);
        UpdateCursor();
    }

    private void OnEnable()
    {
        _input.actions["ExitShip"].performed += OnExitInputPressed;
        _input.actions["MoveShip"].performed += OnMoveInput;
        _input.actions["InteractShip"].performed += OnInteractInput;
    }

    private void OnDisable()
    {
        _input.actions["ExitShip"].performed -= OnExitInputPressed;
        _input.actions["MoveShip"].performed -= OnMoveInput;
        _input.actions["InteractShip"].performed -= OnInteractInput;
    }

    private void OnInteractInput(InputAction.CallbackContext obj)
    {
        if (!isHoldingTile)
        {
            PickUpTile();
        }
        else
        {
            PlaceTile();
        }
    }

    private void OnMoveInput(InputAction.CallbackContext obj)
    {
        var moveInput = _input.actions["MoveShip"].ReadValue<Vector2>();

        var moveDirection = Vector3Int.RoundToInt(new Vector3(moveInput.x, moveInput.y, 0));
        if (moveDirection != Vector3Int.zero)
        {
            MoveCursor(moveDirection);
        }
    }

    private void OnExitInputPressed(InputAction.CallbackContext obj)
        => EndMoveMode();

    public void StartMoveMode()
    {
        _input.SwitchCurrentActionMap("Ship");
        cursorObject.SetActive(true);
    }

    public void EndMoveMode()
    {
        _input.SwitchCurrentActionMap("Player");
        cursorObject.SetActive(false);
    }

    void MoveCursor(Vector3Int direction)
    {
        cursorPosition += direction;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (cursorObject != null && mainTilemap != null)
        {
            Vector3 worldPosition = mainTilemap.CellToWorld(cursorPosition);
            cursorObject.transform.position = worldPosition + mainTilemap.cellSize / 2;
        }
    }

    void PickUpTile()
    {
        currentTile = mainTilemap.GetTile<Tile>(cursorPosition);
        if (currentTile != null)
        {
            initialPosition = cursorPosition;
            mainTilemap.SetTile(cursorPosition, null);
            isHoldingTile = true;
        }
    }

    void PlaceTile()
    {
        if (currentTile != null && CanPlaceTile(out Tilemap waterTilemap))
        {
            // Устанавливаем перемещаемый тайл на основную тайлмапу
            mainTilemap.SetTile(cursorPosition, currentTile);

            // Возвращаем тайл воды на свою начальную позицию
            if (waterTilemap.GetTile(initialPosition) == null)
                waterTilemap.SetTile(initialPosition, waterTilemap.GetTile(cursorPosition));

            // Удаляем тайл воды из текущей позиции курсора
            waterTilemap.SetTile(cursorPosition, null);
            currentTile = null;

            isHoldingTile = false;
        }
    }

    bool CanPlaceTile(out Tilemap validWaterTilemap)
    {
        foreach (var waterTilemap in MoveTragetsTilemaps)
        {
            if (waterTilemap.GetTile(cursorPosition) != null)
            {
                validWaterTilemap = waterTilemap;
                return true;
            }
        }
        validWaterTilemap = null;
        return false;
    }

    void OnDrawGizmos()
    {
        if (mainTilemap != null)
        {
            // Получение мировых координат позиции курсора
            Vector3 worldPosition = mainTilemap.CellToWorld(cursorPosition);
            worldPosition += mainTilemap.tileAnchor; // Учёт смещения якоря тайла

            // Установка цвета Gizmos
            Gizmos.color = Color.green;

            // Рисование куба в позиции курсора
            Gizmos.DrawWireCube(worldPosition, mainTilemap.cellSize);
        }
    }
}
