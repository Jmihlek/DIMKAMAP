using System.Collections;
using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour
{
    public Transform TargetPosition; // Целевая позиция для перемещения игрока
    public SpriteRenderer CurrentCover; // Спрайт, заслоняющий текущий уровень
    public SpriteRenderer NextCover; // Спрайт, заслоняющий следующий уровень
    public float FadeOutDuration = 1f; // Длительность скрытия текущего уровня
    public float FadeInDuration = 1f; // Длительность появления следующего уровня

    private Camera _camera;
    public static bool PlayerInteraction = false;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController2D>(out var player) && !PlayerInteraction)
        {
            PlayerInteraction = true;
            StartCoroutine(StartTransition(player.transform));
        }
    }

    private IEnumerator StartTransition(Transform player)
    {
        // Плавно затемняем текущий уровень
        yield return StartCoroutine(Fade(CurrentCover, 0f, 1f, FadeOutDuration));

        // Перемещаем игрока на целевую позицию
        player.position = TargetPosition.position;

        // Плавно отображаем следующий уровень
        yield return StartCoroutine(Fade(NextCover, 1f, 0f, FadeInDuration));

        // Устанавливаем флаг, что игрок вышел из триггера
        PlayerInteraction = false;
    }

    private IEnumerator Fade(SpriteRenderer cover, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = cover.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Плавное изменение прозрачности
            cover.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, t));

            yield return null;
        }

        cover.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
