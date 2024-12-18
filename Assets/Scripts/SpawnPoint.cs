using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    [Header("Если не указано, то используется игрок на сцене")]
    public GameObject prefabToSpawn;
    public UnityEvent AfterSpawn; 

    public void SpawnOrMovePlayer()
    {
        if (prefabToSpawn != null)
        {
            // Создаем новый экземпляр префаба в позиции SpawnPoint
            Instantiate(prefabToSpawn, transform.position, transform.rotation);

        } else {
            var existingObject = FindAnyObjectByType<PlayerController2D>();
            if (existingObject != null)
            {
                // Перемещаем существующий объект в позицию SpawnPoint
                existingObject.transform.position = transform.position;
            }
        }
        AfterSpawn?.Invoke();
    }
}
