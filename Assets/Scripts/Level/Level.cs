using System.Collections;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string Name;
    public DelayedUnityEvent OnStartLoad;
    public DelayedUnityEvent AfterLoad;
    private Camera _mainCamera;
    private CameraUtils _cameraUtils;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraUtils = FindAnyObjectByType<CameraUtils>();
    }

    public void LoadLevel(string spawnPoint)
    {
        var point = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).
            Where(x => x.ID == spawnPoint).FirstOrDefault();
        if (point == null)
            throw new System.Exception($"Spawn point with id '{spawnPoint}' not found");
        
        StartCoroutine(TransitionToLevel(point));
    }

    private IEnumerator TransitionToLevel(SpawnPoint entryPoint)
    {
        OnStartLoad?.Invoke();

        var player = FindAnyObjectByType<PlayerController2D>();
        player.IsNeedStop = true;
        // ���������� ������
        _cameraUtils.HideScreen(1.0f);
        yield return new WaitForSeconds(1.0f);

        // ������������ ��������� � ������
        player.transform.position = entryPoint.transform.position;
        Camera.main.transform.position = transform.position;

        // ����� ������
        _cameraUtils.ShowScreen(1.0f);
        yield return new WaitForSeconds(1.0f);
        player.IsNeedStop = false;

        AfterLoad?.Invoke();
    }

    void OnDrawGizmos()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null || !_mainCamera.orthographic)
            return;

        // ������������� ���� Gizmos
        Gizmos.color = Color.red;

        // ��������� ������� ������
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        // ���������� ������ ���� ������� �������
        Vector3 bottomLeft = transform.position + new Vector3(-width / 2, -height / 2, 0);
        Vector3 bottomRight = transform.position + new Vector3(width / 2, -height / 2, 0);
        Vector3 topLeft = transform.position + new Vector3(-width / 2, height / 2, 0);
        Vector3 topRight = transform.position + new Vector3(width / 2, height / 2, 0);

        // ������ ����� ����� ������
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
