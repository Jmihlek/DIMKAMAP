using System.Collections;
using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour
{
    public Transform TargetPosition; // ������� ������� ��� ����������� ������
    public SpriteRenderer CurrentCover; // ������, ����������� ������� �������
    public SpriteRenderer NextCover; // ������, ����������� ��������� �������
    public float FadeOutDuration = 1f; // ������������ ������� �������� ������
    public float FadeInDuration = 1f; // ������������ ��������� ���������� ������

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
        // ������ ��������� ������� �������
        yield return StartCoroutine(Fade(CurrentCover, 0f, 1f, FadeOutDuration));

        // ���������� ������ �� ������� �������
        player.position = TargetPosition.position;

        // ������ ���������� ��������� �������
        yield return StartCoroutine(Fade(NextCover, 1f, 0f, FadeInDuration));

        // ������������� ����, ��� ����� ����� �� ��������
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

            // ������� ��������� ������������
            cover.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, t));

            yield return null;
        }

        cover.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
