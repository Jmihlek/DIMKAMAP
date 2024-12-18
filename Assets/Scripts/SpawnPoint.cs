using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    [Header("���� �� �������, �� ������������ ����� �� �����")]
    public GameObject prefabToSpawn;
    public UnityEvent AfterSpawn; 

    public void SpawnOrMovePlayer()
    {
        if (prefabToSpawn != null)
        {
            // ������� ����� ��������� ������� � ������� SpawnPoint
            Instantiate(prefabToSpawn, transform.position, transform.rotation);

        } else {
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
