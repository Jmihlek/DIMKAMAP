using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMover : MonoBehaviour
{
    [Header("���� �� �������, �� ������������ ����� �� �����")]
    public GameObject prefabToSpawn;
    public UnityEvent AfterSpawn;
    public bool IsSpawnOnAwake = false;

    private void Awake()
    {
        if (IsSpawnOnAwake)
            PerformSpawnOrMove();
    }

    public void PerformSpawnOrMove()
    {
        if (prefabToSpawn != null)
        {
            // ������� ����� ��������� ������� � ������� SpawnPoint
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
        else
        {
            var existingObject = FindAnyObjectByType<PlayerController2D>();
            if (existingObject != null)
            {
                // ���������� ������������ ������ � ������� SpawnPoint
                existingObject.transform.position = transform.position;
            }
        }
        AfterSpawn?.Invoke();
    }
}
