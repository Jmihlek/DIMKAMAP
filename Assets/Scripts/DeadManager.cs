using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> deathMessages; // Список сообщений о смерти
    [SerializeField] private List<GameObject> areas; // Список областей
    private PlayerController2D _player;
    private PlayerInput _input;

    private void OnEnable()
    {
        _player = FindAnyObjectByType<PlayerController2D>();
        _player.OnDead += _player_OnDead;

        _input = FindAnyObjectByType<PlayerInput>();
        _input.actions["Restart"].performed += InputRestart;
    }

    private void InputRestart(InputAction.CallbackContext obj)
    {
        RestartLevel();
    }

    private void OnDisable()
    {
        _player.OnDead -= _player_OnDead;
        _input.actions["Restart"].performed -= InputRestart;
    }

    private void _player_OnDead()
    {
        if (areas.Count == 0 || deathMessages.Count == 0)
        {
            Debug.LogWarning("No areas or death messages set.");
            return;
        }

        // Найти ближайшую область к игроку
        GameObject closestArea = FindClosestArea(_player.transform.position);

        if (closestArea != null)
        {
            // Выбрать случайное сообщение о смерти
            GameObject randomMessage = deathMessages[Random.Range(0, deathMessages.Count)];

            randomMessage.SetActive(true);
            randomMessage.transform.position = closestArea.transform.position;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Метод для поиска ближайшей области
    private GameObject FindClosestArea(Vector3 position)
    {
        GameObject closestArea = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject area in areas)
        {
            float distance = Vector3.Distance(position, area.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestArea = area;
            }
        }

        return closestArea;
    }
}
