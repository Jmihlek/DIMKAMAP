using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileMover : MonoBehaviour
{
    public Tilemap[] MoveTragetsTilemaps;  // ������ ��������� � �����
    public Tilemap mainTilemap;  // �������� �������� ��� ���������� ��������
    public GameObject cursorObject;  // ������ �������
    public Vector3Int cursorPosition;  // ������� ������� �� ��������

    private Tile currentTile;  // ����, ������� ������������
    private Vector3Int initialPosition;  // ��������� ������� ������������� �����
    private bool isHoldingTile = false;  // ����, ������������ �� ����
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
            // ������������� ������������ ���� �� �������� ��������
            mainTilemap.SetTile(cursorPosition, currentTile);

            // ���������� ���� ���� �� ���� ��������� �������
            if (waterTilemap.GetTile(initialPosition) == null)
                waterTilemap.SetTile(initialPosition, waterTilemap.GetTile(cursorPosition));

            // ������� ���� ���� �� ������� ������� �������
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
            // ��������� ������� ��������� ������� �������
            Vector3 worldPosition = mainTilemap.CellToWorld(cursorPosition);
            worldPosition += mainTilemap.tileAnchor; // ���� �������� ����� �����

            // ��������� ����� Gizmos
            Gizmos.color = Color.green;

            // ��������� ���� � ������� �������
            Gizmos.DrawWireCube(worldPosition, mainTilemap.cellSize);
        }
    }
}
